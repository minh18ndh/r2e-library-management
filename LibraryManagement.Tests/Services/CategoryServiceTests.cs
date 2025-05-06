using Moq;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Tests.Services
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _categoryRepoMock = null!;
        private Mock<IBookRepository> _bookRepoMock = null!;
        private CategoryService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _bookRepoMock = new Mock<IBookRepository>();
            _service = new CategoryService(_categoryRepoMock.Object, _bookRepoMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedCategoryDtos()
        {
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "Test Category" }
            };

            _categoryRepoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(categories);

            var result = await _service.GetAllAsync(null);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Test Category"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCategoryDto()
        {
            var id = Guid.NewGuid();
            var category = new Category { Id = id, Name = "Existing" };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);

            var result = await _service.GetByIdAsync(id);

            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo("Existing"));
        }

        [Test]
        public async Task CreateAsync_CreatesAndReturnsDto()
        {
            var dto = new CategoryCreateRequestDto { Name = "New Category" };
            var entity = new Category { Id = Guid.NewGuid(), Name = dto.Name };

            _categoryRepoMock.Setup(r => r.AddAsync(It.IsAny<Category>())).ReturnsAsync(entity);

            var result = await _service.CreateAsync(dto);

            Assert.That(result.Name, Is.EqualTo("New Category"));
        }

        [Test]
        public async Task UpdateAsync_Succeeds_ReturnsDto()
        {
            var id = Guid.NewGuid();
            var old = new Category { Id = id, Name = "Old" };
            var updated = new Category { Id = id, Name = "New" };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(old);
            _categoryRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(updated);

            var dto = new CategoryUpdateRequestDto { Name = "New" };

            var result = await _service.UpdateAsync(id, dto);

            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo("New"));
        }

        [Test]
        public async Task DeleteAsync_Succeeds()
        {
            var id = Guid.NewGuid();

            _categoryRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Category
            {
                Id = id,
                Name = "To Delete"
            });

            _bookRepoMock.Setup(r => r.GetByCategoryIdAsync(id)).ReturnsAsync(new List<Book>());
            _categoryRepoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            Assert.That(result, Is.True);
        }
    }
}