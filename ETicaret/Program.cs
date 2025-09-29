using Blazored.LocalStorage;
using Blazored.Toast;
using ETicaret_UI.Auth;
using ETicaret_UI.Components;
using ETicaret_UI.Services;
using ETicaret_UI.Settings;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddHttpContextAccessor();

builder.Services.AddServerSideBlazor()
       .AddCircuitOptions(options =>
       {
           options.DetailedErrors = true;
       });

// Session ve cookie tabanlý authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/login"; // Login sayfasý
        options.AccessDeniedPath = "/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS ise zorunlu
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton<ApiSettings>();
builder.Services.AddHttpClient<RequestManager>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
//    provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddAuthorizationCore();

// HttpClient'i service olarak ekle
builder.Services.AddHttpClient();


// Session ve Cache ekle
builder.Services.AddDistributedMemoryCache(); // <-- Bu eksikti
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddBlazoredToast();
builder.Services.AddScoped<CartService>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.MapPost("/upload", async (HttpRequest request, IWebHostEnvironment env) =>
{
    var form = await request.ReadFormAsync();
    var file = form.Files.FirstOrDefault();
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("Dosya seçilmedi");
    }

    var uploads = Path.Combine(env.WebRootPath, "uploads");
    if (!Directory.Exists(uploads))
    {
        Directory.CreateDirectory(uploads);
    }

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(uploads, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var url = $"/uploads/{fileName}";
    return Results.Ok(url);
});

app.MapGet("/uploads/list", () =>
{
    var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
    if (!Directory.Exists(uploadsPath))
        return new List<string>();

    var files = Directory.GetFiles(uploadsPath)
                         .Select(f => "/uploads/" + Path.GetFileName(f))
                         .ToList();

    return files;
});



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
