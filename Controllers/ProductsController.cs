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
        private static List<Product> Products = new List<Product>();
        private static int CurrentId = 0;

        [HttpPost]
        [Route("create")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            var existingProduct = Products.Where(x => x.Name == product.Name);

            if (existingProduct.Any())
            {
                return StatusCode(409, "Um produto com esse nome já existe em nossa loja."); //208/422
            }
            else
            {
                product.Id = CurrentId++;
                Products.Add(product);
                return StatusCode(201, "O produto foi adicionado com sucesso!");
            }
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetProducts()
        {
            return Ok(Products);
        }

        [HttpPatch]
        [Route("update/{index}")]
        public IActionResult UpdateProduct([FromRoute] int index, [FromBody] Product product)
        {
            try
            {
                product.Id = Products[index].Id;
                Products[index] = product;
                return Ok(Products[index]);
            }
            catch
            {
                return StatusCode(500, "Erro.");
            }
        }

        [HttpDelete]
        [Route("delete/{index}")]
        public IActionResult DeleteProduct([FromRoute] int index)
        {
            try
            {
                Product productToRemove = Products[index];
                Products.RemoveAt(index);
                return Ok("Produto Removido: " + productToRemove.Name);
            }
            catch
            {
                return StatusCode(500, "Erro.");
            }
        }

    }
}
