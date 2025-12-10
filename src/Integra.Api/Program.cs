using Integra.Application;
using Integra.Application.Abstractions.Messaging;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker_Application).Assembly);
});


builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>)
);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(TransactionBehavior<,>)
);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(LoggingBehavior<,>)
);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ExceptionHandlingBehavior<,>)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

