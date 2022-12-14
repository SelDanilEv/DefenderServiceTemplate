using AutoMapper;
using Rentel.ServiceTemplate.WebUI.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentel.ServiceTemplate.Application.Modules.Home.Queries;
using Rentel.ServiceTemplate.Domain.Models;
using Rentel.ServiceTemplate.Application.DTOs;
using Rentel.ServiceTemplate.Application.Common.Enums;

namespace Rentel.ServiceTemplate.WebUI.Controllers.V1;

public class HomeController : BaseApiController
{
    public HomeController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("health")]
    [ProducesResponseType(typeof(HealthDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<HealthDto> HealthCheckAsync()
    {
        return new HealthDto { Status = "Healthy" };
    }

    [HttpGet("authorization/check")]
    [Auth(Roles.Any)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<object> AuthorizationCheckAsync()
    {
        return new { IsAuthorized = true };
    }

    [Auth(Roles.SuperAdmin)]
    [HttpGet("configuration")]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<Dictionary<string, string>> GetConfigurationAsync(
        ConfigurationLevel configurationLevel)
    {
        var query = new GetConfigurationQuery()
        {
            Level = configurationLevel
        };

        return await ProcessApiCallWithoutMappingAsync
            <GetConfigurationQuery, Dictionary<string, string>>
            (query);
    }
}
