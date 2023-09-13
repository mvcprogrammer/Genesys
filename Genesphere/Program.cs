using System.Reflection;
using GenesysCloud.Services.Contracts.Derived;
using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Derived;
using GenesysCloud.Services.PureCloud.Fundamental;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var environment = builder.Environment;
if (environment.IsDevelopment())
{
    builder.Services.AddTransient<IQualityQueryHandlers, PureCloudQualityQueryHandlers>();
    builder.Services.AddTransient<IQualityService, PureCloudQualityService>();
    builder.Services.AddTransient<IAnalyticsQueryHandlers, PureCloudAnalyticsQueryHandlers>();
    builder.Services.AddTransient<IAnalyticsService, PureCloudAnalyticsService>();
    builder.Services.AddTransient<IUsersQueryHandlers, PureCloudUsersQueryHandlers>();
    builder.Services.AddTransient<IUsersService, PureCloudUsersService>();
    builder.Services.AddTransient<IReportDataService, PureCloudReportDataService>();
}
else
{
    builder.Services.AddTransient<IQualityQueryHandlers, PureCloudQualityQueryHandlers>();
    builder.Services.AddTransient<IQualityService, PureCloudQualityService>();
    builder.Services.AddTransient<IAnalyticsQueryHandlers, PureCloudAnalyticsQueryHandlers>();
    builder.Services.AddTransient<IAnalyticsService, PureCloudAnalyticsService>();
    builder.Services.AddTransient<IUsersQueryHandlers, PureCloudUsersQueryHandlers>();
    builder.Services.AddTransient<IUsersService, PureCloudUsersService>();
    builder.Services.AddTransient<IReportDataService, PureCloudReportDataService>();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();