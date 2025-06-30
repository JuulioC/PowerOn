using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Modelo")]
    public class Modelo
    {
        [Key]
        public int Id { get; set; }

        public string? Cod { get; set; }
        public string? Nome { get; set; }
        public string? AnoFabricacao { get; set; }
        public string? AnoModelo { get; set; }
        public int? IdFamilia { get; set; }
    }
}