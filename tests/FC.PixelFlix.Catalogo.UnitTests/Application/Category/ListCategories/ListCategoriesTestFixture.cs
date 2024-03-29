﻿using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using CategoryClass = FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.PixelFlix.Catalogo.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.ListCategories;


[CollectionDefinition(nameof(ListCategoriesTestCollection))]
public class ListCategoriesTestCollection : ICollectionFixture<ListCategoriesTestFixture> { }
public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
{

    public List<CategoryClass.Category> GetValidCategoryList(int length = 10)
    {
        var categoriesList = new List<CategoryClass.Category>();
        for (var i = 0; i < length; i++)
        {
            categoriesList.Add(GetValidCategory());
        }

        return categoriesList;

    }

    public ListCategoriesRequest GetValidRequest()
    {
        var random = new Random();

        return new ListCategoriesRequest(
            page: random.Next(1, 10),
            perPage: random.Next(1, 10),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(1, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
       );
    }
}
