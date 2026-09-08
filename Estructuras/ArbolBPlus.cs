using System;
using System.Collections.Generic;

namespace PRY1.Estructuras
{

// CLASE AUXILIAR OBLIGATORIA: Emula las listas de Python usando arreglos puros 
    public class MiLista<T>
    {
        private T[] items;
        public int Count { get; private set; }
        
        public MiLista(int capacidad = 10)
        {
            items = new T[capacidad];
            Count = 0;
        }
        
        public T this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }
        
        public void Add(T item)
        {
            if (Count == items.Length) Ampliar();
            items[Count++] = item;
        }
        
        public void Insert(int index, T item)
        {
            if (Count == items.Length) Ampliar();
            for (int i = Count; i > index; i--) items[i] = items[i - 1];
            items[index] = item;
            Count++;
        }
        
        public void RemoveAt(int index)
        {
            for (int i = index; i < Count - 1; i++) items[i] = items[i + 1];
            Count--;
        }
        
        public T Pop(int index = -1)
        {
            if (index == -1) index = Count - 1;
            T val = items[index];
            RemoveAt(index);
            return val;
        }
        
        public void AddRange(MiLista<T> other)
        {
            for (int i = 0; i < other.Count; i++) Add(other[i]);
        }
        
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
                if (object.Equals(items[i], item)) return i;
            return -1;
        }

        public MiLista<T> GetRange(int start, int count)
        {
            MiLista<T> res = new MiLista<T>(count > 10 ? count : 10);
            for (int i = 0; i < count; i++) res.Add(items[start + i]);
            return res;
        }
        
        public void RemoveRange(int start, int count)
        {
            for (int i = start + count; i < Count; i++) items[i - count] = items[i];
            Count -= count;
        }

        private void Ampliar()
        {
            T[] nuevo = new T[items.Length * 2];
            for (int i = 0; i < Count; i++) nuevo[i] = items[i];
            items = nuevo;
        }
    }


    public class NodoBPlus
    {
        public bool Hoja { get; set; }              // True si contiene datos reales; False si es un nodo guía.
        public List<int> Claves { get; set; }       // Guarda las claves del nodo.
        public List<NodoBPlus> Hijos { get; set; }  // Guarda los hijos cuando el nodo es interno.
        public NodoBPlus Siguiente { get; set; }    // Conecta una hoja con la siguiente.
        public NodoBPlus Padre { get; set; }        // Permite regresar al padre durante una reparación.

        public NodoBPlus(bool hoja = true)
        {
            Hoja = hoja;
            Claves = new List<int>();
            Hijos = new List<NodoBPlus>();
            Siguiente = null;
            Padre = null;
        }
    }

    public class ArbolBPlus
    {
        private int orden;
        private int max_claves;
        private int min_claves_hoja;
        private int min_hijos_interno;
        private NodoBPlus raiz;

        // Ayuda a insertar y buscar en listas ordenadas.
        private int BisectRight(List<int> lista, int clave)
        {
            int i = 0;
            while (i < lista.Count && lista[i] <= clave) i++;
            return i;
        }

        // Ayuda a insertar y buscar en listas ordenadas.
        private int BisectLeft(List<int> lista, int clave)
        {
            int i = 0;
            while (i < lista.Count && lista[i] < clave) i++;
            return i;
        }

        public ArbolBPlus(int orden = 4)
        {
            if (orden < 3)                                         // Comprueba que el orden sea válido.
            {
                throw new ArgumentException("El orden debe ser al menos 3.");
            }
            this.orden = orden;                                    // Máximo número hijos.
            this.max_claves = orden - 1;                           // Máximo de claves por nodo.
            // Permite redondear hacia arriba.
            this.min_claves_hoja = (int)Math.Ceiling((orden - 1) / 2.0); // Mínimo de claves en una hoja.
            this.min_hijos_interno = (int)Math.Ceiling(orden / 2.0);     // Mínimo de hijos internos.
            this.raiz = new NodoBPlus(hoja: true);                 // Al inicio, la raíz también es hoja.
        }

        // BÚSQUEDA DE LA HOJA
        private NodoBPlus _buscar_hoja(int clave)
        {
            // FUNCIÓN CLAVE PARA B+: TODAS LAS BÚSQUEDAS DEBEN TERMINAR EN UNA HOJA. LOS NODOS INTERNOS SON SOLO RUTAS.
            NodoBPlus nodo = this.raiz;                            // Comienza en la raíz.
            while (!nodo.Hoja)                                     // Continúa mientras no sea hoja.
            {
                int posicion = BisectRight(nodo.Claves, clave);    // Determina el rango de la clave.
                nodo = nodo.Hijos[posicion];                       // Baja al hijo correspondiente.
            }
            return nodo;                                           // Devuelve la hoja encontrada.
        }

        // BÚSQUEDA INDIVIDUAL
        public bool Buscar(int clave)
        {
            // SI EN EL EXAMEN TE PIDEN CONFIRMAR EXISTENCIA, USA ESTE MÉTODO DIRECTO.
            NodoBPlus hoja = this._buscar_hoja(clave);             // Localiza la hoja correcta.
            int posicion = BisectLeft(hoja.Claves, clave);         // Busca dónde debería estar.
            return (
                posicion < hoja.Claves.Count                       // Comprueba que la posición exista.
                && hoja.Claves[posicion] == clave                  // Verifica que la clave coincida.
            );
        }

        // INSERCIÓN
        public bool Insertar(int clave)
        {
            if (this.Buscar(clave))                                // Evita claves repetidas.
            {
                return false;
            }

            NodoBPlus hoja = this._buscar_hoja(clave);             // Localiza la hoja correcta.
            int posicion = BisectLeft(hoja.Claves, clave);         // Calcula la posición ordenada.
            hoja.Claves.Insert(posicion, clave);                   // Inserta la clave.

            if (hoja.Claves.Count > this.max_claves)               // Comprueba si se desbordó.
            {
                this._dividir_hoja(hoja);                          // Divide la hoja.
            }

            this._recalcular_guias(this.raiz);                     // Actualiza claves guía.
            return true;
        }

        private void _dividir_hoja(NodoBPlus hoja)
        {
            // ESTE MÉTODO SE EJECUTA AUTOMÁTICAMENTE CUANDO UNA HOJA EXCEDE SU LÍMITE DE CLAVES (ORDEN - 1)
            int punto = (hoja.Claves.Count + 1) / 2;               // Calcula el punto de división.
            NodoBPlus nueva_hoja = new NodoBPlus(hoja: true);      // Crea la hoja derecha.
            nueva_hoja.Padre = hoja.Padre;                         // Conserva el mismo padre.
            nueva_hoja.Claves = hoja.Claves.GetRange(punto, hoja.Claves.Count - punto); // La derecha recibe la segunda parte.
            hoja.Claves = hoja.Claves.GetRange(0, punto);          // La izquierda conserva la primera.
            nueva_hoja.Siguiente = hoja.Siguiente;                 // La nueva hoja apunta a la siguiente.
            hoja.Siguiente = nueva_hoja;                           // La hoja original apunta a la nueva.
            int clave_guia = nueva_hoja.Claves[0];                 // Primera clave de la hoja derecha.
            this._insertar_en_padre(hoja, clave_guia, nueva_hoja);
        }

        private void _insertar_en_padre(NodoBPlus izquierda, int clave_guia, NodoBPlus derecha)
        {
            if (izquierda == this.raiz)                            // Si se dividió la raíz...
            {
                NodoBPlus nueva_raiz = new NodoBPlus(hoja: false); // Crea una raíz interna.
                nueva_raiz.Claves = new List<int> { clave_guia };  // Coloca la clave guía.
                nueva_raiz.Hijos = new List<NodoBPlus> { izquierda, derecha }; // Conecta ambas hojas.
                izquierda.Padre = nueva_raiz;                      // Actualiza el padre izquierdo.
                derecha.Padre = nueva_raiz;                        // Actualiza el padre derecho.
                this.raiz = nueva_raiz;                            // Sustituye la raíz.
                return;
            }

            NodoBPlus padre = izquierda.Padre;                     // Obtiene el padre actual.
            int posicion = padre.Hijos.IndexOf(izquierda);         // Localiza el hijo que se dividió.
            padre.Claves.Insert(posicion, clave_guia);             // Inserta la guía en el padre.
            padre.Hijos.Insert(posicion + 1, derecha);             // Inserta el nuevo hijo derecho.
            derecha.Padre = padre;                                 // Conecta el nuevo nodo con el padre.

            if (padre.Claves.Count > this.max_claves)              // Si el padre se desbordó...
            {
                this._dividir_interno(padre);                      // También debe dividirse.
            }
        }

        private void _dividir_interno(NodoBPlus nodo)
        {
            // A DIFERENCIA DE LAS HOJAS, CUANDO UN NODO INTERNO SE DIVIDE, LA CLAVE CENTRAL SUBE AL PADRE Y NO SE DUPLICA.
            int centro = nodo.Claves.Count / 2;                    // Busca la clave central.
            int clave_que_sube = nodo.Claves[centro];              // Esta clave sube al padre.
            NodoBPlus nuevo_interno = new NodoBPlus(hoja: false);  // Crea el nodo interno derecho.
            nuevo_interno.Padre = nodo.Padre;                      // Conserva el mismo padre.
            nuevo_interno.Claves = nodo.Claves.GetRange(centro + 1, nodo.Claves.Count - (centro + 1)); // Recibe claves de la derecha.
            nuevo_interno.Hijos = nodo.Hijos.GetRange(centro + 1, nodo.Hijos.Count - (centro + 1));    // Recibe hijos de la derecha.

            foreach (NodoBPlus hijo in nuevo_interno.Hijos)        // Recorre los hijos trasladados.
            {
                hijo.Padre = nuevo_interno;                        // Actualiza su nuevo padre.
            }

            nodo.Claves = nodo.Claves.GetRange(0, centro);         // Conserva claves de la izquierda.
            nodo.Hijos = nodo.Hijos.GetRange(0, centro + 1);       // Conserva hijos de la izquierda.
            this._insertar_en_padre(nodo, clave_que_sube, nuevo_interno);
        }

        // ELIMINACIÓN
        public bool Eliminar(int clave)
        {
            NodoBPlus hoja = this._buscar_hoja(clave);             // Busca la hoja correspondiente.
            int posicion = BisectLeft(hoja.Claves, clave);         // Localiza la posición.

            if (posicion >= hoja.Claves.Count || hoja.Claves[posicion] != clave)
            {
                return false;                                      // La clave no existe.
            }

            hoja.Claves.RemoveAt(posicion);                        // Elimina la clave.

            if (hoja == this.raiz)                                 // Si la raíz también es hoja...
            {
                return true;                                       // No necesita reparación.
            }

            if (hoja.Claves.Count < this.min_claves_hoja)          // Comprueba el subdesbordamiento.
            {
                this._reparar_hoja(hoja);                          // Presta o fusiona.
            }

            this._recalcular_guias(this.raiz);                     // Actualiza todas las claves guía.
            return true;
        }

        // REPARAR UNA HOJA
        private void _reparar_hoja(NodoBPlus hoja)
        {
            // LÓGICA DE UNDERFLOW: SI UNA HOJA QUEDA CON MENOS CLAVES DEL MÍNIMO PERMITIDO, PIDE PRESTADO O SE FUSIONA CON HERMANOS.
            NodoBPlus padre = hoja.Padre;                          // Obtiene el padre.
            int posicion = padre.Hijos.IndexOf(hoja);              // Localiza la hoja.

            NodoBPlus hermano_izquierdo = posicion > 0 ? padre.Hijos[posicion - 1] : null;
            NodoBPlus hermano_derecho = posicion + 1 < padre.Hijos.Count ? padre.Hijos[posicion + 1] : null;

            if (hermano_izquierdo != null && hermano_izquierdo.Claves.Count > this.min_claves_hoja)
            {
                int clave_prestada = hermano_izquierdo.Claves[hermano_izquierdo.Claves.Count - 1]; 
                hermano_izquierdo.Claves.RemoveAt(hermano_izquierdo.Claves.Count - 1); // Extrae la mayor clave izquierda.
                hoja.Claves.Insert(0, clave_prestada);             // La agrega al inicio de la hoja.
                return;
            }

            if (hermano_derecho != null && hermano_derecho.Claves.Count > this.min_claves_hoja)
            {
                int clave_prestada = hermano_derecho.Claves[0];
                hermano_derecho.Claves.RemoveAt(0);                // Extrae la menor clave derecha.
                hoja.Claves.Add(clave_prestada);                   // La agrega al final de la hoja.
                return;
            }

            if (hermano_izquierdo != null)                         // Si existe hermano izquierdo...
            {
                hermano_izquierdo.Claves.AddRange(hoja.Claves);    // Une las claves.
                hermano_izquierdo.Siguiente = hoja.Siguiente;      // Repara el enlace de hojas.
                padre.Hijos.RemoveAt(posicion);                    // Elimina la hoja fusionada.
                padre.Claves.RemoveAt(posicion - 1);               // Elimina la guía correspondiente.
                this._reparar_interno(padre);                      // Revisa el nodo padre.
            }
            else if (hermano_derecho != null)                      // Si solo existe el derecho...
            {
                hoja.Claves.AddRange(hermano_derecho.Claves);      // Une las claves.
                hoja.Siguiente = hermano_derecho.Siguiente;        // Repara el enlace.
                padre.Hijos.RemoveAt(posicion + 1);                // Elimina el hermano derecho.
                padre.Claves.RemoveAt(posicion);                   // Elimina la clave guía.
                this._reparar_interno(padre);                      // Revisa el nodo padre.
            }
        }

        // REPARAR NODOS INTERNOS
        private void _reparar_interno(NodoBPlus nodo)
        {
            if (nodo == this.raiz)                                 // Tratamiento especial de la raíz.
            {
                if (nodo.Claves.Count == 0)                        // Si quedó sin claves...
                {
                    this.raiz = nodo.Hijos[0];                     // Su hijo pasa a ser la raíz.
                    this.raiz.Padre = null;                        // La nueva raíz no tiene padre.
                }
                return;
            }

            if (nodo.Hijos.Count >= this.min_hijos_interno)        // Si conserva suficientes hijos...
            {
                return;                                            // No necesita reparación.
            }

            NodoBPlus padre = nodo.Padre;                          // Obtiene el padre.
            int posicion = padre.Hijos.IndexOf(nodo);              // Localiza el nodo.

            NodoBPlus izquierdo = posicion > 0 ? padre.Hijos[posicion - 1] : null;
            NodoBPlus derecho = posicion + 1 < padre.Hijos.Count ? padre.Hijos[posicion + 1] : null;

            if (izquierdo != null && izquierdo.Hijos.Count > this.min_hijos_interno)
            {
                NodoBPlus hijo_movido = izquierdo.Hijos[izquierdo.Hijos.Count - 1];
                izquierdo.Hijos.RemoveAt(izquierdo.Hijos.Count - 1); // Toma el último hijo izquierdo.
                hijo_movido.Padre = nodo;                          // Actualiza su padre.
                int nueva_guia = izquierdo.Claves[izquierdo.Claves.Count - 1];
                izquierdo.Claves.RemoveAt(izquierdo.Claves.Count - 1); // Toma la guía correspondiente.
                nodo.Hijos.Insert(0, hijo_movido);                 // Inserta el hijo al inicio.
                nodo.Claves.Insert(0, padre.Claves[posicion - 1]); // Baja la guía del padre.
                padre.Claves[posicion - 1] = nueva_guia;           // Sube la guía del hermano.
                return;
            }

            if (derecho != null && derecho.Hijos.Count > this.min_hijos_interno)
            {
                NodoBPlus hijo_movido = derecho.Hijos[0];
                derecho.Hijos.RemoveAt(0);                         // Toma el primer hijo derecho.
                hijo_movido.Padre = nodo;                          // Actualiza su padre.
                nodo.Hijos.Add(hijo_movido);                       // Agrega el hijo al final.
                nodo.Claves.Add(padre.Claves[posicion]);           // Baja la guía del padre.
                padre.Claves[posicion] = derecho.Claves[0];
                derecho.Claves.RemoveAt(0);                        // Sube una guía nueva.
                return;
            }

            if (izquierdo != null)                                 // Fusiona con el hermano izquierdo.
            {
                izquierdo.Claves.Add(padre.Claves[posicion - 1]);
                padre.Claves.RemoveAt(posicion - 1);
                izquierdo.Claves.AddRange(nodo.Claves);            // Une las claves internas.
                foreach (NodoBPlus hijo in nodo.Hijos)             // Recorre los hijos del nodo.
                {
                    hijo.Padre = izquierdo;                        // Actualiza sus padres.
                }
                izquierdo.Hijos.AddRange(nodo.Hijos);              // Une todos los hijos.
                padre.Hijos.RemoveAt(posicion);                    // Elimina el nodo fusionado.
                this._reparar_interno(padre);                      // Revisa el padre.
            }
            else if (derecho != null)                              // Fusiona con el hermano derecho.
            {
                nodo.Claves.Add(padre.Claves[posicion]);
                padre.Claves.RemoveAt(posicion);
                nodo.Claves.AddRange(derecho.Claves);              // Une las claves.
                foreach (NodoBPlus hijo in derecho.Hijos)          // Recorre los hijos trasladados.
                {
                    hijo.Padre = nodo;                             // Actualiza sus padres.
                }
                nodo.Hijos.AddRange(derecho.Hijos);                // Une los hijos.
                padre.Hijos.RemoveAt(posicion + 1);                // Elimina el hermano derecho.
                this._reparar_interno(padre);                      // Revisa el padre.
            }
        }

        // ACTUALIZAR CLAVES GUÍA
        private int _minimo_subarbol(NodoBPlus nodo)
        {
            while (!nodo.Hoja)                                     // Baja hasta la hoja izquierda.
            {
                nodo = nodo.Hijos[0];
            }
            return nodo.Claves[0];                                 // Devuelve la primera clave.
        }

        private void _recalcular_guias(NodoBPlus nodo)
        {
            if (nodo.Hoja)                                         // Las hojas no tienen guías.
            {
                return;
            }
            foreach (NodoBPlus hijo in nodo.Hijos)                 // Recorre cada hijo.
            {
                this._recalcular_guias(hijo);                      // Actualiza niveles inferiores.
            }
            
            nodo.Claves.Clear();
            for (int i = 1; i < nodo.Hijos.Count; i++)
            {
                nodo.Claves.Add(this._minimo_subarbol(nodo.Hijos[i])); // Inicio de cada hijo derecho.
            }
        }

        // BÚSQUEDA POR RANGO
        public List<int> BuscarRango(int inicio, int fin)
        {
            // MUY ÚTIL PARA CONSULTAS TIPO BASE DE DATOS ("TRÁEME LOS REGISTROS DEL X AL Y")
            if (inicio > fin)                                      // Comprueba que el rango sea válido.
            {
                int temp = inicio;
                inicio = fin;
                fin = temp;                                        // Intercambia los límites.
            }

            NodoBPlus hoja = this._buscar_hoja(inicio);            // Localiza la primera hoja.
            List<int> resultado = new List<int>();                 // Guarda las claves encontradas.

            while (hoja != null)                                   // Recorre hojas enlazadas a través del puntero 'siguiente'.
            {
                foreach (int clave in hoja.Claves)                 // Recorre sus claves.
                {
                    if (inicio <= clave && clave <= fin)           // Si pertenece al rango...
                    {
                        resultado.Add(clave);                      // La guarda.
                    }
                    else if (clave > fin)                          // Si supera el límite...
                    {
                        return resultado;                          // Finaliza la búsqueda.
                    }
                }
                hoja = hoja.Siguiente;                             // Avanza a la siguiente hoja.
            }
            return resultado;                                      // Devuelve las coincidencias.
        }

        // RECORRER TODAS LAS HOJAS
        public List<int> Recorrer()
        {
            // APROVECHA EL PUNTERO 'SIGUIENTE' PARA LISTAR TODOS LOS ELEMENTOS EN ORDEN O(N)
            NodoBPlus nodo = this.raiz;                            // Comienza en la raíz.
            while (!nodo.Hoja)                                     // Baja a la hoja más izquierda.
            {
                nodo = nodo.Hijos[0];
            }
            List<int> resultado = new List<int>();                 // Guarda todas las claves.
            while (nodo != null)                                   // Recorre las hojas enlazadas.
            {
                resultado.AddRange(nodo.Claves);                   // Agrega sus claves.
                nodo = nodo.Siguiente;                             // Avanza a la siguiente hoja.
            }
            return resultado;                                      // Devuelve las claves ordenadas.
        }

        // MOSTRAR LA ESTRUCTURA
        public void Mostrar()
        {
            this._mostrar(this.raiz, 0);                           // Comienza desde la raíz.
        }

        private void _mostrar(NodoBPlus nodo, int nivel)
        {
            string sangria = new string(' ', nivel * 4);           // Representa la profundidad.
            string tipo = nodo.Hoja ? "Hoja" : "Interno";          // Identifica el tipo de nodo.
            Console.WriteLine($"{sangria}{tipo}: [{string.Join(", ", nodo.Claves)}]"); // Muestra el nodo.
            if (!nodo.Hoja)                                        // Si tiene hijos...
            {
                foreach (NodoBPlus hijo in nodo.Hijos)             // Recorre cada uno.
                {
                    this._mostrar(hijo, nivel + 1);                // Los muestra recursivamente.
                }
            }
        }
    }
}