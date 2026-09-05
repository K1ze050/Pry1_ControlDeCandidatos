using System;

namespace PRY1.Estructuras
{
    public class MaxHeap
    {
        // Lista donde se almacenan los elementos estructurados (ahora con un arreglo estático en C# para cumplir las reglas)
        private NodoHeap[] heap;
        private int cantidad;
        private int capacidad;

        public MaxHeap(int capacidadInicial = 10)
        {
            capacidad = capacidadInicial;
            heap = new NodoHeap[capacidad];
            cantidad = 0;
        }

        // INSERTAR
        public void Insertar(double prioridad, object elemento)
        {
            if (cantidad == capacidad)
            {
                AmpliarArreglo();
            }

            heap[cantidad] = new NodoHeap(prioridad, elemento);
            int indice = cantidad;
            
            // AL INSERTAR UN ELEMENTO NUEVO, ESTE DEBE "FLOTAR" (SUBIR) HASTA SU POSICIÓN CORRECTA SEGÚN SU PRIORIDAD.
            HeapifyUp(indice);
            
            cantidad++;
        }

        // HEAPIFY UP (HACER FLOTAR EL NODO)
        private void HeapifyUp(int indice)
        {
            while (indice > 0)
            {
                // FÓRMULA MATEMÁTICA PARA LOCALIZAR AL NODO PADRE[cite: 2]
                int padre = (indice - 1) / 2; 

                // ESTA FUNCIÓN SE ASEGURA DE QUE SE CUMPLA LA PROPIEDAD DEL MAX-HEAP: EL PADRE DEBE SER MAYOR QUE SUS HIJOS.[cite: 4]
                // Lógica MAX HEAP: Subimos el nodo si es MAYOR que su padre.
                if (heap[indice].Prioridad > heap[padre].Prioridad)
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

        // EXTRAER EL MÁS PRIORITARIO[cite: 2]
        public NodoHeap Extraer()
        {
            // SE ELIMINA LA RAÍZ (LA DE MAYOR PRIORIDAD), SE SUBE EL ÚLTIMO ELEMENTO A LA RAÍZ, Y LUEGO SE HUNDE PARA REACOMODARSE.[cite: 2]
            if (cantidad == 0) return null;
            if (cantidad == 1)
            {
                cantidad--;
                return heap[0];
            }

            NodoHeap prioridad_maxima = heap[0];
            heap[0] = heap[cantidad - 1];
            heap[cantidad - 1] = null; // Liberamos memoria
            cantidad--;
            
            HeapifyDown(0);
            
            return prioridad_maxima;
        }

        // HEAPIFY DOWN (HUNDIR EL NODO)[cite: 2]
        private void HeapifyDown(int indice)
        {
            while (true)
            {
                int mayor = indice;
                // FÓRMULA MATEMÁTICA PARA UBICAR EL HIJO IZQUIERDO Y DERECHO EN EL ARREGLO[cite: 4]
                int izquierdo = 2 * indice + 1; 
                int derecho = 2 * indice + 2;   

                // Verificamos si el hijo izquierdo es mayor que el padre actual
                if (izquierdo < cantidad && heap[izquierdo].Prioridad > heap[mayor].Prioridad)
                {
                    mayor = izquierdo;
                }

                // Verificamos si el hijo derecho es mayor que el "mayor" encontrado hasta ahora
                if (derecho < cantidad && heap[derecho].Prioridad > heap[mayor].Prioridad)
                {
                    mayor = derecho;
                }

                if (mayor == indice)
                {
                    break;
                }

                NodoHeap temporal = heap[indice];
                heap[indice] = heap[mayor];
                heap[mayor] = temporal;
                
                indice = mayor;
            }
        }

        // CONSULTAR EL MÁS PRIORITARIO[cite: 2]
        public NodoHeap Peek()
        {
            if (cantidad == 0) return null;
            return heap[0]; // SIEMPRE ES LA RAÍZ (POSICIÓN 0 DEL ARREGLO)[cite: 2]
        }

        public bool EstaVacia()
        {
            return cantidad == 0;
        }

        // Función auxiliar obligatoria por restricciones del proyecto
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