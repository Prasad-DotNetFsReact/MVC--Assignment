


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Custom Middleware 1: Terminate chain when URL contains "/end"
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Contains("/end"))
    {
        await context.Response.WriteAsync("Request Ended.");
        return;
    }
    await next();
});

// Custom Middleware 2: Display "Hello1" and "Hello2"
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello1 ");
    await next();
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello2 ");
    await next();
});

// Custom Middleware 3: Display "Hello" when URL contains "hello" and move to next middleware
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Contains("hello"))
    {
        await context.Response.WriteAsync("Hello ");
    }
    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

