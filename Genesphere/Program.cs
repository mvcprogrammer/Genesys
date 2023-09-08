using System.Diagnostics;
using System.Reflection;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud;
using GenesysCloud.Services.PureCloud.Fundamental;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var environment = builder.Environment;
if (environment.IsDevelopment())
{
    //builder.Services.AddTransient<IUsersQueryHandlers, MockUsersQueryHandlers>();
    builder.Services.AddTransient<IUsersQueryHandlers, PureCloudUsersQueryHandlers>();
    builder.Services.AddTransient<IUsersService, PureCloudUsersService>();
}
else
{
    Debug.Assert(false); // shouldn't ever happen, but just in case.
    //builder.Services.AddTransient<IUsersQueryHandlers, PureCloudUsersQueryHandlers>();
    builder.Services.AddTransient<IUsersQueryHandlers, MockUsersQueryHandlers>();
    builder.Services.AddTransient<IUsersService, PureCloudUsersService>();
}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Genesphere", Version = "v1" });
    c.OperationFilter<AddCustomParameter>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();