using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Familia")]
    public class Familia
    {
        [Key]
        public int Id { get; set; }

        public string? Cod { get; set; }
        public string? Nome { get; set; }
        public int? IdMarca { get; set; }
    }
}