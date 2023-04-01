using DMS.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Sql Server
//services.AddTransient<IDatabaseContext, DatabaseContext>();

string connectionString = builder.Configuration["ConnectionStrings:SqlServer"];
builder.Services.AddDbContext<IdentityDatabaseContext>(option => option.UseSqlServer(connectionString));
//services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(connectionString));

//services.AddDistributedSqlServerCache(option =>
//{
//    option.ConnectionString = connectionString;
//    option.SchemaName = "dbo";
//    option.TableName = "CacheData";
//});
#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
