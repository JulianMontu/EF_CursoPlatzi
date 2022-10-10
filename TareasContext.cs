using Microsoft.EntityFrameworkCore;
using projectef.Models;

namespace projectef;
public class TareasContext : DbContext
{
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public TareasContext(DbContextOptions<TareasContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria(){CategoriaId=Guid.Parse("e17ccf1e-1f91-42ab-bd2f-26759ccfd10f"),Nombre="Actividades pendientes",Peso=20});
        categoriasInit.Add(new Categoria(){CategoriaId=Guid.Parse("e17ccf1e-1f91-42ab-bd2f-26759ccfd20f"),Nombre="Actividades Personales",Peso=50});
        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categoria");
            categoria.HasKey(p => p.CategoriaId);
            categoria.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            categoria.Property(p => p.Descripcion).IsRequired(false);
            categoria.Property(p=>p.Peso);
            categoria.HasData(categoriasInit);

        });
        List<Tarea> tareasInit=new List<Tarea>();
        tareasInit.Add(new Tarea(){TareaId=Guid.Parse("e17ccf1e-1f91-42ab-bd2f-26759ccfd12f"),CategoriaId=Guid.Parse("e17ccf1e-1f91-42ab-bd2f-26759ccfd10f"),PrioridadTarea=Prioridad.Media, Titulo="Pago de servicios publicos",FechaCreacion=DateTime.Now});
        tareasInit.Add(new Tarea(){TareaId=Guid.Parse("e17ccf1e-1f91-42ab-bd2f-26759ccfd13f"),CategoriaId=Guid.Parse("e17ccf1e-1f91-42ab-bd2f-26759ccfd20f"),PrioridadTarea=Prioridad.Baja, Titulo="Mirar pelicula",FechaCreacion=DateTime.Now});
        modelBuilder.Entity<Tarea>(tarea =>
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(p => p.TareaId);
            tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId);
            tarea.Property(p => p.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(p => p.Descripcion).IsRequired(false);
            tarea.Property(p => p.PrioridadTarea);
            tarea.Property(p => p.FechaCreacion);
            tarea.Ignore(p => p.Resumen);
            tarea.HasData(tareasInit);
        });
    }

}