using System.Text;
using System.Text.Json;
using MekkysCakes.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace MekkysCakes.Presentation.Attributes
{
    internal class RedisCacheAttribute(int timeToLive = 10) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (HasSearchParameter(context))
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = CreateCacheKey(context);

            var cacheValue = await cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                var cachedResult = JsonSerializer.Deserialize<object>(cacheValue);

                context.Result = new OkObjectResult(cachedResult);
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okResult)
            {
                var serializedData = JsonSerializer.Serialize(okResult.Value);
                await cacheService.SetAsync(cacheKey, okResult.Value!, TimeSpan.FromMinutes(timeToLive));
            }
        }



        private string CreateCacheKey(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            var key = new StringBuilder();
            key.Append(request.Path);

            foreach (var argument in context.ActionArguments.OrderBy(a => a.Key))
            {
                if (argument.Value == null)
                    continue;

                key.Append($"|{argument.Key}-{Serialize(argument.Value)}");
            }

            return key.ToString();
        }

        private string Serialize(object value)
        {
            if (value is string || value.GetType().IsPrimitive || value is Enum)
                return value.ToString();

            return JsonSerializer.Serialize(value);
        }

        private bool HasSearchParameter(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg == null) continue;

                var prop = arg.GetType().GetProperty("Search");

                if (prop != null)
                {
                    var value = prop.GetValue(arg) as string;

                    if (!string.IsNullOrWhiteSpace(value))
                        return true;
                }
            }

            return false;
        }
    }
}
