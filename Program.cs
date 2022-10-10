using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectef;
using projectef.Models;

var builder = WebApplication.CreateBuilder(args);
//Configuracion para crear una bd en memoria
//builder.Services.AddDbContext<TareasContext>(opt => opt.UseInMemoryDatabase("TareasDB"));
//                               String de conexion: nombre servidor
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
// crear base de datos 
app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
});
//obtiene las tareas correspondientes
app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) =>
{
    //return Results.Ok(dbContext.Tareas.Include(p=>p.Categoria).Where(p=>p.PrioridadTarea==Prioridad.Baja));
     return Results.Ok(dbContext.Tareas.Include(p=>p.Categoria));
});
// Crea las tareas correspondientes
app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext,[FromBody] Tarea tarea) =>
{
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await dbContext.AddAsync(tarea);
    //await dbContext.Tareas.AddAsync(tarea); 2da manera de hacerlo
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});
//Actualiza las tareas que son correspondientes
app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext,[FromBody] Tarea tarea,[FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);
    if(tareaActual != null){
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo=tarea.Titulo;
        tareaActual.PrioridadTarea=tarea.PrioridadTarea;
        tareaActual.Descripcion=tarea.Descripcion;
        await dbContext.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});
//Elimina las tareas correspondientes
app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext,[FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);
    if(tareaActual != null){
        dbContext.Remove(tareaActual);
        await dbContext.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound();
});
app.Run();
