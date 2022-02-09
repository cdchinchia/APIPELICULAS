﻿using System;
using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
