using AspNetCore.Common.Data;
using AspNetCore.Common.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Common.Domain.Tests
{
    internal sealed class SchoolReadonlyRepositoryTests
    {
        private SchoolReadonlyRepository? repository;
        private AppDbContext context;

        [SetUp]
        public void Setup()
        {
            var databaseId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseId.ToString())
                .Options;

            context = new AppDbContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            context?.Dispose();
        }

        [Test]
        public async Task GetList()
        {
            // Arrange
            repository = new SchoolReadonlyRepository(context);

            var expected = new List<School>
            {
                new School
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                },
                new School
                {
                    Id = Guid.NewGuid(),
                    Name = "Test2",
                }
            };

            context.AddRange(expected);

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetListAsync(0, 10, null, null, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalCount, Is.EqualTo(2));

            result.Items.Should().BeEquivalentTo(expected);
        }
    }
}
