using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace projectef.Models;
public class Categoria
{
 //   [Key]
    public Guid CategoriaId { get; set; }
   // [Required]
    //[MaxLength]
    public string Nombre { get; set; }
    
    public string Descripcion { get; set; }
    public int Peso {get;set;}
    [JsonIgnore]
    public virtual ICollection<Tarea> Tareas { get; set; }
}