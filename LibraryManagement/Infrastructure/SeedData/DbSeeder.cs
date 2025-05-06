using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.SeedData;

public static class DbSeeder
{
    public static void Seed(LibraryDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "Computer Science" },
                new() { Id = Guid.NewGuid(), Name = "Mathematics" },
                new() { Id = Guid.NewGuid(), Name = "Literature" },
                new() { Id = Guid.NewGuid(), Name = "History" },
                new() { Id = Guid.NewGuid(), Name = "Science Fiction" }
            };

            context.Categories.AddRange(categories);

            // Seed 20 books distributed among the 5 categories
            var books = new List<Book>
            {
                new() { Id = Guid.NewGuid(), Title = "Clean Code", Author = "Robert C. Martin", Description = "A Handbook of Agile Software Craftsmanship", Quantity = 5, CategoryId = categories[0].Id },
                new() { Id = Guid.NewGuid(), Title = "Design Patterns", Author = "Erich Gamma", Description = "Elements of Reusable Object-Oriented Software", Quantity = 4, CategoryId = categories[0].Id },
                new() { Id = Guid.NewGuid(), Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", Description = "CLRS classic", Quantity = 3, CategoryId = categories[0].Id },
                new() { Id = Guid.NewGuid(), Title = "Refactoring", Author = "Martin Fowler", Description = "Improving the Design of Existing Code", Quantity = 2, CategoryId = categories[0].Id },
                new() { Id = Guid.NewGuid(), Title = "Code Complete", Author = "Steve McConnell", Description = "A Practical Handbook of Software Construction", Quantity = 6, CategoryId = categories[0].Id },

                new() { Id = Guid.NewGuid(), Title = "Calculus", Author = "James Stewart", Description = "Early Transcendentals", Quantity = 5, CategoryId = categories[1].Id },
                new() { Id = Guid.NewGuid(), Title = "Linear Algebra Done Right", Author = "Sheldon Axler", Description = "Modern take on linear algebra", Quantity = 3, CategoryId = categories[1].Id },
                new() { Id = Guid.NewGuid(), Title = "Discrete Mathematics", Author = "Kenneth H. Rosen", Description = "Math foundations for CS", Quantity = 2, CategoryId = categories[1].Id },
                new() { Id = Guid.NewGuid(), Title = "Principles of Mathematical Analysis", Author = "Walter Rudin", Description = "Aka baby Rudin", Quantity = 4, CategoryId = categories[1].Id },
                new() { Id = Guid.NewGuid(), Title = "Concrete Mathematics", Author = "Donald Knuth", Description = "Foundation for the art of programming", Quantity = 2, CategoryId = categories[1].Id },

                new() { Id = Guid.NewGuid(), Title = "Pride and Prejudice", Author = "Jane Austen", Description = "Classic novel of manners", Quantity = 4, CategoryId = categories[2].Id },
                new() { Id = Guid.NewGuid(), Title = "Hamlet", Author = "William Shakespeare", Description = "Tragedy by Shakespeare", Quantity = 2, CategoryId = categories[2].Id },
                new() { Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Description = "Roaring 20s story", Quantity = 3, CategoryId = categories[2].Id },
                new() { Id = Guid.NewGuid(), Title = "Moby Dick", Author = "Herman Melville", Description = "The great white whale", Quantity = 1, CategoryId = categories[2].Id },
                new() { Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Author = "Harper Lee", Description = "Classic on racial injustice", Quantity = 5, CategoryId = categories[2].Id },

                new() { Id = Guid.NewGuid(), Title = "Sapiens", Author = "Yuval Noah Harari", Description = "A Brief History of Humankind", Quantity = 6, CategoryId = categories[3].Id },
                new() { Id = Guid.NewGuid(), Title = "Guns, Germs, and Steel", Author = "Jared Diamond", Description = "The Fates of Human Societies", Quantity = 4, CategoryId = categories[3].Id },
                new() { Id = Guid.NewGuid(), Title = "The Silk Roads", Author = "Peter Frankopan", Description = "A New History of the World", Quantity = 3, CategoryId = categories[3].Id },
                new() { Id = Guid.NewGuid(), Title = "The History of the Ancient World", Author = "Susan Wise Bauer", Description = "From the Earliest Accounts to the Fall of Rome", Quantity = 2, CategoryId = categories[3].Id },
                new() { Id = Guid.NewGuid(), Title = "A People's History of the United States", Author = "Howard Zinn", Description = "Told from the perspective of common people", Quantity = 3, CategoryId = categories[3].Id }
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}