using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Marca")]
    public class Marca
    {
        [Key]
        public int Id { get; set; }

        public string? Cod { get; set; }
        public string? Nome { get; set; }
    }
}