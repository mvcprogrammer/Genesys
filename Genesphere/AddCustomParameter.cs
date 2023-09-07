namespace Genesphere;

using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

internal class AddCustomParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        /*operation.Parameters.Add(new OpenApiParameter
        {
            Name = "MyCustomParameter",
            In = ParameterLocation.Query, // or ParameterLocation.Header, ParameterLocation.Path, etc.
            Description = "Custom parameter description",
            Required = false, // Set to true if this is a required parameter
            Schema = new OpenApiSchema { Type = "string" }
        });*/
    }
}