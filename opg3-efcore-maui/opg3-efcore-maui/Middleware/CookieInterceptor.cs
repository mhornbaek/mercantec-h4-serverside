namespace opg3_efcore_maui.Middleware
{
    public class CookieInterceptor
    {
        private readonly RequestDelegate _next;

        public CookieInterceptor(IConfiguration configuration, RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authCookieName = "authToken";

            var cookie = context.Request.Cookies[authCookieName];
            if (cookie != null && !string.IsNullOrEmpty(cookie))
            {
                var headerValue = "Bearer " + cookie;

                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers["Authorization"] = headerValue;
                }
                else
                {
                    context.Request.Headers.Append("Authorization", headerValue);
                }

                await _next.Invoke(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }

}
