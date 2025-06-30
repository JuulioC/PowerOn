using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Sistemas")]
    public class Sistemas
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
    }
}
