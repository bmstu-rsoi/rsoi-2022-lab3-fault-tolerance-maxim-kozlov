using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FlightBooking.FlightService.Database;
using FlightBooking.FlightService.Database.Entities;
using FlightBooking.FlightService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightBooking.FlightService.Controllers;

[ApiController]
[Route("/api/v1/flights")]
public class FlightsController : ControllerBase
{
    private readonly ILogger<FlightsController> _logger;
    private readonly FlightsContext _context;
    private readonly IMapper _mapper;
    
    public FlightsController(ILogger<FlightsController> logger, FlightsContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Получить список рейсов
    /// </summary>
    /// <param name="page">Номер страницы </param>
    /// <param name="size">Количество элементов на странице </param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(PaginationFlightsResponse), description: "Список рейсов")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetAll([Range(1, int.MaxValue)] int page, [Range(1, 100)] int size)
    {
        try
        {
            var allCount = await _context.Flights.CountAsync();
            
            var flights = await _context.Flights
                .AsNoTracking()
                .OrderByDescending(x => x.Datetime)
                .Skip((page - 1) * size)
                .Take(size)
                .Include(x => x.FromAirport)
                .Include(x => x.ToAirport)
                .ToListAsync();

            var response = new PaginationFlightsResponse()
            {
                PageSize = size,
                Page = page,
                TotalElements = allCount,
                Flights = flights.Select(flight => _mapper.Map<Flight, FlightResponse>(flight)).ToArray()
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
    
    /// <summary>
    /// Получить рейс
    /// </summary>
    /// <param name="flightNumber">Номер полета </param>
    /// <returns></returns>
    [HttpGet("{flightNumber}")]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(FlightResponse), description: "Рейс")]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(FlightResponse), description: "Рейс не найден ")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> Get(string flightNumber)
    {
        try
        {
            var flight = await _context.Flights
                .AsNoTracking()
                .Include(x => x.FromAirport)
                .Include(x => x.ToAirport)
                .FirstOrDefaultAsync(x => x.FlightNumber == flightNumber);
            
            if (flight == null)
                return NotFound(flightNumber);

            return Ok(_mapper.Map<Flight, FlightResponse>(flight));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}