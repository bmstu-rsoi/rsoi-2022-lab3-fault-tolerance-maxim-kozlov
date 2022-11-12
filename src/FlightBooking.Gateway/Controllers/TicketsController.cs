using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightBooking.Gateway.Dto.Tickets;
using FlightBooking.Gateway.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Http;
using AutoMapper;
using FlightBooking.Gateway.Domain;

namespace FlightBooking.Gateway.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("/api/v1/tickets")]
public class TicketsController: ControllerBase
{
    private readonly ILogger<TicketsController> _logger;
    private readonly ITicketsService _ticketsService;
    private readonly IMapper _mapper;
    
    public TicketsController(ILogger<TicketsController> logger, IMapper mapper, ITicketsService ticketsService)
    {
        _logger = logger;
        _mapper = mapper;
        _ticketsService = ticketsService;
    }
    
    /// <summary>
    /// Информация по всем билетам пользователя
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(TicketResponse[]), description: "Список билетов пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetAll([Required, FromHeader(Name = "X-User-Name")] string username)
    {
        try
        {
            var tickets = await _ticketsService.GetAllAsync(username);
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }

    /// <summary>
    /// Информация по конкретному билету пользователя
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <param name="ticketUid">UUID билета </param>
    /// <returns></returns>
    [HttpGet("{ticketUid:guid}")]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(TicketResponse), description: "Билет пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Билет не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Get([Required, FromHeader(Name = "X-User-Name")] string username, Guid ticketUid)
    {
        try
        {
            var ticket = await _ticketsService.GetAsync(username, ticketUid);
            return Ok(ticket);
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
    
    /// <summary>
    /// Покупка билета
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <param name="request">Информация для покупки </param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(TicketPurchaseResponse), description: "Список билетов пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Рейс не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status409Conflict, description: "Нет билетов на рейс.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Purchase([Required, FromHeader(Name = "X-User-Name")] string username, [FromBody] TicketPurchaseRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _ticketsService.PurchaseAsync(username, request);
            return Ok(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
    
    /// <summary>
    /// Возврат билета
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <param name="ticketUid">UUID билета </param>
    /// <returns></returns>
    [HttpDelete("{ticketUid:guid}")]
    [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "Возврат билета успешно выполнен.")]
    [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Билет не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Delete([Required, FromHeader(Name = "X-User-Name")] string username, Guid ticketUid)
    {
        try
        {
            await _ticketsService.DeleteAsync(username, ticketUid);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}