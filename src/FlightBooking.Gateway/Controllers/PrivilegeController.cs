using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightBooking.BonusService.Dto;
using FlightBooking.Gateway.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightBooking.Gateway.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("/api/v1/privilege")]
public class PrivilegeController: ControllerBase
{
    private readonly ILogger<PrivilegeController> _logger;
    private readonly IPrivilegeRepository _privilegeRepository;
    
    public PrivilegeController(ILogger<PrivilegeController> logger, IPrivilegeRepository privilegeRepository)
    {
        _logger = logger;
        _privilegeRepository = privilegeRepository;
    }
    
    /// <summary>
    /// Получить информацию о состоянии бонусного счета
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(PrivilegeDto), description: "Данные о бонусном счете.")]
    [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Get([Required, FromHeader(Name = "X-User-Name")] string username)
    {
        try
        {
            var response = await _privilegeRepository.GetAsync(username, needHistory: true);
            return Ok(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound(username);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}