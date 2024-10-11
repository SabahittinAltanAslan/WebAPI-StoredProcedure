using CategoryService.API.Data.Repositories;
using CategoryService.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryRepository _categoryRepository;

    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _categoryRepository.GetCategories();
        return Ok(categories);
    }

    [HttpPost]
    public ActionResult Add([FromBody] Category category)
    {
        _categoryRepository.AddCategory(category.Name);
        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Category category)
    {
        _categoryRepository.UpdateCategory(id, category.Name);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _categoryRepository.DeleteCategory(id);
        return NoContent();
    }
}
