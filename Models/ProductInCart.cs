using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsShop.Models
{
    public class ProductInCart
    {

        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
