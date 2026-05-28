using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaWeb.Models
{
    public class Cerveza
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingresa el nombre")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Ingresa el porcentaje de alcohol")]
        [Display(Name = "% de Alcohol")]
        public double alcohol { get; set; }
        [Display(Name = "Estilo")]
        public int IdEstilo { get; set; }
        [ForeignKey("IdEstilo")]
        public Estilo Estilo { get; set; }
        [Required(ErrorMessage = "Ingresa el precio")]
        [Display(Name = "Precio")]
        public double precio { get; set; }
        [Display(Name = "Imagen")]
        public string? UrlImagen { get; set; }
    }
}