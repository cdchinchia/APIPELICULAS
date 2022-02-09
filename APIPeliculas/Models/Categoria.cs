using System;
using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
       
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
// los parametros Id y Fecha de creacion no son necesarios que el cliente los edite puesto que el id es una primary key identity y la fecha cuando se hace la insercion del registro se agrega