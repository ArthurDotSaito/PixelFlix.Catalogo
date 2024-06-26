﻿using System.Net;
using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FC.Pixelflix.Catalogo.Api.Filter;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;
    
    public ApiGlobalExceptionFilter(IHostEnvironment env)
    {
        _env = env;
    }
    
    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var exception = context.Exception;
        
        if(_env.IsDevelopment())
            details.Extensions.Add("StackTrace", exception.StackTrace);

        if (exception is EntityValidationException)
        {
            var ex = exception as EntityValidationException;
            details.Title = "One or more validation errors occurred.";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnprocessableEntity";
            details.Detail = ex!.Message;
        }
        else if (exception is NotFoundException)
        {
            details.Title = "Not Found";
            details.Status = StatusCodes.Status404NotFound;
            details.Type = "NotFound";
            details.Detail = exception!.Message;   
        }
        else
        {
            details.Title = "An unexpected error ocurred.";
            details.Status = StatusCodes.Status400BadRequest;
            details.Type = "UnexpectedError";
            details.Detail = exception.Message; 
        }
        
        context.HttpContext.Response.StatusCode = details.Status.Value;
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
}