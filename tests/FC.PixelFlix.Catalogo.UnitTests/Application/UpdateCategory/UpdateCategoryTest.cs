
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Moq;
using Xunit;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FC.Pixelflix.Catalogo.Application.Exceptions;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestCollection))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory))]
    [Trait("Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), 
        parameters:10, 
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
    public async Task GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory(Category aCategory, UpdateCategoryRequest request)
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
        var aUnitOfWork = _fixture.GetMockUnitOfWork();
        var cancellationToken = It.IsAny<CancellationToken>();

        aRepository.Setup(category => category.Get(aCategory.Id, cancellationToken))
            .ReturnsAsync(aCategory);

        var useCase = new UseCase.UpdateCategory(aRepository.Object, aUnitOfWork.Object);

        //when

        CategoryModelResponse response = await useCase.Handle(request, CancellationToken.None);

        //then

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);

        aRepository.Verify(category => category.Get(aCategory.Id, It.IsAny<CancellationToken>()), 
            Times.Once);

        aRepository.Verify(category => category.Update(aCategory, It.IsAny<CancellationToken>()),
            Times.Once);

        aUnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(GivenAInvalidId_whenCallsUpdateCategory_shouldReturnNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task GivenAInvalidId_whenCallsUpdateCategory_shouldReturnNotFound()
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
        var aUnitOfWork = _fixture.GetMockUnitOfWork();
        var aRequest = _fixture.GetValidRequest();
        var cancellationToken = It.IsAny<CancellationToken>();

        aRepository.Setup(category => category.Get(aRequest.Id, cancellationToken))
            .ThrowsAsync(new NotFoundException($"category '{aRequest.Id}' was not found"));

        var useCase = new UseCase.UpdateCategory(aRepository.Object, aUnitOfWork.Object);

        //when
        var aTask = async () => await useCase.Handle(aRequest, CancellationToken.None);

        //then

        await aTask.Should().ThrowAsync<NotFoundException>();
        aRepository.Verify(category => category.Get(aRequest.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategoryWithoutIsActive_shouldReturnACategory))]
    [Trait("Application", "UpdateCategory - UseCases")]
    [MemberData(
    nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
    parameters: 10,
    MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task GivenAValidId_whenCallsUpdateCategoryWithoutIsActive_shouldReturnACategory(Category aCategory, UpdateCategoryRequest request)
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
        var aUnitOfWork = _fixture.GetMockUnitOfWork();
        var aRequestWithoutIsActive = new UpdateCategoryRequest(request.Id, request.Name, request.Description);
        var cancellationToken = It.IsAny<CancellationToken>();

        aRepository.Setup(category => category.Get(aCategory.Id, cancellationToken))
            .ReturnsAsync(aCategory);

        var useCase = new UseCase.UpdateCategory(aRepository.Object, aUnitOfWork.Object);

        //when

        CategoryModelResponse response = await useCase.Handle(request, CancellationToken.None);

        //then

        response.Should().NotBeNull();
        response.Name.Should().Be(aRequestWithoutIsActive.Name);
        response.Description.Should().Be(aRequestWithoutIsActive.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);

        aRepository.Verify(category => category.Get(aCategory.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        aRepository.Verify(category => category.Update(aCategory, It.IsAny<CancellationToken>()),
            Times.Once);

        aUnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategoryWithOnlyName_shouldReturnACategory))]
    [Trait("Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
    public async Task GivenAValidId_whenCallsUpdateCategoryWithOnlyName_shouldReturnACategory(Category aCategory, UpdateCategoryRequest request)
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
        var aUnitOfWork = _fixture.GetMockUnitOfWork();
        var aRequestWithName = new UpdateCategoryRequest(request.Id, request.Name);
        var cancellationToken = It.IsAny<CancellationToken>();

        aRepository.Setup(category => category.Get(aCategory.Id, cancellationToken))
            .ReturnsAsync(aCategory);

        var useCase = new UseCase.UpdateCategory(aRepository.Object, aUnitOfWork.Object);

        //when

        CategoryModelResponse response = await useCase.Handle(request, CancellationToken.None);

        //then

        response.Should().NotBeNull();
        response.Name.Should().Be(aRequestWithName.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);

        aRepository.Verify(category => category.Get(aCategory.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        aRepository.Verify(category => category.Update(aCategory, It.IsAny<CancellationToken>()),
            Times.Once);

        aUnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
