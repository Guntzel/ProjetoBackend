using ProjetoBackend.Data;
using ProjetoBackend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoBackend.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsuarioController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Página inicial que lista os Usuarios
        public IActionResult Index()
        {
            var Usuarios = _context.Usuarios.ToList(); // Busca todos os Usuarios do banco
            return View(Usuarios); // Passa os Usuarios para a view
        }

        // Método POST para adicionar um novo Usuario
        [HttpPost]
        
        public IActionResult Adicionar(Usuario Usuario, IFormFile Arquivo)
        {
            // Verifica se o nome já existe no banco de dados
            if (_context.Usuarios.Any(d => d.Nome == Usuario.Nome))
            {
                ModelState.AddModelError("Nome", "Já existe um Usuario com este nome.");
            }

            // Verifica se o arquivo foi enviado
            if (Arquivo == null || Arquivo.Length == 0)
            {
                ModelState.AddModelError("Arquivo", "Selecione uma imagem para o usuário.");
            }

            // Verifica se o arquivo possui uma extensão proibida
            if (Arquivo != null)
            {
                var extensao = Path.GetExtension(Arquivo.FileName).ToLower();
                var extensoesProibidas = new[] { ".jpg", ".jpeg", ".png" };

                if (!extensoesProibidas.Contains(extensao))
                {
                    ModelState.AddModelError("Arquivo", "O tipo de arquivo enviado não é permitido.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(Usuario);
            }

            // Hash da senha
            using (var sha256 = SHA256.Create())
            {
                var senhaBytes = Encoding.UTF8.GetBytes(Usuario.Senha);
                var hashBytes = sha256.ComputeHash(senhaBytes);
                Usuario.Senha = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            // Salva o arquivo de imagem
            var caminhoPasta = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens");
            if (!Directory.Exists(caminhoPasta))
            {
                Directory.CreateDirectory(caminhoPasta);
            }

            var nomeArquivo = Path.GetFileName(Arquivo.FileName);
            var caminhoArquivo = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                Arquivo.CopyTo(stream);
            }

            Usuario.ImagePath = $"/Imagens/{nomeArquivo}";

            _context.Usuarios.Add(Usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                return NotFound();

            var model = new UsuarioEdicaoViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
                // Não preencha Senha nem Arquivo aqui
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Editar(UsuarioEdicaoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Id == model.Id);
            if (usuarioExistente == null)
                return NotFound();

            usuarioExistente.Nome = model.Nome;
            usuarioExistente.Email = model.Email;

            // Só atualiza a senha se foi preenchida
            if (!string.IsNullOrWhiteSpace(model.Senha))
            {
                using (var sha256 = SHA256.Create())
                {
                    var senhaBytes = Encoding.UTF8.GetBytes(model.Senha);
                    var hashBytes = sha256.ComputeHash(senhaBytes);
                    usuarioExistente.Senha = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }

            // Só atualiza a imagem se foi enviada
            if (model.Arquivo != null && model.Arquivo.Length > 0)
            {
                var extensao = Path.GetExtension(model.Arquivo.FileName).ToLower();
                var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png" };

                if (!extensoesPermitidas.Contains(extensao))
                {
                    ModelState.AddModelError("Arquivo", "Só são permitidas imagens jpg, jpeg ou png.");
                    return View(model);
                }

                // Exclui a imagem anterior, se existir
                if (!string.IsNullOrEmpty(usuarioExistente.ImagePath))
                {
                    var caminhoAntigo = Path.Combine(_webHostEnvironment.WebRootPath, usuarioExistente.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(caminhoAntigo))
                    {
                        System.IO.File.Delete(caminhoAntigo);
                    }
                }

                // Salva a nova imagem
                var caminhoPasta = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens");
                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);

                var nomeArquivo = Path.GetFileName(model.Arquivo.FileName);
                var caminhoArquivo = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    model.Arquivo.CopyTo(stream);
                }

                usuarioExistente.ImagePath = $"/Imagens/{nomeArquivo}";
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // Método POST para excluir um Usuario
        [HttpPost]
            public IActionResult Excluir(int id)
            {
                var Usuario = _context.Usuarios.FirstOrDefault(d => d.Id == id);
                if (Usuario == null)
                {
                    return NotFound();
                }

                // Exclui o arquivo do caminho físico
                var caminhoArquivo = Path.Combine(_webHostEnvironment.WebRootPath, Usuario.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(caminhoArquivo))
                {
                    System.IO.File.Delete(caminhoArquivo);
                }
                // Remove o Usuario do banco de dados
                _context.Usuarios.Remove(Usuario);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        [HttpGet]
        public IActionResult Adicionar()
        {
            return View();
        }
  }
}