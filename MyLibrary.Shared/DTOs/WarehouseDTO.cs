using MyLibrary.Server.Data.Entities;
using MyLibrary.Shared.Interfaces.IDTOs;
using System.Diagnostics;

namespace MyLibrary.Server.Data.DTOs
{
    public class WarehouseDTO : IWarehouseDTO
    {
        public int Id { get; set; }

        public required string ISBN { get; set; }

        public required string Name { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            try
            {
                var dtoToCompare = obj as IWarehouseDTO;

                return (this.Id == dtoToCompare?.Id
                    && this.ISBN == dtoToCompare.ISBN
                    && this.Name.Equals(dtoToCompare.Name, StringComparison.OrdinalIgnoreCase)
                    && this.Price == dtoToCompare.Price
                    && this.Quantity == dtoToCompare.Quantity);
            }
            catch
            {
                return false;
            }
        }
    }
}
