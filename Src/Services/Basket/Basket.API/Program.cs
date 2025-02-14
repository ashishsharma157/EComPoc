using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//Add services to container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<BasketRepository>();
//    return new CacheBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
//});


var app = builder.Build();

//configure the HTTP request pipeline

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",new HealthCheckOptions
{
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
