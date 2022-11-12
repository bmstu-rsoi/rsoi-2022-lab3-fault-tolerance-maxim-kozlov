using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightBooking.Gateway.Domain;
using FlightBooking.Gateway.Dto.Users;
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
            response.Privilege = await _privilegeRepository.GetAsync(username, needHistory: false);
            
            try
            {
                var tickets = await _ticketsService.GetAllAsync(username);
                response.Tickets = tickets.ToArray();
            }
            catch (Exception e)
            {
                // ignore
            }
            
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