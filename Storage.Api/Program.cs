using Storage.Application;
using Storage.Domain.Repositories;
using Storage.Infrastructure.Interfaces;
using Storage.Infrastructure.Repositories;
using Storage.Infrastructure.StorageProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Read the type of storage from configuration
var typeOfStorage = builder.Configuration["TypeOfStorage"];
// Register the appropriate storage provider based on configuration
switch (typeOfStorage)
{
    case "AzureFileStorage":
        // Register the AzureBlobStorageProvider
        builder.Services.AddSingleton<IStorageProvider, AzureFileStorageProvider>();
        break;
    case "LocalStorage":
        // Register the LocalStorageProvider
        builder.Services.AddSingleton<IStorageProvider, LocalStorageProvider>();
        break;
    default:
        throw new Exception("Invalid storage provider type configured");
}



builder.Services.AddSingleton<IFileRepository, FileRepository>();
builder.Services.AddSingleton<IFileService, FileService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//  To Prevent Call  Microservice  exeption ApiGateway 

//var apiGatewayBaseUrl = builder.Configuration["ApiGatewayBaseUrl"]
//app.Use(async (context, next) =>
//{
//    // Check if the request is coming from the API Gateway
//    var referer = context.Request.Headers["Referer"].ToString();
//    if (string.IsNullOrEmpty(referer) || !referer.StartsWith(apiGatewayBaseUrl))
//    {
//        context.Response.StatusCode = StatusCodes.Status403Forbidden;
//        await context.Response.WriteAsync("Forbidden");
//        return;
//    }
//    await next.Invoke();
//});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
