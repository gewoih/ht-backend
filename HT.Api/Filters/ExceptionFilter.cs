using HT.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HT.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case NotFoundException nf:
            context.Result = new NotFoundObjectResult(new { error = nf.Message });
            context.ExceptionHandled = true;
            break;
        }
    }
}