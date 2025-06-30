using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("ClienteTelefone")]
    public class ClienteTelefone
    {
        [Key]
        public int Id { get; set; }
        public int? Cliente { get; set; }
        public string? Tipo { get; set; }
        public string? DDD { get; set; }
        public string? Telefone { get; set; }
    }
}
