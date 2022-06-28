using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueaderoApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PARQUEADEROContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ParqueaderoContext"));
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("vehiculos/getAll", async (PARQUEADEROContext dbContext) =>
{
    var vehiculos = await dbContext.Estacionamientos.ToListAsync();
    return Results.Ok(vehiculos);
});

app.MapPost("vehiculos/create", async (Estacionamiento estacionamiento, PARQUEADEROContext dbContext) =>
{
    dbContext.Estacionamientos.Add(estacionamiento);
    await dbContext.SaveChangesAsync();
    return Results.Ok(estacionamiento);
});

app.MapPost("vehiculos/GetbyRangeTime", async ([FromBody] Estacionamiento estacionamiento, PARQUEADEROContext dbContext) =>
{
    var vehiculos = await dbContext.Estacionamientos.Where(x => x.Ingreso >= estacionamiento.Ingreso && x.Salida <= estacionamiento.Salida).ToListAsync();
    return Results.Ok(vehiculos);
});

app.MapPost("vehiculos/Liquidar", async ([FromBody] Estacionamiento estacionamiento, PARQUEADEROContext dbContext) =>
{
    var liquidacion = await (from vehiculo in dbContext.Estacionamientos
                             join
                             tipo in dbContext.TipoVehiculos on vehiculo.IdTipoVehiculo equals tipo.IdTipoVehiculo
                             select new
                             {
                                 vehiculo.Id,
                                 vehiculo.Placa,
                                 vehiculo.Ingreso,
                                 vehiculo.Salida,
                                 Tarifa = (estacionamiento.AplicaDescuento == true ? tipo.Tarifa * 0.3 : tipo.Tarifa)
                             }).Where(x => x.Placa == estacionamiento.Placa && x.Salida == null).FirstOrDefaultAsync();


    if (liquidacion == null)
    {
        return Results.NotFound();
    }
    var update = await dbContext.Estacionamientos.FindAsync(liquidacion.Id);
    
    if (update == null)
    {
        return Results.NotFound();
    }
    update.Salida = DateTime.Now;
    await dbContext.SaveChangesAsync();

    return Results.Ok(liquidacion);
});

app.Run();