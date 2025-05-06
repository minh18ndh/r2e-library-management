using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.Tests.Controllers;

[TestFixture]
public class CategoriesControllerTests
{
    private Mock<ICategoryService> _serviceMock = null!;
    private CategoriesController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ICategoryService>();
        _controller = new CategoriesController(_serviceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithCategories()
    {
        var categories = new List<CategoryResponseDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Tech" }
        };

        _serviceMock.Setup(s => s.GetAllAsync(null)).ReturnsAsync(categories);

        var result = await _controller.GetAll(null) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(categories));
    }

    [Test]
    public async Task GetById_ReturnsOkWithCategory()
    {
        var id = Guid.NewGuid();
        var category = new CategoryResponseDto { Id = id, Name = "Science" };

        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(category);

        var result = await _controller.GetById(id) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(category));
    }

    [Test]
    public async Task Update_ValidDto_ReturnsOk()
    {
        var id = Guid.NewGuid();
        var dto = new CategoryUpdateRequestDto { Name = "Updated" };
        var updated = new CategoryResponseDto { Id = id, Name = "Updated" };

        _serviceMock.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(updated);

        var result = await _controller.Update(id, dto) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(updated));
    }

    [Test]
    public async Task Delete_ReturnsOkWithMessage()
    {
        var id = Guid.NewGuid();

        _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var result = await _controller.Delete(id) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value!.ToString(), Does.Contain("deleted"));
    }
}