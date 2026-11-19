using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class AcessoDto
    {
        public bool Verificado { get; set; }

        [MaxLength(200)]
        public string? Observacao { get; set; }

        public bool Liberado { get; set; }

        public DateTime DataEntrada { get; set; }

        [Required]
        [MaxLength(100)]
        public string Despachante { get; set; } = string.Empty;

        public DateTime? DataSaida { get; set; }
        public string? Servico { get; set; }
        public string? Transportadora { get; set; }
        public string? Motorista { get; set; }
        public string? Cnh { get; set; }
        public string? Placa { get; set; }
        public int? Veiculo { get; set; }
        public bool? VeiculoAberto { get; set; }
        public string? NumeroNf { get; set; }
        public string? Quantidade { get; set; }
        public DateTime? Duracao { get; set; }
        public string? TelefoneMotorista { get; set; }
        public string? MensagemCheckin { get; set; }
        public string? MensagemColeta { get; set; }
    }
}
