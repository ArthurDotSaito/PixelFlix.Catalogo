using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using DomainGenre = FC.Pixelflix.Catalogo.Domain.Entities.Genre;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.CreateGenre;

[Collection(nameof(CreateGenreTestFixture))]
public class CreateGenreTest
{
    private readonly CreateGenreTestFixture _fixture;

    public CreateGenreTest(CreateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateGenre_shouldReturnAGenre))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsCreateGenre_shouldReturnAGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var dateTimeBefore = DateTime.Now;
        var dateTimeAfterCommand = DateTime.Now.AddSeconds(1);
        
        var useCase = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);

        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);
        
        genreRepositoryMock.Verify(e => e.Insert(It.IsAny<DomainGenre>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(e => e.Commit(It.IsAny<CancellationToken>()), Times.Once);
        
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Categories.Should().HaveCount(0);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(null);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        (output.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (output.CreatedAt <= dateTimeAfterCommand).Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateGenreWithCategories_shouldReturnAGenre))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsCreateGenreWithCategories_shouldReturnAGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var input = _fixture.GetValidInputWithCategories();
        
        categoryRepositoryMock.Setup(x=> x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyList<Guid>)input.Categories!);
        
        var dateTimeBefore = DateTime.Now;
        var dateTimeAfterCommand = DateTime.Now.AddSeconds(1);
        
        var useCase = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);
        
        var output = await useCase.Handle(input, CancellationToken.None);
        
        genreRepositoryMock.Verify(e => e.Insert(It.IsAny<DomainGenre>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(e => e.Commit(It.IsAny<CancellationToken>()), Times.Once);
        
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Categories.Should().HaveCount(input.Categories?.Count ?? 0);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(null);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        (output.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (output.CreatedAt <= dateTimeAfterCommand).Should().BeTrue();
        
        input.Categories.ForEach(id => output.Categories.Should().Contain(id));
    }
    
    [Fact(DisplayName = nameof(GivenAValidCreateCommand_whenThereIsNoRelatedCategories_shouldThrownNotFound))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task GivenAValidCreateCommand_whenThereIsNoRelatedCategories_shouldThrownNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        
        var input = _fixture.GetValidInputWithCategories();
        var aGuid = input.Categories![^1];
        
        categoryRepositoryMock.Setup(x=> x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyList<Guid>)input.Categories.FindAll(id=> aGuid != id));
        
        var useCase = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);
        
        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related categories not found: {aGuid}");
        
        categoryRepositoryMock.Verify(x=> x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory(DisplayName = nameof(GivenAValidCreateCommand_whenNameIsInvalid_shouldThrownEntityValidationException))]
    [Trait("Application", "CreateGenre - Use Cases")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GivenAValidCreateCommand_whenNameIsInvalid_shouldThrownEntityValidationException(string name)
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        
        var input = _fixture.GetValidInput(name);
        
        var useCase = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);
        
        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>().WithMessage($"Name should not be empty or null");
    }
}