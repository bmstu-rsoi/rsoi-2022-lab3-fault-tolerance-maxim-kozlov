using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FlightBooking.TicketService.Database;
using FlightBooking.TicketService.Database.Entities;
using FlightBooking.TicketService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightBooking.TicketService.Controllers;

[ApiController]
[Route("/api/v1/tickets")]
public class TicketsController : ControllerBase
{
    private readonly TicketsContext _context;
    private readonly ILogger<TicketsController> _logger;
    private readonly IMapper _mapper;
    
    public TicketsController(ILogger<TicketsController> logger, TicketsContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Информация по всем билетам пользователя
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(TicketDto[]), description: "Список билетов пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetAll([Required, FromQuery(Name = "username")] string username)
    {
        try
        {
            var tickets = await _context.Tickets
                .AsNoTracking()
                .Where(x => x.Username == username)
                .ToListAsync();
            
            return Ok(tickets.Select(ticket => _mapper.Map<Ticket, TicketDto>(ticket)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
    
    /// <summary>
    /// Добавление оплаченного билета
    /// </summary>
    /// <param name="ticketDto">Билет </param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(TicketDto), description: "Создан билет пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Post([FromBody] TicketDto ticketDto)
    {
        try
        {
            var entity = new Ticket();
            _mapper.Map(ticketDto, entity);
            if (ticketDto.TicketUid == default)
                entity.TicketUid = Guid.NewGuid();

            _context.Tickets.Add(entity);

            await _context.SaveChangesAsync();

            return Created($"/api/v1/{entity.TicketUid}", _mapper.Map<TicketDto>(entity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }

    /// <summary>
    /// Информация по конкретному билету
    /// </summary>
    /// <param name="username"></param>
    /// <param name="ticketId">UUID билета </param>
    /// <returns></returns>
    [HttpGet("{ticketId:guid}")]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(TicketDto), description: "Билет пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Билет не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Get([Required, FromQuery(Name = "username")] string username, Guid ticketId)
    {
        try
        {
            var ticket = await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TicketUid == ticketId && x.Username == username);
            
            if (ticket == null)
                return NotFound(ticketId);

            return Ok(_mapper.Map<Ticket, TicketDto>(ticket));
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
    /// <param name="ticketId">UUID билета </param>
    /// <returns></returns>
    [HttpDelete("{ticketId:guid}")]
    [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "Возврат билета успешно выполнен.")]
    [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Билет не найден.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Delete([Required, FromQuery(Name = "username")] string username, Guid ticketId)
    {
        try
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(x => x.TicketUid == ticketId && x.Username == username);
            
            if (ticket == null)
                return NotFound(ticketId);
            
            ticket.Status = TicketStatusDto.Canceled.ToString().ToUpper();
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}