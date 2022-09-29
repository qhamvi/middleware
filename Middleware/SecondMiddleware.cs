using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class SecondMiddleware : IMiddleware
{
    /**
    Chuc nang: Khi HttpContext di qua Middleware nay : 
    no se kiem tra cai 
    + Url : /xxx.html
    -> Khong goi middleware phia sau
    -> user ko dc truy cap 
    -> Tao ra Header : SecondMiddleware co content : Ban ko duoc truy cap
    + Url != /xxx.html
    -> Tao ra Header : SecondMiddleware co content : Ban duoc truy cap
    -> Thuc hien chuyen HttpContext ngay Middleware phia sau
    */
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        System.Console.WriteLine("This is second middleware");
        if(context.Request.Path == "/xxx.html")
        {
            
            // Phai thiet lap Header truoc khi thiet lap Context
            context.Response.Headers.Add("SecondMiddleware", "Ban khong duoc truy cap"); 
            var dataFromFirstMiddleware = context.Items["DataFirstMiddlewarer"];
            if(dataFromFirstMiddleware != null)
            {
                await context.Response.WriteAsync((string)dataFromFirstMiddleware);
            }
            await context.Response.WriteAsync("Ban khong duoc truy cap");
            
        }
        else
        {
            context.Response.Headers.Add("SecondMiddleware", "Ban duoc truy cap");
            // Chuyen httpcontext cho Middleware phia sau
            await next(context);
        }
    }
}
// Doi voi loai Middleware dc impliment tu IMiddleware -> dki cho no la mot dich vu ung dung truoc
// => Vao Startup.cs 