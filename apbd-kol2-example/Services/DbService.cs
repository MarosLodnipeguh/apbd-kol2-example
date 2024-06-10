using apbd_kol2.DTOs;
using ExampleTest2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace apbd_kol2_example.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<int> GetClientId(string? lastname)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.LastName == lastname);
        if (client == null)
        {
            throw new Exception("Client not found");
        }

        return client.Id;
    }

    public async Task<IEnumerable<OrderInfoDto>> GetOrders(int? clientId)
    {
        if (clientId == null)
        {
            // return all orders
            var allOrders = await _context.Orders.ToListAsync();

            var allOrderInfoDtos = new List<OrderInfoDto>();

            foreach (var order in allOrders)
            {
                var orderInfoDto = new OrderInfoDto
                {
                    Id = order.Id,
                    AcceptedAt = order.AcceptedAt,
                    FulfilledAt = order.FulfilledAt,
                    Comments = order.Comments,
                    pastries = await GetPastries(order.Id)
                };
                allOrderInfoDtos.Add(orderInfoDto);
            }

            return allOrderInfoDtos;
        }

        var orders = await _context.Orders
            .Where(o => o.ClientId == clientId)
            .ToListAsync();

        var orderInfoDtos = new List<OrderInfoDto>();

        foreach (var order in orders)
        {
            var orderInfoDto = new OrderInfoDto
            {
                Id = order.Id,
                AcceptedAt = order.AcceptedAt,
                FulfilledAt = order.FulfilledAt,
                Comments = order.Comments,
                pastries = await GetPastries(order.Id)
            };

            orderInfoDtos.Add(orderInfoDto);
        }

        return orderInfoDtos;
    }

    public async Task<IEnumerable<OrderPastryDto>> GetPastries(int orderId)
    {
        var orderPastries = await _context.OrderPastries
            .Where(op => op.OrderId == orderId)
            .ToListAsync();

        var orderPastryDtos = new List<OrderPastryDto>();

        foreach (var orderPastry in orderPastries)
        {
            var pastry = await _context.Pastries
                .FirstOrDefaultAsync(p => p.Id == orderPastry.PastryId);

            if (pastry == null)
            {
                throw new Exception("Pastry not found");
            }

            var orderPastryDto = new OrderPastryDto
            {
                Name = pastry.Name,
                Price = pastry.Price,
                Amount = orderPastry.Amount,
                Comment = orderPastry.Comment
            };

            orderPastryDtos.Add(orderPastryDto);
        }

        return orderPastryDtos;
    }
    

    // ===================== add new order =====================


    public async Task<bool> DoesClientExist(int clientId)
    {
        return await _context.Clients.AnyAsync(e => e.Id == clientId);
    }

    public async Task<bool> DoesEmployeeExist(int employeeId)
    {
        return await _context.Employees.AnyAsync(e => e.Id == employeeId);
    }
    
    public async Task<bool> DoesPastryExist(string pastryName)
    {
        var pastry = await _context.Pastries.FirstOrDefaultAsync(p => p.Name == pastryName);
        return pastry != null;
    }

    public async Task AddOrder(Order order)
    {
        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task AddOrderPastries(IEnumerable<OrderPastry> orderPastries)
    {
        await _context.AddRangeAsync(orderPastries);
        await _context.SaveChangesAsync();
    }
    
    public async Task<int> GetPastryId(string pastryName)
    {
        var pastry = await _context.Pastries.FirstOrDefaultAsync(p => p.Name == pastryName);
        return pastry.Id;
    }
}