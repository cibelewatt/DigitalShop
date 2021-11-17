using LetsShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsShop.TransactModels
{
    public class CheckoutTransactModel
    {
        public List<ProductInCart> productsInCart { get; set; }
        public double totalValue { get; set; }

        
    }
}
