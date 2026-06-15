using Microsoft.EntityFrameworkCore;
using MobiGate.Infrastructure;
using MobiGate.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

// Infrastructure (EF Core + DbContext)
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers + API Explorer + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Problem Details (RFC 7807)
builder.Services.AddProblemDetails();

var app = builder.Build();

// Auto-migrate + seed on startup (dev convenience)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MobiGateDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.InitializeAsync(db);
}

// Exception → ProblemDetails
app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
