using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("PacoteProduto")]
    public class PacoteProduto
    {
        // Chaves estrangeiras que formam a chave primária composta
        public int IdPacote { get; set; }
        public int IdProduto { get; set; }

        public int Quantidade { get; set; }

        // Propriedades de navegação para os objetos completos
        [ForeignKey("IdPacote")]
        public virtual Pacote Pacote { get; set; } = null!;

        [ForeignKey("IdProduto")]
        public virtual Produto Produto { get; set; } = null!;
    }
}