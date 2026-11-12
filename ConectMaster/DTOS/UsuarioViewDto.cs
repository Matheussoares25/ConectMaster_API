using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class UsuarioViewDto
    {
            [Required]
            public int UsuarioId { get; set; }

            [Required]
            public int ViewId { get; set; }
        
    }

    public class UsuarioViewResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }
        public int ViewId { get; set; }
        public string? ViewNome { get; set; }
    }
}
