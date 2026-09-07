using System;

namespace PRY1
{
    public class Candidato
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string LugarOrigen { get; set; }
        public string Puesto { get; set; }
        public double PretensionSalarial { get; set; }

        public Candidato(int id, string nombre, string lugarOrigen, string puesto, double pretensionSalarial)
        {
            Id = id;
            Nombre = nombre;
            LugarOrigen = lugarOrigen;
            Puesto = puesto;
            PretensionSalarial = pretensionSalarial;
        }

        public override string ToString()
        {
            return $"ID: {Id}|Nombre: {Nombre}|Origen: {LugarOrigen}|Puesto: {Puesto}|Salario: Q{PretensionSalarial}";
        }
    }
}