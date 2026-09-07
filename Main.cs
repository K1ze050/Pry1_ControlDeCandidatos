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
                Console.Clear();
                Console.WriteLine("1. Registrar nuevo candidato");
                Console.WriteLine("2. Buscar candidato por ID");
                Console.WriteLine("3. Mostrar candidato con menor pretensión salarial");
                Console.WriteLine("4. Mostrar todos los candidatos ");
                Console.WriteLine("5. Mostrar candidato con mayor pretensión salarial");
                Console.WriteLine("0. Salir");
                Console.Write("Selecciona una opción: ");
                
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
                        case 0:
                            Console.WriteLine("Programa finalizado.");
                            break;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }
                }
                
                if (opcion != 0)
                {
                    Console.WriteLine("\nIngrese un valor valido");
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
            Console.Write("Ingresa su ID: ");
            int id = int.Parse(Console.ReadLine());

            if (arbolBPlus.Buscar(id))
            {
                Console.WriteLine("ID ya existente, no se puede repetir.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Departamento: ");
            string origen = Console.ReadLine();

            Console.Write("Puesto de aplicación [Gerente|Secretario|Contador|Ventas]: ");
            string puesto = Console.ReadLine();

            Console.Write("Ingrese la pretensión salarial: ");
            double salario = double.Parse(Console.ReadLine());

            Candidato nuevo = new Candidato(id, nombre, origen, puesto, salario);

            baseDatos.Add(nuevo);
            arbolBPlus.Insertar(id);
            minHeap.Insertar(salario, nuevo);
            maxHeap.Insertar(salario, nuevo);

            using (StreamWriter sw = new StreamWriter(archivo, true))
            {
                sw.WriteLine($"{id},{nombre},{origen},{puesto},{salario}");
            }

            Console.WriteLine("Candidato registrado");
        }

        static void BuscarCandidato()
        {
            Console.Write("Coloca su ID y se buscara en sistema: ");
            int id = int.Parse(Console.ReadLine());

            if (arbolBPlus.Buscar(id))
            {
                Console.WriteLine("Candidato ubicado:");
                for (int i = 0; i < baseDatos.Count; i++)
                {
                    if (baseDatos[i].Id == id)
                    {
                        Console.WriteLine(baseDatos[i].ToString());
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Candidato inexistente.");
            }
        }

        static void MostrarMenorPretension()
        {
            if (minHeap.EstaVacia())
            {
                Console.WriteLine("No cuentas con candidato registrado.");
            }
            else
            {
                NodoHeap nodo = minHeap.Peek();
                Candidato candidato = (Candidato)nodo.Elemento;
                Console.WriteLine("Este es el candidato con menos salario:");
                Console.WriteLine(candidato.ToString());
            }
        }

        static void MostrarTodos()
        {
            if (baseDatos.Count == 0)
            {
                Console.WriteLine("No hay candidatos registrados.");
                return;
            }

            Console.WriteLine("Listado general de candidatos:");
            for (int i = 0; i < baseDatos.Count; i++)
            {
                Console.WriteLine(baseDatos[i].ToString());
            }
        }

        static void MostrarMayorPretension()
{
        if (maxHeap.EstaVacia())
        {
           Console.WriteLine("No hay candidatos.");
         }
        else
        {
        NodoHeap nodo = maxHeap.Peek();
        Candidato candidato = (Candidato)nodo.Elemento;
        Console.WriteLine("Candidato con mayor pretención salarial");
        Console.WriteLine(candidato.ToString());
    }
}
    }
}

