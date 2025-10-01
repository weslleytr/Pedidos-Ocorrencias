using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Application.Handler.Pedido;
using OrderFlow.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderFlow.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class OcorrenciaController : ControllerBase
    {
        private readonly ILogger<OcorrenciaController> _logger;
        private readonly CreateOcorrenciaHandler _createOcorrenciaHandler;
        private readonly DeleteOcorrenciaHandler _deleteOcorrenciaHandler;
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public OcorrenciaController(CreateOcorrenciaHandler createOcorrenciaHandler, DeleteOcorrenciaHandler deleteOcorrenciaHandler ,IOcorrenciaRepository ocorrenciaRepository, ILogger<OcorrenciaController> logger)
        {
            _createOcorrenciaHandler = createOcorrenciaHandler;
            _deleteOcorrenciaHandler = deleteOcorrenciaHandler;
            _ocorrenciaRepository = ocorrenciaRepository;
            _logger = logger;
        }

        [HttpPost("create-ocorrencia")]
        [SwaggerOperation(
            Summary = "Cria uma nova ocorrência",
            Description = "Registra uma ocorrência vinculada a um pedido existente."
        )]
        public async Task<IActionResult> Criar([FromBody] CreateOcorrenciaDto dto)
        {
            try
            {
                var ocorrencia = await _createOcorrenciaHandler.Handle(dto);
                _logger.LogInformation("Ocorrência criada com sucesso para o pedido {Pedido}", dto.NumeroPedido);
                return Ok("Ocorrencia Registrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar ocorrência para o pedido {Pedido}", dto.NumeroPedido);
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpDelete("delete-ocorrencia")]
        [SwaggerOperation(
            Summary = "Exclui uma ocorrência",
            Description = "Remove uma ocorrência existente de um pedido, respeitando as regras de negócio."
        )]
        public async Task<IActionResult> Remover([FromBody] DeleteOcorrenciaDto dto)
        {
            try
            {
                var ocorrencia = await _deleteOcorrenciaHandler.Handle(dto);
                _logger.LogInformation("Ocorrência excluída com sucesso para o pedido {Pedido}", dto.NumeroPedido);
                return Ok("Ocorrencia Removida");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir ocorrência para o pedido {Pedido}", dto.NumeroPedido);
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
