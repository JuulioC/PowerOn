using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerOn.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public int? Perfil { get; set; }
        public int? EmpresaId { get; set; }
        public string? CodigoSistema { get; set; }
        public byte[]? ImgPerfil { get; set; }
        // *** MUDANÇA AQUI: Nova propriedade para armazenar o tipo MIME da imagem ***
        public string? ImgPerfilMimeType { get; set; }
        public int? Ativo { get; set; }
        public DateTime? UltimoAcesso { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa? Empresa { get; set; }
    }
}
