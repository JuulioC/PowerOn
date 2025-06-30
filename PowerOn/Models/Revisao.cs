using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Revisao")]
    public class Revisao
    {
        [Key]
        public int Id { get; set; }

        public string? Nome { get; set; }
        public int? IdMarca { get; set; }
        public int? IdModelo { get; set; }
        public string? Km { get; set; }
        public string? Motor { get; set; }
        public int? Ativo { get; set; }
    }
}