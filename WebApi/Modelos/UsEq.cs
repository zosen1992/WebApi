using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Modelos
{
    public class UsEq
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int? Id { get; set; }
        //public string name { get; set; }
        //public int CentroSanitario { get; set; }
        //public string? NombrePaciente { get; set; }
        public int? Id { get; set; }
        public string? Equipo { get; set; }
        public string? Team { get; set; }

        public DateTime? Fecha { get; set; }

        public int? semana { get; set; }
        //public decimal horas { get; set; }
        //public decimal CapacidadSemanal { get; set; }


    }
}
