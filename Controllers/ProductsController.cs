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
    public class ProductsController : ControllerBase
    {
        private List<Product> Products = new List<Product>();

        [HttpPost]
        [Route("create")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            var existingProduct = Products.Where(x => x.Name == product.Name);

            if (existingProduct.Any()) return StatusCode(409, "Um produto com esse nome já existe em nossa loja."); //208/422
            else return StatusCode(201, "O produto foi adicionado com sucesso!");
        }

        [HttpGet]
        [Route("products")]
        public IActionResult GetProducts()
        {
            return Ok(Products);
        }


    }
}
