using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Agenda")]
    public class Agenda
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Data_Agendamento { get; set; }
        public string? Cliente { get; set; }
        public string? Placa { get; set; }
        public string? IdConsultor { get; set; }
        public string? IdModelo { get; set; }
        public string? Observacao { get; set; } // Corrigido
        public string? OS { get; set; }
        public string? Situacao { get; set; }
    }
}