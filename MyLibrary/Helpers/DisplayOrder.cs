using CommunityToolkit.Mvvm.ComponentModel;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Helpers
{
    public class DisplayOrder : ObservableObject, IOrder
    {
        public string? ItemId { get; set; }
        public string? ItemISBN { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }

        public decimal? Total => Price * Quantity;

        public DisplayOrder(IOrder order)
        {
            ItemId = order.ItemId;
            ItemISBN = order.ItemISBN;
            ItemName = order.ItemName;
            Price = order.Price;
            Quantity = order.Quantity;
        }
    }
}
