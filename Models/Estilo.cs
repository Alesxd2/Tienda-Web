using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Models
{
    public class Estilo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del estilo es obligatorio")]
        [Display(Name = "Nombre Estilo")]

        public string Nombre { get; set; }

        public string Descripcion { get; set; }


    }
}
