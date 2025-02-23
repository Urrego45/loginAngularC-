﻿using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Custom;
using api.Models;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly DbpruebaContext _dbPruebaContext;
        private readonly Utilidades _utilidades;
        public AccesoController(DbpruebaContext dbPruebaContext, Utilidades utilidades)
        {
            _dbPruebaContext = dbPruebaContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {

            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Clave = _utilidades.encriptarSHA256(objeto.Clave)
            };

            await _dbPruebaContext.Usuarios.AddAsync(modeloUsuario);
            await _dbPruebaContext.SaveChangesAsync();

            if (modeloUsuario.IdUsuario != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var usuarioEncontrado = await _dbPruebaContext.Usuarios
                                                    .Where(u =>
                                                        u.Correo == objeto.Correo &&
                                                        u.Clave == _utilidades.encriptarSHA256(objeto.Clave)
                                                      ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
        }

        [HttpGet]
        [Route("ValidarToken")]
        public IActionResult ValidarToken([FromQuery] string token)
        {
            bool respuesta = _utilidades.validarToken(token);

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta});
        }
    }
}
