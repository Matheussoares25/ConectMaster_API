using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConectMaster.Models
{
    public class Servico 
    {
        public int Id { get; set; }

        // ===== IDENTIFICAÇÃO =====
        [MaxLength(100)]
        public string? Titulo { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(100)]
        public string? Setor { get; set; }
        [MaxLength(100)]
        public string? Categoria { get; set; }        
        public int Prioridade { get; set; }       

        // ===== CLIENTE / CONTRATANTE =====
        [MaxLength(100)]
        public string? ClienteNome { get; set; }
        [MaxLength(50)]
        public string? ClienteDocumento { get; set; } 
        [MaxLength(20)]
        public string? ClienteTelefone { get; set; }
        [MaxLength(100)]
        public string? ClienteContato { get; set; }    

        // ===== CARGA =====
        [MaxLength(1000)]
        public string? DescricaoCarga { get; set; }
        public decimal? PesoBruto { get; set; }       // kg
        public decimal? Volume { get; set; }          // m³
        public int? QtdVolumes { get; set; }
        public decimal? ValorMercadoria { get; set; }
        [MaxLength(100)]
        public string? NaturezaCarga { get; set; }    

        // ===== ORIGEM E DESTINO =====
        [MaxLength(300)]
        public string? EnderecoColeta { get; set; }
        [MaxLength(300)]
        public string? EnderecoEntrega { get; set; }
        public DateTime? DataColeta { get; set; }
        public DateTime? DataEntrega { get; set; }

        // ===== TRANSPORTE =====
        [MaxLength(20)]
        public string? PlacaVeiculo { get; set; }
        [MaxLength(100)]
        public string? TipoVeiculo { get; set; }     
        [MaxLength(100)]
        public string? MotoristaNome { get; set; }
        [MaxLength(20)]
        public string? MotoristaTelefone { get; set; }

        // ===== FISCAL =====
        [MaxLength(50)]
        public string? NumeroNfe { get; set; }
        [MaxLength(50)]
        public string? NumeroCte { get; set; }

        // ===== VALORES =====
        public decimal? ValorFrete { get; set; }
        [MaxLength(100)]
        public string? FormaPagamento { get; set; } 

        // ===== OBSERVAÇÕES =====
        [MaxLength(2000)]
        public string? Descricao { get; set; }

        // ===== CONTROLE =====
        [MaxLength(50)]
        public string? Status { get; set; } = "Aberta";
        public int SolicitanteId { get; set; }

        [ForeignKey("SolicitanteId")]
        public Usuarios? Usuario { get; set; }
        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;


    }
}
