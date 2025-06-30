using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Pacote")]
    public class Pacote
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }

        // Propriedade de navegação para a tabela de ligação
        public virtual ICollection<PacoteProduto> PacoteProdutos { get; set; } = new List<PacoteProduto>();
    }
}
