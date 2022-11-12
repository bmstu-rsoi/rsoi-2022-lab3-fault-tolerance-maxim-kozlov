using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FlightBooking.BonusService.Database;
using FlightBooking.BonusService.Database.Entities;
using FlightBooking.BonusService.Dto;
using FlightBooking.BonusService.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightBooking.BonusService.Controllers;

[ApiController]
[Route("/api/v1/history")]
public class HistoryController : ControllerBase
{
    private readonly BonusContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<HistoryController> _logger;

    public HistoryController(ILogger<HistoryController> logger, BonusContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Информация по истории покупок пользователя
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(BalanceHistoryDto[]), description: "История пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Get([Required, FromQuery(Name = "username")] string username)
    {
        try
        {
            var privilege = await _context.Privileges
                .AsNoTracking()
                .Include(x => x.PrivilegeHistories)
                .FirstOrDefaultAsync(x => x.Username == username);
            
            if (privilege == null)
                return Ok(Array.Empty<BalanceHistoryDto>());
            
            return Ok(privilege.PrivilegeHistories.Select(history => _mapper.Map<PrivilegeHistory, BalanceHistoryDto>(history)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
    
    /// <summary>
    /// Добавить историю покупок пользователя
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(statusCode: StatusCodes.Status201Created, type: typeof(BalanceHistoryDto), description: "История пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Post([Required, FromQuery(Name = "username")] string username, [FromBody] TicketPurchaseRequest request)
    {
        try
        {
            var privilege = await _context.Privileges
                .FirstOrDefaultAsync(x => x.Username == username);
            
            if (privilege == null)
                return StatusCode(403);

            var entity = _mapper.Map<TicketPurchaseRequest, PrivilegeHistory>(request);
            entity.PrivilegeId = privilege.Id;

            if (request.PaidFromBalance)
            {
                if (request.Price > privilege.Balance)
                    entity.BalanceDiff = -privilege.Balance;
                else
                    entity.BalanceDiff = -request.Price;
                entity.OperationType = OperationTypeDto.DebitAccount.GetValue();
            }
            else
            {
                entity.BalanceDiff = (int)(request.Price * 0.1);
                entity.OperationType = OperationTypeDto.FillInBalance.GetValue();
            }
            entity.Datetime = DateTime.Now;
            privilege.Balance += entity.BalanceDiff;
            
            await _context.PrivilegeHistories.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return Created($"/api/v1/history/{entity.Id}", _mapper.Map<BalanceHistoryDto>(entity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }

    /// <summary>
    /// Добавить историю покупок пользователя
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <param name="ticketUid"></param>
    /// <returns></returns>
    [HttpDelete("{ticketUid:guid}")]
    [SwaggerResponse(statusCode: StatusCodes.Status201Created, type: typeof(BalanceHistoryDto), description: "История пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Delete([Required, FromQuery(Name = "username")] string username, Guid ticketUid)
    {
        try
        {
            var privilege = await _context.Privileges
                .FirstOrDefaultAsync(x => x.Username == username);
            
            if (privilege == null)
                return StatusCode(403);
            
            var lastOperation = await _context.PrivilegeHistories
                .OrderByDescending(x => x.Datetime)
                .LastOrDefaultAsync(x => x.TicketUid == ticketUid);
            
            if (lastOperation == null)
                return NotFound(ticketUid);

            var newOperation = new PrivilegeHistory();
            _mapper.Map(lastOperation, newOperation);
            
            var balanceDiff = -lastOperation.BalanceDiff;
            if (balanceDiff > 0)
                newOperation.OperationType = OperationTypeDto.FillInBalance.GetValue();
            else
            {
                if (balanceDiff + privilege.Balance < 0)
                    balanceDiff = -privilege.Balance;
                newOperation.OperationType = OperationTypeDto.DebitAccount.GetValue();
            }

            newOperation.Datetime = DateTime.Now;
            newOperation.BalanceDiff = balanceDiff;

            await _context.PrivilegeHistories.AddAsync(newOperation);
            
            privilege.Balance += balanceDiff;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<BalanceHistoryDto>(newOperation));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}