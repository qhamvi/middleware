using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class FirstMiddleware
{
    //RequestDelegate tham chieu thi hanh middleware ~ async 
    private readonly RequestDelegate _next;
    public FirstMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    //HttpContext di qua middleware di trong pipeline
    //Nvu cua Middleware la xu li login trong method InvokeAsync
    //build chuc nang in re log cai thong tin : Dia chi request gui den server la gi
    //Sau khi xu li viec do xong -> day httpcontext cho cac middleware phia sau no xu li tiep
    public async Task InvokeAsync(HttpContext context)
    {
        // System.Console.WriteLine(context.Request.Path);
        System.Console.WriteLine("This is first middleware");
        System.Console.WriteLine($"URL {context.Request.Path}");
        //De truyen du lieu giua cac middleware => Dung thuoc tinh
        context.Items.Add("DataFirstMiddlewarer", $"<p>URL = {context.Request.Path}</p>");
        //await context.Response.WriteAsync($"<p>URL = {context.Request.Path}</p>");//co the phat sinh loi neu Header truoc 
        // chuyen context cho cac midddleware phia sau 
        await _next(context);
    }
    // Gio muon dang ki class nay vao pipeline cua luong xu li thi vao method Configure của Startup.cs
}