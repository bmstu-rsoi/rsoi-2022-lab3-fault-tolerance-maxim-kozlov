using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightBooking.BonusService.Dto;
using FlightBooking.Gateway.Domain;
using FlightBooking.Gateway.Dto;
using FlightBooking.Gateway.Dto.Users;
using FlightBooking.Gateway.Exceptions;
using FlightBooking.Gateway.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightBooking.Gateway.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("/api/v1/")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly ITicketsService _ticketsService;
    private readonly IPrivilegeRepository _privilegeRepository;
    
    public UsersController(ILogger<UsersController> logger, ITicketsService ticketsService, IPrivilegeRepository privilegeRepository)
    {
        _logger = logger;
        _ticketsService = ticketsService;
        _privilegeRepository = privilegeRepository;
    }

    /// <summary>
    /// Получить информацию о пользователе
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <returns></returns>
    [HttpGet("me")]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(UserInfoResponse), description: "Пользователь найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Get([Required, FromHeader(Name = "X-User-Name")] string username)
    {
        try
        {
            var response = new UserInfoResponse();
            var tickets = await _ticketsService.GetAllAsync(username);
            response.Tickets = tickets.ToArray();
            
            try
            {
                response.Privilege = await _privilegeRepository.GetAsync(username, needHistory: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed get bonus for {username}", username);
            }
            
            return Ok(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound(username);
        }
        catch (ServiceUnavailableException ex)
        {
            _logger.LogError(ex, "Service is inoperative, please try later on");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new MessageDto($"{ex.ServiceName} unavailable"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}