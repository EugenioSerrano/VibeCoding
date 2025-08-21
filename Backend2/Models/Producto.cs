using System.ComponentModel.DataAnnotations;

namespace Backend2.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}