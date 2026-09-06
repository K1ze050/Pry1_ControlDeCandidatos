using System;

namespace PRY1.Estructuras
{
    public class NodoHeap
    {
        public double Prioridad { get; set; } 
        public object Elemento { get; set; }  
        
        public NodoHeap(double prioridad, object elemento)
        {
            Prioridad = prioridad;
            Elemento = elemento;
        }
    }

    public class MinHeap
    {
        private NodoHeap[] heap; 
        private int cantidad;    
        private int capacidad;   

        public MinHeap(int capacidadInicial = 10)
        {
            capacidad = capacidadInicial;
            heap = new NodoHeap[capacidad];
            cantidad = 0; 
        }

        // INSERTAR Y HACER FLOTAR (HEAPIFY UP)
        public void Insertar(double prioridad, object elemento)
        {
            if (cantidad == capacidad)
            {
                AmpliarArreglo();
            }

            heap[cantidad] = new NodoHeap(prioridad, elemento);
            HeapifyUp(cantidad); // Sube el nodo a su posición correcta
            cantidad++;
        }

        private void HeapifyUp(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2; // Fórmula matemática para el padre

                // Lógica MIN Heap: Subimos el nodo si es MENOR que su padre
                if (heap[indice].Prioridad < heap[padre].Prioridad)
                {
                    NodoHeap temporal = heap[indice];
                    heap[indice] = heap[padre];
                    heap[padre] = temporal;
                    indice = padre;
                }
                else
                {
                    break;
                }
            }
        }

        // EXTRAER Y HUNDIR (HEAPIFY DOWN)
        public NodoHeap Extraer()
        {
            if (cantidad == 0) return null; // Retorna nulo si está vacío
            if (cantidad == 1)
            {
                cantidad--;
                return heap[0];
            }

            // Se elimina la raíz (la de mayor prioridad/menor valor) y se sube el último elemento
            NodoHeap raiz = heap[0];
            heap[0] = heap[cantidad - 1]; 
            heap[cantidad - 1] = null; // Limpiamos la referencia
            cantidad--;
            
            // Reacomodamos el nuevo nodo raíz hundiéndolo
            HeapifyDown(0);
            
            return raiz;
        }

        private void HeapifyDown(int indice)
        {
            while (true)
            {
                int menor = indice;
                int izquierdo = 2 * indice + 1; // Fórmula para hijo izquierdo
                int derecho = 2 * indice + 2;   // Fórmula para hijo derecho

                // Buscamos si el hijo izquierdo es menor que el padre
                if (izquierdo < cantidad && heap[izquierdo].Prioridad < heap[menor].Prioridad)
                {
                    menor = izquierdo;
                }

                // Buscamos si el hijo derecho es menor que el actual "menor"
                if (derecho < cantidad && heap[derecho].Prioridad < heap[menor].Prioridad)
                {
                    menor = derecho;
                }

                // Si el índice no cambió, el nodo ya está en su posición correcta
                if (menor == indice) break;

                // Intercambio
                NodoHeap temporal = heap[indice];
                heap[indice] = heap[menor];
                heap[menor] = temporal;
                
                indice = menor;
            }
        }

        // FUNCIONES DE CONSULTA
        public NodoHeap Peek()
        {
            if (cantidad == 0) return null;
            return heap[0]; // Siempre es la raíz
        }

        public bool EstaVacia()
        {
            return cantidad == 0; // Verifica si hay elementos
        }

        public void Mostrar()
        {
            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine($"Prioridad: Q{heap[i].Prioridad} | Candidato: {heap[i].Elemento}");
            }
        }

        // FUNCIÓN AUXILIAR DE ARREGLO DINÁMICO
        private void AmpliarArreglo()
        {
            capacidad *= 2; 
            NodoHeap[] nuevoHeap = new NodoHeap[capacidad];
            for (int i = 0; i < cantidad; i++)
            {
                nuevoHeap[i] = heap[i];
            }
            heap = nuevoHeap; 
        }
    }
}