using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CnpjApi.Services;
using CnpjApi.DTOs;
using System.Collections.Generic;

namespace CnpjApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/v1/cnpj")]
    public class CNPJController : ControllerBase
    {
        private readonly ICNPJService _cnpjService;

        public CNPJController(ICNPJService cnpjService)
        {
            _cnpjService = cnpjService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{cnpj}")]
        public async Task<IActionResult> ConsultarCNPJ(string cnpj)
        {
            if (!ValidarCNPJ(cnpj))
                return BadRequest(new { message = "CNPJ inválido" });

            var resultado = await _cnpjService.ConsultarCNPJ(cnpj);

            if (resultado == null)
                return NotFound(new { message = "CNPJ não encontrado" });

            return Ok(resultado);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("filtros")]
        public async Task<IActionResult> ConsultarPorFiltros(
            [FromQuery] string? uf = null,
            [FromQuery] int ano = 0,
            [FromQuery] string? formaTributacao = null,
            [FromQuery] decimal capitalSocialMinimo = 0)
        {
            var filtros = new ConsultaFiltrosDto
            {
                UF = uf,
                Ano = ano,
                FormaTributacao = formaTributacao,
                CapitalSocialMinimo = capitalSocialMinimo
            };

            var resultados = await _cnpjService.ConsultarPorFiltros(filtros);
            return Ok(resultados);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{cnpj}/existe")]
        public async Task<IActionResult> VerificarExistencia(string cnpj)
        {
            if (!ValidarCNPJ(cnpj))
                return BadRequest(new { message = "CNPJ inválido" });

            var existe = await _cnpjService.CNPJExiste(cnpj);
            return Ok(new { existe });
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> ConsultarPorCPFSocio(string cpf)
        {
            var cpfLimpo = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpfLimpo.Length != 11)
                return BadRequest(new { message = "CPF inválido" });

            // Implementar busca por CPF de sócio
            return Ok(new { message = "Funcionalidade em desenvolvimento" });
        }

        /// <summary>
        /// Valida se o CNPJ possui o formato correto
        /// Um CNPJ válido deve ter 14 dígitos e não pode ter todos os dígitos iguais
        /// </summary>
        /// <param name="cnpj">CNPJ a ser validado</param>
        /// <returns>True se o CNPJ for válido, False caso contrário</returns>
        private bool ValidarCNPJ(string cnpj)
        {
            var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());

            // Um CNPJ válido deve ter exatamente 14 dígitos
            if (cnpjLimpo.Length != 14)
                return false;

            // Verifica se todos os dígitos são iguais (caso comum de CNPJ inválido)
            if (cnpjLimpo.All(d => d == cnpjLimpo[0]))
                return false;

            return true;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "OK",
                timestamp = DateTime.UtcNow,
                message = "API CNPJ está funcionando"
            });
        }

    }
}