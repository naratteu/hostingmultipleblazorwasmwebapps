using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

app.HostSubWasm("blazorwasm").HostSubWasm("fluentblazorwasm");
app.UseStaticFiles(options: new()
{
    RequestPath = $"/_content",
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "_content")),
    ServeUnknownFileTypes = true
});

app.MapGet("/", () => Results.Text("""
<h1>hello world</h1>
<a href="/blazorwasm">blazorwasm</a><br/>
<a href="/fluentblazorwasm">fluentblazorwasm</a>
""", "text/html"));

app.Run();

static class Exts
{
    public static WebApplication HostSubWasm(this WebApplication app, string path)
    {
        app.MapGroup($"/{path}").MapFallbackToFile($"/{path}/index.html");
        app.UseStaticFiles(options: new()
        {
            RequestPath = $"/{path}",
            FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "wwwroot", path)),
            ServeUnknownFileTypes = true
        });
        app.UseStaticFiles(options: new()
        {
            RequestPath = $"/{path}/_content",
            FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "_content")),
            ServeUnknownFileTypes = true
        });
        return app;
    }
}
