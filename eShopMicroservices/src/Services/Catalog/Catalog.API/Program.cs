var builder = WebApplication.CreateBuilder(args);

// Add serives to the container
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.Run();
