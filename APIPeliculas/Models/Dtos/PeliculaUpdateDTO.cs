using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static APIPeliculas.Models.Pelicula;

namespace APIPeliculas.Models.Dtos
{
    public class PeliculaUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "la descripción es obligatoria")]
        public string Descripcion { get; set; }
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }

        public int categoriaId { get; set; }
    }
}
// DTO que va permitir actualizar una pelicula enviandole el metodo patch a la API