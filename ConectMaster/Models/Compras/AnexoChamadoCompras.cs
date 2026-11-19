using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectMaster.Models.Compras
{
    public class AnexoChamadoCompras
    {
        [Key]
        public int Id { get; set; }

        public int ChamadoComprasId { get; set; }

        [ForeignKey(nameof(ChamadoComprasId))]
        public Chamados_Compras? Chamado { get; set; }

        [MaxLength(255)]
        public string NomeOriginal { get; set; } = string.Empty;

        public byte[] Conteudo { get; set; } = Array.Empty<byte>();

        public DateTime DataUpload { get; set; }
    }
}
