using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using simple_crud.Controllers;
using simple_crud.Data;
using simple_crud.Data.Entities;
using simple_crud.DTO;

namespace Tests.Controllers
{
    // TODO add cases for cancellation handling
    public sealed class NoveltyControllerTests
    {
        private ILogger<NoveltyController> _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<NoveltyController>>();
        }

        [Test]
        public async Task Get_ShouldReturnOk_IfRepositoryReturnsEntity()
        {
            // Arrange
            var novelty = Substitute.For<INovelty>();
            var repository = Substitute.For<INoveltyRepository>();
            repository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult(novelty));
            var controller = new NoveltyController(_logger, repository);

            // Act
            var result = await controller.Get(1, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var typedResult = (OkObjectResult) result;
            Assert.That(typedResult.Value, Is.EqualTo(novelty));
        }

        [Test]
        public async Task Get_ShouldReturnNotFound_IfRepositoryReturnsNull()
        {
            // Arrange
            var repository = Substitute.For<INoveltyRepository>();
            repository.GetAsync(1, CancellationToken.None).ReturnsForAnyArgs(Task.FromResult<INovelty>(null));
            var controller = new NoveltyController(_logger, repository);

            // Act
            var result = await controller.Get(1, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Combinatorial]
        public async Task Add_ShouldReturnNotFound_IfProvidedInvalidDto([Values("", null, "valid")] string name, [Values("", null, "valid")] string description)
        {
            if (name?.Equals("valid", StringComparison.Ordinal) == true && description?.Equals("valid", StringComparison.Ordinal) == true)
                return;

            // Arrange
            var repository = Substitute.For<INoveltyRepository>();
            var controller = new NoveltyController(_logger, repository);
            var dto = new NoveltyToAddDto {Description = description, Name = name};

            // Act
            var result = await controller.Add(dto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Add_ShouldReturnCreated_IfRepositoryReturnsEntity()
        {
            // Arrange
            var novelty = Substitute.For<INovelty>();
            var repositoryReturn = Task.FromResult(AddOrUpdateResult<INovelty>.Success(novelty));
            var repository = Substitute.For<INoveltyRepository>();
            repository.TryAdd(Arg.Is<NoveltyToAdd>(x => x.Name.Equals("name", StringComparison.Ordinal) && x.Description.Equals("des", StringComparison.Ordinal)), CancellationToken.None).Returns(repositoryReturn);
            var controller = new NoveltyController(_logger, repository);
            var dto = new NoveltyToAddDto {Name = "name", Description = "des"};

            // Act
            var result = await controller.Add(dto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task Add_ShouldReturnBadRequest_IfRepositoryReturnsFailure()
        {
            // Arrange
            var repositoryReturn = Task.FromResult(AddOrUpdateResult<INovelty>.Failure());
            var repository = Substitute.For<INoveltyRepository>();
            repository.TryAdd(Arg.Is<NoveltyToAdd>(x => x.Name.Equals("name", StringComparison.Ordinal) && x.Description.Equals("des", StringComparison.Ordinal)), CancellationToken.None).Returns(repositoryReturn);
            var controller = new NoveltyController(_logger, repository);
            var dto = new NoveltyToAddDto { Name = "name", Description = "des" };

            // Act
            var result = await controller.Add(dto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Modify_ShouldReturnOk_IfRepositoryReturnsEntity()
        {
            // Arrange
            var novelty = Substitute.For<INovelty>();
            var repositoryReturn = Task.FromResult(AddOrUpdateResult<INovelty>.Success(novelty));
            var repository = Substitute.For<INoveltyRepository>();
            repository.TryUpdate(Arg.Is<NoveltyToAdd>(x => x.Name.Equals("name", StringComparison.Ordinal) && x.Description.Equals("des", StringComparison.Ordinal) && x.ID == 2), CancellationToken.None).Returns(repositoryReturn);
            var controller = new NoveltyController(_logger, repository);
            var dto = new NoveltyToAddDto { Name = "name", Description = "des" };

            // Act
            var result = await controller.Modify(2, dto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task Modify_ShouldReturnBadRequest_IfRepositoryReturnsFailure()
        {
            // Arrange
            var repositoryReturn = Task.FromResult(AddOrUpdateResult<INovelty>.Failure());
            var repository = Substitute.For<INoveltyRepository>();
            repository.TryUpdate(Arg.Is<NoveltyToAdd>(x => x.Name.Equals("name", StringComparison.Ordinal) && x.Description.Equals("des", StringComparison.Ordinal) && x.ID == 3), CancellationToken.None).Returns(repositoryReturn);
            var controller = new NoveltyController(_logger, repository);
            var dto = new NoveltyToAddDto { Name = "name", Description = "des" };

            // Act
            var result = await controller.Modify(3, dto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Delete_ShouldReturnNoContent_IfRepositoryReturnsSuccess()
        {
            // Arrange
            var repository = Substitute.For<INoveltyRepository>();
            repository.TryRemove(4, CancellationToken.None).Returns(Task.FromResult(true));
            var controller = new NoveltyController(_logger, repository);

            // Act
            var result = await controller.Delete(4, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_IfRepositoryReturnsFailure()
        {
            // Arrange
            var repository = Substitute.For<INoveltyRepository>();
            repository.TryRemove(5, CancellationToken.None).Returns(Task.FromResult(false));
            var controller = new NoveltyController(_logger, repository);

            // Act
            var result = await controller.Delete(5, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
