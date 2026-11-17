using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class DepartamentoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public sealed class DepartamentoRequest
        {
            [Required(ErrorMessage = "Nome é obrigatório")]
            [MaxLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
            public string Nome { get; set; }

            [MaxLength(500, ErrorMessage = "Descrição não pode exceder 500 caracteres")]
            public string Descricao { get; set; }
        }

        public sealed class DepartamentoResponse
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public DateTime DataCriacao { get; set; }
        }
    }
}
