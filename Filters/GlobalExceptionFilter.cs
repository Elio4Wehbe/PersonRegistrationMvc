using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonRegistrationMvc.Filters
{

    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Items["UnhandledException"] = context.Exception;

            var referer = context.HttpContext.Request.Headers["Referer"].ToString();

            context.Result = new RedirectResult(string.IsNullOrEmpty(referer) ? "/Dashboard" : referer);


            var tempDataFactory = context.HttpContext.RequestServices
                .GetService(typeof(Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionaryFactory))
                as Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionaryFactory;

            if (tempDataFactory != null)
            {
                var tempData = tempDataFactory.GetTempData(context.HttpContext);
                tempData["ErrorMessage"] = "Something went wrong while processing your request: " + context.Exception.Message;
            }

            context.ExceptionHandled = true;
        }
    }
}