using Prepper;
using Prepper.Repositories;
using DotNetEnv;
using Supabase;
using Prepper.Models;
var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

bool useSupabaseDB = true;

if (useSupabaseDB)
{
    var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
    var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

    // Initialize Supabase client
    var options = new SupabaseOptions
    {
        AutoConnectRealtime = true,
        AutoRefreshToken = true
    };

    // Register the Client as a Singleton
    builder.Services.AddSingleton(provider => new Supabase.Client(url!, key!, options));
    
    // Register the repository
    builder.Services.AddScoped<IRepositoryDB<Ingredient>, IngredientDBRepo>();
    builder.Services.AddScoped<IRepositoryDB<NutritionalProfile>, NutritionalProfileDBRepo>();
}
else
{
    builder.Services.AddSingleton<IRepository<Ingredient>, IngrediantRepo>();
}



    // Add services to the container.

    builder.Services.AddControllers();
// Learn more about configuring OpenAPI at htt)ps://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();