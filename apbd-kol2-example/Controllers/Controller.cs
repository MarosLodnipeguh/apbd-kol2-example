using System.Diagnostics.CodeAnalysis;
using apbd_kol2_example.Services;
using apbd_kol2.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace apbd_kol2.Controllers;

[ApiController]
[Route("api/")]
public class Controller : ControllerBase
{
    private readonly IDbService _service;

    public Controller(IDbService dbService)
    {
        _service = dbService;
    }
    
    
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders(string? lastName)
    {
        try
        {
            if (lastName == null)
            {
                var allOrders = await _service.GetOrders(null);
                return Ok(allOrders);
            }
            var clientId = await _service.GetClientId(lastName);
            var orders = await _service.GetOrders(clientId);
            return Ok(orders);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("clients/{clientId}/orders")]
    public async Task<IActionResult> AddOrder(int clientId, [FromBody] NewOrderInfoDto newOrderInfoDto)
    {
        // check if client exists
        if (!await _service.DoesClientExist(clientId))
        {
            return BadRequest("Client does not exist");
        }
        // check if employee exists
        if (!await _service.DoesEmployeeExist(newOrderInfoDto.employeeId))
        {
            return BadRequest("Employee does not exist");
        }
        // check if all pastries exist
        foreach (var pastry in newOrderInfoDto.pastries)
        {
            if (!await _service.DoesPastryExist(pastry.Name))
            {
                return BadRequest("Pastry does not exist");
            }
        }
        
        // add order and pastries to db
        var orderId = await _service.AddOrder(newOrderInfoDto, clientId);
        
        return Ok("Order added with id: " + orderId);
    }
    
    
    
    
    
    
    
    
    
    
    
}