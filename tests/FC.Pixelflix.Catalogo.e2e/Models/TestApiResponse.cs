using FC.Pixelflix.Catalogo.Api.ApiModels.Response;

namespace FC.Pixelflix.Catalogo.e2e.Models;

public class ApiResponseTest<TOutput>
{
    public TOutput? Data { get; set; }

    public ApiResponseTest() { }
    
    public ApiResponseTest(TOutput data)
    {
        Data = data;
    }
}

public class ApiResponseTestMeta
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }

    public ApiResponseTestMeta() { }

    public ApiResponseTestMeta(int currentPage, int perPage, int total)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
    }
}

public class ApiResponseListTest<TOutputItem> : ApiResponseTest<List<TOutputItem>>
{
    public ApiResponseListMeta? Meta { get; set; }

    public ApiResponseListTest() { }

    public ApiResponseListTest(List<TOutputItem> data) : base(data) { }
    
    public ApiResponseListTest(List<TOutputItem> data, ApiResponseListMeta meta) : base(data)
    {
        Meta = meta;
    }
}