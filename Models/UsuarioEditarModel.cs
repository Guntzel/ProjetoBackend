using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class UsuarioEdicaoViewModel
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    // Senha e imagem NÃO são obrigatórios na edição
    [MinLength(6)]
    public string Senha { get; set; }

    public IFormFile Arquivo { get; set; }
}