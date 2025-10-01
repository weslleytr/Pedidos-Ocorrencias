using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Core;
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
        [SwaggerOperation(
            Summary = "Cria uma nova ocorrência",
            Description = "Registra uma ocorrência vinculada a um pedido existente."
        )]
        public async Task<IResult> Criar([FromBody] CreateOcorrenciaDto dto)
        {
            var ocorrencia = await _createOcorrenciaHandler.Handle(dto);
            return Results.Extensions.MapResults(ocorrencia);
        }

        [HttpDelete("delete-ocorrencia")]
        [SwaggerOperation(
            Summary = "Exclui uma ocorrência",
            Description = "Remove uma ocorrência existente de um pedido, respeitando as regras de negócio."
        )]
        public async Task<IResult> Remover([FromBody] DeleteOcorrenciaDto dto)
        {
            var ocorrencia = await _deleteOcorrenciaHandler.Handle(dto);
            return Results.Extensions.MapResults(ocorrencia);
        }
    }
}
