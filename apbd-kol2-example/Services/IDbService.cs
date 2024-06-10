using apbd_kol2.DTOs;

namespace apbd_kol2_example.Services;

public interface IDbService
{
    Task<int> GetClientId(string? lastname);
    Task<IEnumerable<OrderInfoDto>> GetOrders(int? clientId);
    Task<IEnumerable<OrderPastryDto>> GetPastries(int orderId);
    
    
    Task<bool> DoesClientExist(int clientId);
    Task<bool> DoesEmployeeExist(int employeeId);
    Task<bool> DoesPastryExist(string pastryName);
    Task<int> GetPastryId(string pastryName);
    Task<int> AddOrder(NewOrderInfoDto newOrderInfoDto, int clientId);
    Task<int> AddOrderPastry(NewOrderPastryDto newOrderPastryDto, int orderId);


    

}