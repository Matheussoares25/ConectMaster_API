using System.ComponentModel.DataAnnotations;
namespace ConectMaster.Models
{
    public class Permissao
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
