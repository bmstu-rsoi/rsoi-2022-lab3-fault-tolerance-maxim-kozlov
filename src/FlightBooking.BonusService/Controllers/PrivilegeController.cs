using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FlightBooking.BonusService.Database;
using FlightBooking.BonusService.Database.Entities;
using FlightBooking.BonusService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightBooking.BonusService.Controllers;

[ApiController]
[Route("/api/v1/privilege")]
public class PrivilegeController : ControllerBase
{
    private readonly BonusContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PrivilegeController> _logger;

    public PrivilegeController(ILogger<PrivilegeController> logger, BonusContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Информация по привилегии пользователя
    /// </summary>
    /// <param name="username">Имя пользователя </param>
    /// <param name="needHistory">Требуется ли история пользователя </param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(PrivilegeDto), description: "Привилегия пользователя.")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Get([Required, FromQuery(Name = "username")] string username, bool needHistory = false)
    {
        try
        {
            var query = _context.Privileges
                .AsNoTracking();

            if (needHistory)
                query = query.Include(x => x.PrivilegeHistories);
            
            var privilege = await query.FirstOrDefaultAsync(x => x.Username == username);
            if (privilege == null)
                return NotFound(username);
        
            return Ok(_mapper.Map<Privilege, PrivilegeDto>(privilege));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}