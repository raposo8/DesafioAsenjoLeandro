using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDesafio.Data;
using ApiDesafio.Services;
using ApiDesafio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ApiDesafio.Models;

namespace ApiDesafio.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("account/login")]
        public IActionResult Login(
    [FromBody] UserLoginViewModel model,
    [FromServices] AppDbContext context,
    [FromServices] TokenService tokenService)
        {
            var user = context.Usuarios.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            if (Settings.GenerateHash(model.Password) != user.Senha)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            try
            {
                var token = tokenService.CreateToken(user);
                return Ok(new { token = token });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [HttpPost("account/signup")]
        public IActionResult Signup(
            [FromBody] UserSignupViewModel model,
            [FromServices] AppDbContext context)
        {
            var user = context.Usuarios.FirstOrDefault(x => x.Email == model.Email);

            if (user != null)
                return StatusCode(401, new { message = "E-mail já cadastrado" });

            try
            {
                var userNew = new User
                {
                    Nome = model.Name,
                    Email = model.Email,
                    Senha = Settings.GenerateHash(model.Password),
                    Role = "Admin"
                };

                context.Usuarios.Add(userNew);
                context.SaveChanges();

                return Ok(new { userId = userNew.Id });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("account/user")]
        public IActionResult Get(
            [FromServices] AppDbContext context)
        {
            try
            {
                var users = context.Usuarios.ToList().Select(x => new UserReturnViewModel
                {
                    Id = x.Id,
                    Name = x.Nome,
                    Email = x.Email,
                    Role = x.Role,
                });

                return Ok(users);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("account/user/{id}/role")]
        public IActionResult UpdateUserRole(
         int id,
        [FromBody] UserReturnViewModel model,
         [FromServices] AppDbContext context)
        {
            var user = context.Usuarios.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                user.Role = model.Role; // Atualiza a role do usuário
                context.Usuarios.Update(user);
                context.SaveChanges();

                return Ok(new { message = "Role do usuário atualizada com sucesso" });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }
    }
}