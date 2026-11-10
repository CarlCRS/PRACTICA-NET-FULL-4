using Microsoft.Data.SqlClient;
using System.Data;
using WebAPI_Carlos_Chica_20261110.Repository;
using WebAPI_Carlos_Chica_20261110.Service;

var builder = WebApplication.CreateBuilder(args);

var conexion = builder.Configuration.GetConnectionString("Base_conexion") ?? throw new Exception("Error al conectar la base de datos");
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(conexion));

builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<FacturaRepository>();
builder.Services.AddScoped<PagoRepository>();

builder.Services.AddScoped<ServiceCliente>();
builder.Services.AddScoped<ServiceFactura>();
builder.Services.AddScoped<ServicePago>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("PermitirAngular");
app.MapControllers();

app.Run();
