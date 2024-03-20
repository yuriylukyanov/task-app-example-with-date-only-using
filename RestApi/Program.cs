using DomainLogic.Repositories;
using DomainLogic.Services;
using Implementations.RepositoriesEF;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddTransient<TaskService>()
                .AddTransient<ITaskRepository, TaskRepository>()
                .AddDbContext<TaskAppDbContext>(options =>
                    options.UseNpgsql(
                        builder.Configuration.GetValue<string>("taskAppConnectionString"),
                        a => a.MigrationsAssembly(typeof(TaskAppDbContext).Assembly.FullName)));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestApi", Version = "v1" });
});

builder.Services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder =>
                    builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp v1");
});

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();