using System;
using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models
{
    public class LogAuditoria
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [MaxLength(100)]
        public string? UsuarioName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Acao { get; set; } = string.Empty; // "Criar", "Editar", "Excluir"

        [Required]
        [MaxLength(100)]
        public string Entidade { get; set; } = string.Empty; // "Usuario", "Perfil", "Chamado"

        public int? EntidadeId { get; set; }

        [MaxLength(2000)]
        public string? Detalhes { get; set; }

        public DateTime DataHora { get; set; } = DateTime.UtcNow;
    }
}
