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
using System.Text.Json;
using System.Text.Json.Serialization;
using LetsShop.TransactModels;

namespace LetsShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static List<ProductInCart> ProductsInCart = new List<ProductInCart>();
        private static int CurrentId = 0;

        public object JsonSerialize { get; private set; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetProductsInCart()
        {
            return Ok(ProductsInCart);
        }

        [HttpPost]
        [Route("add")]
        [AllowAnonymous]
        public IActionResult AddProduct([FromBody] ProductInCart productInCart)
        {

            //verifica se existe no carrinho
            var existingProductInCart = ProductsInCart.Where(x => x.Id == productInCart.Id);

            //verifica se o produto realmente existe
            var existingProduct = ProductsController.Products.Where(x => x.Id == productInCart.ProductId);

            if (existingProduct.Any())
            {
                if (existingProductInCart.Any())
                {
                    try
                    {
                        existingProductInCart.FirstOrDefault().Quantity += productInCart.Quantity;
                        return Ok("Quantidade do item de Id " + productInCart.Id + " atualizada com sucesso. ");
                    }
                    catch
                    {
                        return StatusCode(404);
                    }
                }
                else
                {
                    try
                    {
                        productInCart.Id = CurrentId++;
                        ProductsInCart.Add(productInCart);
                        return Ok("Produto de Id " + productInCart.ProductId + " adicionado ao carrinho como item de Id " + productInCart.Id);
                    }
                    catch
                    {
                        return StatusCode(404);
                    }
                    
                }
            } else
            {
                return StatusCode(501, "O produto de Id " + productInCart.ProductId + " não existe em nossa loja.");
            }

        }

        [HttpDelete]
        [Route("delete/{id}")]
        [AllowAnonymous]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            var productToRemove = ProductsInCart.Where(x => x.Id == id).FirstOrDefault();

            if (productToRemove != null)
            {
                try
                {
                    if (productToRemove.Quantity == 1)
                    {
                        ProductsInCart.Remove(productToRemove);
                        return Ok("O item de Id " + productToRemove.Id + " foi deletado do carrinho.");
                    }
                    else
                    {
                        productToRemove.Quantity--;
                        return Ok("Uma unidade do item de Id " + productToRemove.Id + " foi deletada do carrinho.");
                    }
                }
                catch
                {
                    return StatusCode(500, "Erro.");
                }
            }
            else
            {
                return StatusCode(404, "Não existe a Id " + id + " no carrinho atual.");
            }
        }

        [HttpDelete]
        [Route("empty")]
        [AllowAnonymous]
        public IActionResult EmptyCart()
        {
            try
            {
                if (ProductsInCart.Any())
                {
                    ProductsInCart.Clear();
                    return Ok("Seu carrinho foi esvaziado.");
                }
                else
                {
                    return Ok("Seu carrinho está vazio.");
                }
            }
            catch
            {
                return StatusCode(500, "Erro.");
            }
        }

        [HttpGet]
        [Route("cliente/checkout")]
        [Authorize(Roles = "cliente")]
        public IActionResult Checkout()
        {
            double amountDue = 0;

            foreach (ProductInCart product in ProductsInCart)
            {
                amountDue += ((product.Quantity) * (ProductsController.Products.Where(x => x.Id == product.ProductId).FirstOrDefault().Price));
            }

            CheckoutTransactModel checkout = new();

            checkout.productsInCart = ProductsInCart;
            checkout.totalValue = amountDue;

            return Ok(checkout);
        }
    }
}
