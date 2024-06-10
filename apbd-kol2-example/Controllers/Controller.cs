using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using apbd_kol2_example.Services;
using apbd_kol2.DTOs;
using ExampleTest2.Models;
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
            return NotFound("Client does not exist");
        }
        // check if employee exists
        if (!await _service.DoesEmployeeExist(newOrderInfoDto.employeeId))
        {
            return NotFound("Employee does not exist");
        }
        // check if all pastries exist
        foreach (var pastry in newOrderInfoDto.pastries)
        {
            if (!await _service.DoesPastryExist(pastry.Name))
            {
                return NotFound("Pastry does not exist");
            }
        }
        
        // create order
        var order = new Order
        {
            ClientId = clientId,
            EmployeeId = newOrderInfoDto.employeeId,
            AcceptedAt = newOrderInfoDto.acceptedAt,
            Comments = newOrderInfoDto.comments
        };
        
        // create pastries
        var pastries = new List<OrderPastry>();
        foreach (var pastry in newOrderInfoDto.pastries)
        {
            pastries.Add(new OrderPastry
            {
                OrderId = order.Id,
                PastryId = await _service.GetPastryId(pastry.Name),
                Amount = pastry.Amount,
                Comment = pastry.Comment
            });
        }

        // add order and pastries to db
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _service.AddOrder(order);
            await _service.AddOrderPastries(pastries);
    
            scope.Complete();
        }
    
        return Created("api/orders", new
        {
            order.Id,
            order.AcceptedAt,
            order.FulfilledAt,
            order.Comments,
        });
        
        
        
    }
    
    
    
    
    
    
    
    
    
    
    
}