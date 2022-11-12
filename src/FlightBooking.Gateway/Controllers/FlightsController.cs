using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightBooking.FlightService.Dto;
using FlightBooking.Gateway.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightBooking.Gateway.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("/api/v1/flights")]
public class FlightsController : ControllerBase
{
    private readonly ILogger<FlightsController> _logger;
    private readonly IFlightsRepository _flightsRepository;
    
    public FlightsController(ILogger<FlightsController> logger, IFlightsRepository flightsRepository)
    {
        _logger = logger;
        _flightsRepository = flightsRepository;
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
    public async Task<IActionResult> Get([Range(1, int.MaxValue)] int page, [Range(1, 100)] int size)
    {
        try
        {
            var response = await _flightsRepository.GetAllAsync(page, size);
            return Ok(response);
        }
        catch (HttpRequestException ex) when ((int?)ex.StatusCode < 500)
        {
            var statusCode = ex.StatusCode ?? HttpStatusCode.BadRequest;
            return StatusCode((int)statusCode, ex.Source);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Service is inoperative, please try later on");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "flight-service");
        }
        catch (BrokenCircuitException ex)
        {
            _logger.LogError(ex, "Service is inoperative, please try later on (BrokenCircuit)");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "flight-service");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error!");
            throw;
        }
    }
}