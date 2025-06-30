using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }
        public string? Placa { get; set; }
        public string? Chassi { get; set; }
        public int? Cliente { get; set; }
        public string? Marca { get; set; }
        public string? Familia { get; set; }
        public string? Modelo { get; set; }
        public string? Motor { get; set; }
        public string? Km { get; set; }
    }
}