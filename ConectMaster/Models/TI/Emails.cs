using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectMaster.Models.Financeiro;

namespace ConectMaster.Models.TI
{
    public class Emails
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Responsavel { get; set; }

        public int Departamentoid { get; set; }

        [ForeignKey(nameof(Departamentoid))]
        public Departamento? Departamento { get; set; }
    }
}
