using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDesafio.Data;
using ApiDesafio.Models;
using ApiDesafio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDesafio.Controllers
{
    [ApiController]
    [Route("bar")]
    public class BarController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateBar(
                   [FromBody] BarCreateViewModel model,
                   [FromServices] AppDbContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var bar = new Bar
                {
                    Nome = model.Nome,
                    Endereco = model.Endereco,
                    Descricao = model.Descricao,
                    NotaMedia = model.NotaMedia
                };

                context.Bares.Add(bar);
                context.SaveChanges();

                return Ok(new { id = bar.Id });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }


        [HttpGet]
        public IActionResult GetAllBars(
            [FromServices] AppDbContext context)
        {
            try
            {
                var bars = context.Bares.ToList().Select(x => new BarReturnViewModel
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Endereco = x.Endereco,
                    Descricao = x.Descricao,
                    NotaMedia = x.NotaMedia
                });

                return Ok(bars);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetBar(
            int id,
            [FromServices] AppDbContext context)
        {
            try
            {
                var bar = context.Bares.FirstOrDefault(x => x.Id == id);

                if (bar == null)
                    return NotFound(new { message = "Bar não encontrado." });

                var barViewModel = new BarReturnViewModel
                {
                    Id = bar.Id,
                    Nome = bar.Nome,
                    Endereco = bar.Endereco,
                    Descricao = bar.Descricao,
                    NotaMedia = bar.NotaMedia
                };

                return Ok(barViewModel);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateBar(
            int id,
            [FromBody] BarCreateViewModel model,
            [FromServices] AppDbContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var bar = context.Bares.FirstOrDefault(x => x.Id == id);

                if (bar == null)
                    return NotFound(new { message = "Bar não encontrado." });

                bar.Nome = model.Nome;
                bar.Endereco = model.Endereco;
                bar.Descricao = model.Descricao;
                bar.NotaMedia = model.NotaMedia;

                context.Bares.Update(bar);
                context.SaveChanges();

                return NoContent(); // 204 No Content
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteBar(
            int id,
            [FromServices] AppDbContext context)
        {
            try
            {
                var bar = context.Bares.FirstOrDefault(x => x.Id == id);

                if (bar == null)
                    return NotFound(new { message = "Bar não encontrado." });

                context.Bares.Remove(bar);
                context.SaveChanges();

                return NoContent(); // 204 No Content
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }
    }
}