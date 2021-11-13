using LetsShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static List<Product> ProductsInCart = new List<Product>();

        [HttpGet]
        public IActionResult GetProductsInCart()
        {
            return Ok(ProductsInCart);
        }

        [HttpPost]
        [Route("Products/add/{productIndex}")]
        public IActionResult AddProduct([FromRoute] int productIndex)
        {
            try
            {
                ProductsInCart.Add(ProductsController.Products[productIndex]);
                return Ok("O produto " + ProductsController.Products[productIndex].Name + " foi adicionado ao carrinho com sucesso!");
            }
            catch
            {
                return StatusCode(501, "O produto de index " + productIndex + " não existe em nossa loja.");
            }
        }

        [HttpDelete]
        [Route("Products/delete/{productIndex}")]
        public IActionResult DeleteProduct([FromRoute] int productIndex)
        {
            try
            {
                var productToRemove = ProductsInCart.Where(x => x.Name == ProductsController.Products[productIndex].Name).FirstOrDefault();
                ProductsInCart.Remove(productToRemove);
                return Ok("O produto " + productToRemove.Name + " foi deletado do carrinho.");
            }
            catch
            {
                return StatusCode(501, "O produto de index " + productIndex + " não existe em nossa loja.");
            }
        }
    }
}
