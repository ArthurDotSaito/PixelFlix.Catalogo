﻿namespace FC.Pixelflix.Catalogo.Api.ApiModels.Response;

public class ApiResponseList<TItemData> : ApiResponse<IReadOnlyList<TItemData>>
{
    public ApiResponseListMeta Meta { get; private set; }
    public ApiResponseList(IReadOnlyList<TItemData> data, int currentPage, int perPage, int total) : base(data)
    {
        Meta = new ApiResponseListMeta(currentPage, perPage, total);
    }
}