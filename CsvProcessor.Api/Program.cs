using CsvProcessor.Api.Extensions;
using CsvProcessor.Api.Middlewares;
using CsvProcessor.Shared.Settings;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

# region [Options Configurations]

builder.Services.Configure<AwsSettings>(
    builder.Configuration.GetSection(AwsSettings.SectionName));
builder.Services.Configure<SqsSettings>(
    builder.Configuration.GetSection(SqsSettings.SectionName));
builder.Services.Configure<S3Settings>(
    builder.Configuration.GetSection(S3Settings.SectionName));

#endregion

// AWS S3 pointing to RustFS
builder.Services.AddS3Service();

// AWS SQS pointing to ElasticMQ
builder.Services.AddSqsService();

builder.Services.AddServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("CSV Processor API");
        options.DisableAgent();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();