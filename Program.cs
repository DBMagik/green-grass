using GreenGrass.Data;
using GreenGrass.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Entity Framework
builder.Services.AddScoped<ApplicationDbContext>();

// Register Authentication Service
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IClientService, ClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<GreenGrass.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();