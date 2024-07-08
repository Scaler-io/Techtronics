using Serilog;
using Shared.DI;
using Shared.Logger;

var builder = WebApplication.CreateBuilder(args);

// add services
var logger = Logging.GetLogger(builder.Configuration, builder.Environment, "dummy-index");

builder.Services.AddConfigureOptions(builder.Configuration);
builder.Host.UseSerilog(logger);

var app = builder.Build();

app.Run();
