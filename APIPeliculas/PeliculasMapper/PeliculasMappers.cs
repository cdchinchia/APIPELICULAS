
using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using AutoMapper;

namespace APIPeliculas.PeliculasMapper
{
    public class PeliculasMappers : Profile
    {
        public PeliculasMappers()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<Pelicula, PeliculaCreateDTO>().ReverseMap();
            CreateMap<Pelicula, PeliculaUpdateDTO>().ReverseMap();
        }
    }
}
// se vincula Categoria con CateriaDTO utilizando Automapper. aca es donde se vinculan todos lo modelos con sus respectivos DTOs