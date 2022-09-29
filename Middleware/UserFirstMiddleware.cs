using Microsoft.AspNetCore.Builder;

public static class UserFirstMiddlewareMethod
{
    //De thiet lap day la method mo rong cho IApplicationBuilder su dung tu khoa this
    public static void UserFirstMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<FirstMiddleware>();
    }

    // Gio ta mo rong phuong thuc tinh ra nua 
    public static void UserSecondMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<SecondMiddleware>();
    }
}