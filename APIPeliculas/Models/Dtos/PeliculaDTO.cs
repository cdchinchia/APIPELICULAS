﻿using System;
using System.ComponentModel.DataAnnotations;
using static APIPeliculas.Models.Pelicula;

namespace APIPeliculas.Models.Dtos
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La ruta de la imagen es obligatoria")]
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "la descripción es obligatoria")]
        public string Descripcion { get; set; }
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }        
        
        public int categoriaId { get; set; }        
        public Categoria Categoria { get; set; }
    }
}
