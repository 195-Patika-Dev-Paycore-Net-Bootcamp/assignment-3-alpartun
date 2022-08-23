using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using payCoreHW3.Context;
using payCoreHW3.Mapping;
using payCoreHW3.Middleware;
using payCoreHW3.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapper = new ModelMapper();
//var xTypes = new[] {typeof(VehicleMap).Assembly.ExportedTypes,typeof(ContainerMap).Assembly.ExportedTypes };
mapper.AddMappings(typeof(AllMap).Assembly.ExportedTypes);
HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

var cfg = new Configuration();
cfg.DataBaseIntegration(c =>
{
    c.Dialect<PostgreSQLDialect>();
    c.ConnectionString = builder.Configuration.GetConnectionString("PostgreSQLConnectionString");
    c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
    c.SchemaAction = SchemaAutoAction.Update;
    c.LogFormattedSql = true;
    c.LogSqlInConsole = true;
});
cfg.AddMapping(domainMapping);
var sessionFactory = cfg.BuildSessionFactory();

builder.Services.AddSingleton(sessionFactory);
builder.Services.AddScoped(factory => sessionFactory.OpenSession());
builder.Services.AddScoped<IMapperSession, MapperSession>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

