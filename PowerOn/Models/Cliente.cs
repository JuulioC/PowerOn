using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? TipoPessoa { get; set; }
        public string? Sexo { get; set; }
        public string? CPFCNPJ { get; set; }
        public string? RG { get; set; }
        public string? Email { get; set; }
        public int? PermiteSMS { get; set; }
        public int? PermiteEMAIL { get; set; }
        public int? PermiteWHATSAPP { get; set; }
    }
}
