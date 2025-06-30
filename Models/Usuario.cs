using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoBackend.Models
{


    //     public class Documento
    //     {
    //         public int Id { get; set; }

    //         [Required(ErrorMessage = "O título é obrigatório.")]
    //         [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
    //         public string Titulo { get; set; }
    //         [Required(ErrorMessage = "A descrição é obrigatória.")]
    //         [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres.")]
    //         public string Descricao { get; set; }

    //         [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
    //         public string NomeArquivo { get; set; }

    //         public string CaminhoArquivo { get; set; }

    //         public DateTime DataCriacao { get; set; } = DateTime.Now;
    //     }
    // }

    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve ser válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A senha deve ter no mínimo 6 e no máximo 100 caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        public string ImagePath { get; set; } // Caminho da imagem do usuário
    }
}