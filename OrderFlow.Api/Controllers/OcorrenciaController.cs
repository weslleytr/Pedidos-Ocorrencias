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
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public OcorrenciaController(CreateOcorrenciaHandler createOcorrenciaHandler, IOcorrenciaRepository ocorrenciaRepository)
        {
            _createOcorrenciaHandler = createOcorrenciaHandler;
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        [HttpPost("create-ocorrencia/{idPedido}")]
        public async Task<IActionResult> Criar([FromBody] CreateOcorrenciaDto dto, int idPedido)
        {
            try
            {
                var ocorrencia = await _createOcorrenciaHandler.Handle(idPedido, dto);
                return Ok("Ocorrencia Registrada");
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
