using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Application.Common;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixtureCollection))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { };

public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{}
