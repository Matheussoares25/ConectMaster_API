using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models.Financeiro
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
