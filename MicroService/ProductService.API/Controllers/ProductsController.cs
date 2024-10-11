using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Repositories;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductRepository _productRepository;

    public ProductsController(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _productRepository.GetProducts();
        return Ok(products);
    }

    [HttpPost]
    public ActionResult Add([FromBody] Product product)
    {
        _productRepository.AddProduct(product.Name, product.Price, product.CategoryId);
        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Product product)
    {
        _productRepository.UpdateProduct(id, product.Name, product.Price, product.CategoryId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _productRepository.DeleteProduct(id);
        return NoContent();
    }
}
