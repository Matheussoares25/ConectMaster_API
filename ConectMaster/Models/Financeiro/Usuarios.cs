using ConectMaster.Models.Financeiro;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Usuarios
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Senha { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? Ramal { get; set; }

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public int? DepartamentoId { get; set; }
    [ForeignKey(nameof(DepartamentoId))]
    public Departamento? Departamento { get; set; }

    public int? PerfilId { get; set; }
    [ForeignKey(nameof(PerfilId))]
    public Perfil? Perfil { get; set; }
}