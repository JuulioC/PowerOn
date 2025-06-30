using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("logApp")]
    public class LogApp
    {
        [Key]
        public int Id { get; set; }
        public string? Caminho { get; set; }
        public string? Mensagem { get; set; }
        public string? Usuario { get; set; }
        public DateTime? DataErro { get; set; }
    }
}