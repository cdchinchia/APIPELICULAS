using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static APIPeliculas.Models.Pelicula;

namespace APIPeliculas.Models.Dtos
{
    public class PeliculaCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        
        public string RutaImagen { get; set; }        
        public IFormFile Foto { get; set; } // esta propiedad permite subir la imagen a la API 
        [Required(ErrorMessage = "la descripción es obligatoria")]
        public string Descripcion { get; set; }
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }

        public int categoriaId { get; set; }
        
    }
}
// DTO que va permitir crear una pelicula enviandole el metodo post a la API