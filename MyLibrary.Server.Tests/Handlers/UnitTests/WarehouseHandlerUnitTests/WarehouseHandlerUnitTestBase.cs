using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.WarehouseHandlerUnitTests
{
    public abstract class WarehouseHandlerUnitTestBase : UnitTestBase
    {
        protected IWarehouseHandler<IWarehouse<int>> _warehouseHandler;

        [SetUp]
        public void Setup()
        {
            _warehouseHandler = new WarehouseHandler(
                DbContext,
                new Mock<ILogger<WarehouseHandler>>().Object,
                Mapper
                );
        }

        protected async Task<ICollection<WarehouseDTO>> CreateFakeStocksAsync(
            int? id = default,
            string? isbn = default, 
            string? name = default,
            decimal? price = default,
            int? quantity = 1,
            int stocksToBeAdded = 1)
        {
            var stocks = new List<Warehouse>();

            for (int i = 1; i <= stocksToBeAdded; i++)
            {
                var stock = new Warehouse
                {
                    Id = id ?? 0 + i,
                    ISBN = isbn ?? $"978-3-16-148410-{i}",
                    Name = name ?? $"Sample Book {i}",
                    Price = price ?? 9.99m,
                    Quantity = quantity ?? 1
                };
                stocks.Add(stock);
                DbContext.Add(stock);
            }

            await DbContext.SaveChangesAsync();
            return Mapper.Map<ICollection<WarehouseDTO>>(stocks);
        }

        protected ICollection<IWarehouseDTO> NewStocks(
            string? isbn = default,
            string? name = default,
            decimal? price = default,
            int? quantity = 1,
            int stocksToBeAdded = 1
            )
        {
            var stocks = new List<IWarehouseDTO>();
            for(int i = 1; i <= stocksToBeAdded; i++)
            {
                var stock = new WarehouseDTO
                {
                    ISBN = isbn ?? $"978-3-16-148410-{i}",
                    Name = name ?? $"Sample Book {i}",
                    Price = price ?? 9.99m,
                    Quantity = quantity ?? 1
                };
                stocks.Add(stock);
            }

            return stocks;
        }
    }
}
