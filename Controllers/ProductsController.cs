using LetsShop.Models;
using LetsShop.Repoitories;
using LetsShop.Services;
using Microsoft.AspNetCore.Authorization;
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
        //mudei p/ public p/ ser acessível no cart
        public static List<Product> Products = new List<Product>();
        private static int CurrentId = 0;

        [HttpPost]
        [Route("funcionario/create")]
        [Authorize(Roles = "funcionario")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            var existingProduct = Products.Where(x => x.Name == product.Name);

            if (existingProduct.Any())
            {
                return StatusCode(409, "Um produto com esse nome já existe na loja."); //208/422
            }
            else
            {
                product.Id = CurrentId++;
                Products.Add(product);
                return StatusCode(201, "O produto "+ product.Name + " foi adicionado com sucesso!");
            }
        }

        [HttpGet]
        [Route("all")] //erro sem rota
        [AllowAnonymous]
        public IActionResult AllProducts()
        {
            return Ok(Products);
        }

        [HttpPatch]
        [Route("funcionario/update/{index}")]
        [Authorize(Roles = "funcionario")]
        public IActionResult UpdateProduct([FromRoute] int index, [FromBody] Product product)
        {
            try
            {
                var existingProduct = Products[index];
                product.Id = Products[index].Id;
                Products[index] = product;
                return Ok(Products[index]);
            }
            catch
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var existingProduct = Products.Where(x => x.Id == id);

            try
            {
                if (existingProduct.Any())
                {
                    return Ok(existingProduct.FirstOrDefault());
                }
                else
                {
                    return StatusCode(404, "Produto não cadastrado.");
                }
            }
            catch
            {
                return StatusCode(500, "Erro.");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetProductByName([FromQuery] string name)
        {
            var existingProduct = Products.Where(x => x.Name == name);

            try
            {
                if (existingProduct.Any())
                {
                    return Ok(existingProduct.FirstOrDefault());
                }
                else
                {
                    return StatusCode(404, "Produto não cadastrado.");
                }
            }
            catch
            {
                return StatusCode(500, "Erro.");
            }
        }

        [HttpDelete]
        [Route("funcionario/delete/{index}")]
        [Authorize(Roles = "funcionario")]
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
