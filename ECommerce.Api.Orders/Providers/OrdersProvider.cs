using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        
        private void SeedData()
        {
            if (!dbContext.OrderItems.Any())
            {
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 1, ProductId = 3, Quantity = 4, UnitPrice = 150 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 2, ProductId = 2, Quantity = 1, UnitPrice = 5 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 3, ProductId = 4, Quantity = 5, UnitPrice = 200 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 4, ProductId = 2, Quantity = 8, UnitPrice = 5 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 5, ProductId = 1, Quantity = 3, UnitPrice = 20 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 6, ProductId = 1, Quantity = 2, UnitPrice = 20 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 7, ProductId = 3, Quantity = 9, UnitPrice = 150 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 8, ProductId = 2, Quantity = 3, UnitPrice = 5 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 9, ProductId = 4, Quantity = 1, UnitPrice = 200 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 10, ProductId = 3, Quantity = 7, UnitPrice = 150 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 11, ProductId = 1, Quantity = 4, UnitPrice = 20 });
                dbContext.OrderItems.Add(new Db.OrderItem() { Id = 12, ProductId = 4, Quantity = 11, UnitPrice = 200 });
                dbContext.SaveChanges();
            }

            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order() { 
                    Id = 1, 
                    CustomerId = 2, 
                    OrderDate = DateTime.Now.AddDays(-13), 
                    Items = new List<Db.OrderItem>()
                    {
                        dbContext.OrderItems.Find(2),
                        dbContext.OrderItems.Find(6),
                        dbContext.OrderItems.Find(1),
                    },
                    Total = 645
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-27),
                    Items = new List<Db.OrderItem>()
                    {
                        dbContext.OrderItems.Find(7),
                        dbContext.OrderItems.Find(11),
                    },
                    Total = 1430
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 3,
                    CustomerId = 3,
                    OrderDate = DateTime.Now.AddDays(-5),
                    Items = new List<Db.OrderItem>()
                    {
                        dbContext.OrderItems.Find(12),
                    },
                    Total = 2200
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 4,
                    CustomerId = 2,
                    OrderDate = DateTime.Now.AddDays(-55),
                    Items = new List<Db.OrderItem>()
                    {
                        dbContext.OrderItems.Find(8),
                        dbContext.OrderItems.Find(5),
                    },
                    Total = 75
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 5,
                    CustomerId = 5,
                    OrderDate = DateTime.Now.AddDays(-7),
                    Items = new List<Db.OrderItem>()
                    {
                        dbContext.OrderItems.Find(4),
                        dbContext.OrderItems.Find(9),
                        dbContext.OrderItems.Find(10),
                    },
                    Total = 1290
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 6,
                    CustomerId = 4,
                    OrderDate = DateTime.Now,
                    Items = new List<Db.OrderItem>()
                    {
                        dbContext.OrderItems.Find(3),
                    },
                    Total = 1000
                });

                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                var orders = await dbContext.Orders.ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
