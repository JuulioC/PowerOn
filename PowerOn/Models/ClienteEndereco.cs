using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("ClienteEndereco")]
    public class ClienteEndereco
    {
        [Key]
        public int Id { get; set; }
        public int? Cliente { get; set; }
        public string? Tipo { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? CEP { get; set; }
    }
}