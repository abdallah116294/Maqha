using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Maqha.MiddleWares
{
    public class SerilogMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SerilogMiddleWare(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task InvokeAsync(HttpContext context)
       {
            var sw = Stopwatch.StartNew();
            var request = context.Request;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            var claim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "_e")?.Value ?? "unknown";
            var userName = claim ?? "unknown";
            var requestPath = request.Path;

            using (LogContext.PushProperty("Username", userName))
            using (LogContext.PushProperty("RequestPath", requestPath))
            using (LogContext.PushProperty("RequestMethod", request.Method))
            using (LogContext.PushProperty("RequestIP", ipAddress))
            {
                Log.Information("Incoming request: {Method} {Path} - IP:{IP}", request.Method, request.Path, ipAddress);

                try
                {
                    await _next(context);
                    sw.Stop();
                    Log.Information("Answer: {Statuscode} - Sure: {Elapsed} ms", context.Response.StatusCode, sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    Log.Error(ex, "Error occurred: {message} - IP:{IP} - Certainly:{timed out}", ex.Message, ipAddress, sw.ElapsedMilliseconds);
                    throw;
                }
            }

        }

    }
}
