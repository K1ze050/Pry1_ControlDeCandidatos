using System;
using System.IO;
using PRY1.Estructuras;

namespace PRY1
{
    class Program
    {
        static string archivo = "candidatos.csv";
        
        static ArbolBPlus arbolBPlus = new ArbolBPlus(4);
        static MinHeap minHeap = new MinHeap();
        static MaxHeap maxHeap = new MaxHeap();
        
        static MiLista<Candidato> baseDatos = new MiLista<Candidato>();

        static void Main(string[] args)
        {
            CargarDatos();
            int opcion = -1;

            while (opcion != 0)
            {
                // 1. Pintamos el fondo de negro y limpiamos la pantalla
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                // 2. Dibujamos el encabezado en color Cian
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\t+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*");
                Console.WriteLine("\t            SISTEMA DE CONTROL DE CANDIDATOS                ");
                Console.WriteLine("\t+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*+*");
                
                // 3. Opciones del menú en Blanco
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t║                                                            ║");
                Console.WriteLine("\t║  [ 1 ] Registrar nuevo candidato                           ║");
                Console.WriteLine("\t║  [ 2 ] Buscar candidato por ID                             ║");
                Console.WriteLine("\t║  [ 3 ] Mostrar candidato con menor pretensión salarial     ║");
                Console.WriteLine("\t║  [ 4 ] Mostrar todos los candidatos (Usando Árbol B+)      ║");
                Console.WriteLine("\t║  [ 5 ] Mostrar candidato con mayor pretensión salarial     ║");
                Console.WriteLine("\t║  [ 6 ] Eliminar un candidato por ID                        ║");
                Console.WriteLine("\t║  [ 0 ] Salir                                               ║");
                Console.WriteLine("\t║                                                            ║");
                
                // 4. Borde inferior en Cian
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\t╚════════════════════════════════════════════════════════════╝");
                
                // 5. Input del usuario en Amarillo
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n\t   Selecciona una opción: ");
                Console.ResetColor();
                
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine();
                    switch (opcion)
                    {
                        case 1:
                            RegistrarCandidato();
                            break;
                        case 2:
                            BuscarCandidato();
                            break;
                        case 3:
                            MostrarMenorPretension();
                            break;
                        case 4:
                            MostrarTodos();
                            break;
                        case 5:
                            MostrarMayorPretension();
                            break;
                        case 6:
                            EliminarCandidato();
                            break;
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\tPrograma finalizado.");
                            Console.ResetColor();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\tOpción no válida.");
                            Console.ResetColor();
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\tPor favor, ingrese un número válido.");
                    Console.ResetColor();
                }
                
                if (opcion != 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\tPresiona enter");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }

        static void CargarDatos()
        {
            if (File.Exists(archivo))
            {
                string[] lineas = File.ReadAllLines(archivo);
                foreach (string linea in lineas)
                {
                    string[] datos = linea.Split(',');
                    if (datos.Length == 5)
                    {
                        int id = int.Parse(datos[0]);
                        string nombre = datos[1];
                        string origen = datos[2];
                        string puesto = datos[3];
                        double salario = double.Parse(datos[4]);

                        Candidato nuevo = new Candidato(id, nombre, origen, puesto, salario);
                        
                        baseDatos.Add(nuevo);
                        arbolBPlus.Insertar(id);
                        minHeap.Insertar(salario, nuevo);
                        maxHeap.Insertar(salario, nuevo);
                    }
                }
            }
        }

        static void RegistrarCandidato()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tIngresa su ID: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            int id = int.Parse(Console.ReadLine());
            Console.ResetColor();

            if (arbolBPlus.Buscar(id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tID ya existente, no se puede repetir.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tNombre: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string nombre = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tDepartamento: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string origen = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tPuesto de aplicación [Gerente|Secretario|Contador|Ventas]: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string puesto = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tIngrese la pretensión salarial: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            double salario = double.Parse(Console.ReadLine());
            Console.ResetColor();

            Candidato nuevo = new Candidato(id, nombre, origen, puesto, salario);

            baseDatos.Add(nuevo);
            arbolBPlus.Insertar(id);
            minHeap.Insertar(salario, nuevo);
            maxHeap.Insertar(salario, nuevo);

            using (StreamWriter sw = new StreamWriter(archivo, true))
            {
                sw.WriteLine($"{id},{nombre},{origen},{puesto},{salario}");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\tCandidato registrado exitosamente.");
            Console.ResetColor();
        }

        static void BuscarCandidato()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tColoca su ID y se buscara en sistema: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            int id = int.Parse(Console.ReadLine());
            Console.ResetColor();

            if (arbolBPlus.Buscar(id))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\tCandidato ubicado:");
                Console.ResetColor();
                for (int i = 0; i < baseDatos.Count; i++)
                {
                    if (baseDatos[i].Id == id)
                    {
                        Console.WriteLine("\t" + baseDatos[i].ToString());
                        break;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\tCandidato inexistente.");
                Console.ResetColor();
            }
        }

        static void MostrarMenorPretension()
        {
            if (minHeap.EstaVacia())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tNo cuentas con candidato registrado.");
                Console.ResetColor();
            }
            else
            {
                NodoHeap nodo = minHeap.Peek();
                Candidato candidato = (Candidato)nodo.Elemento;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tEste es el candidato con menos salario:");
                Console.ResetColor();
                Console.WriteLine("\t" + candidato.ToString());
            }
        }

        static void MostrarTodos()
        {
            // Usamos la función Recorrer del Árbol B+ para justificar su uso
            var idsOrdenados = arbolBPlus.Recorrer();

            if (idsOrdenados.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tNo hay candidatos registrados en el sistema.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\tListado general de candidatos (Ordenados por ID en Árbol B+):");
            Console.ResetColor();

            for (int i = 0; i < idsOrdenados.Count; i++)
            {
                int idActual = idsOrdenados[i];
                // Buscamos el candidato correspondiente a este ID
                for (int j = 0; j < baseDatos.Count; j++)
                {
                    if (baseDatos[j].Id == idActual)
                    {
                        Console.WriteLine("\t" + baseDatos[j].ToString());
                        break;
                    }
                }
            }
        }

        static void MostrarMayorPretension()
        {
            if (maxHeap.EstaVacia())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tNo hay candidatos.");
                Console.ResetColor();
            }
            else
            {
                NodoHeap nodo = maxHeap.Peek();
                Candidato candidato = (Candidato)nodo.Elemento;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tCandidato con mayor pretensión salarial:");
                Console.ResetColor();
                Console.WriteLine("\t" + candidato.ToString());
            }
        }

        static void EliminarCandidato()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tIngrese el ID del candidato a eliminar: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            int id = int.Parse(Console.ReadLine());
            Console.ResetColor();

            // Intentamos eliminar del Árbol B+
            if (arbolBPlus.Eliminar(id))
            {
                // Si se eliminó del árbol, lo quitamos de la base de datos auxiliar
                for (int i = 0; i < baseDatos.Count; i++)
                {
                    if (baseDatos[i].Id == id)
                    {
                        baseDatos.RemoveAt(i);
                        break;
                    }
                }

                // Actualizamos el archivo CSV para que la eliminación sea permanente
                using (StreamWriter sw = new StreamWriter(archivo, false)) // false = sobrescribir todo
                {
                    for (int i = 0; i < baseDatos.Count; i++)
                    {
                        sw.WriteLine($"{baseDatos[i].Id},{baseDatos[i].Nombre},{baseDatos[i].LugarOrigen},{baseDatos[i].Puesto},{baseDatos[i].PretensionSalarial}");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\tCandidato eliminado exitosamente del Árbol B+ y del archivo.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\tError: El candidato con ese ID no existe.");
                Console.ResetColor();
            }
        }
    }
}