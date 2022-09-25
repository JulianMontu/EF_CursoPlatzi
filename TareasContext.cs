using Microsoft.EntityFrameworkCore;
using projectef.Models;

namespace projectef;
public class TareasContext:DbContext
{
    public DbSet<Categoria> Categorias;
    public DbSet<Tarea> Tareas;
    public TareasContext(DbContextOptions<TareasContext> options) : base(options){}
}