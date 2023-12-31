using Amazon.S3;
using AWS_Project_Study.Models.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var s3BucketConfigs = builder.Configuration.GetSection("s3BucketConfigs").Get<S3BucketConfigs>();
var s3Configs = builder.Configuration.GetSection("s3Configs").Get<S3Configs>();

if (s3BucketConfigs is null || s3Configs is null)
    throw new ArgumentNullException($"{nameof(s3BucketConfigs)} or {nameof(s3Configs)} was not found.");

builder.Services.AddSingleton(s3BucketConfigs);
builder.Services.AddSingleton(s3Configs);

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.Configure<AmazonS3Config>(options =>
{
    options.ServiceURL = s3Configs?.ServiceURL;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
