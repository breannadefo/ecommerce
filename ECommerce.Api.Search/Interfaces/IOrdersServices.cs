using ECommerce.Api.Search.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IOrdersServices
    {
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)>
            GetOrdersAsync(int customerId);
    }
}
