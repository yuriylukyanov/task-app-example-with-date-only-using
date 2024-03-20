using Implementations.RepositoriesEF;
using Microsoft.EntityFrameworkCore;
using MigrationProject;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<TaskAppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetValue<string>("taskAppConnectionString"), 
        npgsqlOptionsAction => npgsqlOptionsAction.MigrationsAssembly(typeof(Program).Assembly.FullName))
    );
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
