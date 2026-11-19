using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models.Adversos.Acesso
{
    public class Veiculos
    {
        [Key]
        public int Id { get; set; }

        public string? Modelo { get; set; }
    }
}
