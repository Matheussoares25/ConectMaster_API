using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectMaster.Models.Adversos.Acesso
{
    public class Acesso
    {
        [Key]
        public int Id { get; set; }

        public bool? Verificado { get; set; } = false;

        [MaxLength(200)]
        public string? Observacao { get; set; }

        public bool? Liberado { get; set; } = false;

        public DateTime Data_entrada { get; set; } 

        [MaxLength(100)]
        public string? Despachante { get; set; } = string.Empty;

        public DateTime? Data_saida { get; set; }

        public string? Servico { get; set; }

        public string? Transportadora { get; set; }

        public string? Motorista { get; set; }

        public string? Cnh { get; set; }

        public string? Placa { get; set; }
 
        public int? Veiculo { get; set; }

        [ForeignKey(nameof(Veiculo))]
        public Veiculos? Veiculos { get; set; } = null!;

        public bool? Veiculo_aberto { get; set; } = false;

        public string? Numero_nf { get; set; }

        public string? Quantidade { get; set; }

        public DateTime? Duracao { get; set; }

        public string? Telefone_motorista { get; set; }


      

    }
}
