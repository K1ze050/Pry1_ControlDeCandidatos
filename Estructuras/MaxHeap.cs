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


        }
}