using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? Tipo { get; set; }
        public string? CodSistema { get; set; }
        public float? Valor { get; set; }

        // Propriedade de navegação para a tabela de ligação
        public virtual ICollection<PacoteProduto> PacoteProdutos { get; set; } = new List<PacoteProduto>();
    }
}