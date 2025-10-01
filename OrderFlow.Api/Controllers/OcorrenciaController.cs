using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Application.Handler.Pedido;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class OcorrenciaController : ControllerBase
    {
        private readonly CreateOcorrenciaHandler _createOcorrenciaHandler;
        private readonly DeleteOcorrenciaHandler _deleteOcorrenciaHandler;
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public OcorrenciaController(CreateOcorrenciaHandler createOcorrenciaHandler, DeleteOcorrenciaHandler deleteOcorrenciaHandler ,IOcorrenciaRepository ocorrenciaRepository)
        {
            _createOcorrenciaHandler = createOcorrenciaHandler;
            _deleteOcorrenciaHandler = deleteOcorrenciaHandler;
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        [HttpPost("create-ocorrencia")]
        public async Task<IActionResult> Criar([FromBody] CreateOcorrenciaDto dto)
        {
            try
            {
                var ocorrencia = await _createOcorrenciaHandler.Handle(dto);
                return Ok("Ocorrencia Registrada");
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpDelete("delete-ocorrencia")]
        public async Task<IActionResult> Remover(int numeroPedido, int idOcorrencia)
        {
            try
            {
                var ocorrencia = await _deleteOcorrenciaHandler.Handle(numeroPedido, idOcorrencia);
                return Ok("Ocorrencia Removida");
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
