using System.Net;

namespace myApi.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private ILogger<ExceptionHandleMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandleMiddleware(ILogger <ExceptionHandleMiddleware> logger,RequestDelegate next) 
        {
            this.logger = logger;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);

            }
            catch (Exception ex) 

            {
                var errorId = Guid.NewGuid();
                logger.LogError(ex,$"{errorId}:{ex.Message}");
                httpContext.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong we are lokking into resolving this"
                };

                 await httpContext.Response.WriteAsJsonAsync(error);
            }

        }
    }
}
