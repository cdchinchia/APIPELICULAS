using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APIPeliculas.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        //instaciar cadena de conexion 
        private readonly ApplicationDbContext _bd;

        public PeliculaRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }
        //Metodos
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Peliculas;
            //Si no es nulo, valida si existe coincidencia entre el string nombre que se esta pasando como parametro con los campos nombre y descripcion de la tabla Peliculas
            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));

            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            // Buscar en la base de datos Peliculas(c) si en nombre convertido a minuscula y cortandole los espacios existe un nombre igual al que se esta pasando como parametro entonces retorna el valor(valida si existe o  no ese nombre de pelicula)
            //Buscar con Any si exite c => c.Nombre, ToLower()pasa a miniscula, Trim() Corta si tiene espacio
            bool valor = _bd.Peliculas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExistePelicula(int id)
        {
            //Busca si existe una Pelicula que tenga el mismo id que se esta pasando como parametro 
            return _bd.Categorias.Any(c => c.Id == id);
        }

        public Pelicula GetPelicula(int PeliculaId)
        {
            // Busca el id de la pelicula que se pasa como parametro 
            return _bd.Peliculas.FirstOrDefault(c => c.Id == PeliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            //Devuelve todas las Peliculas ordenadas de manera ascendente
            return _bd.Peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int CatId)
        {
            //Devuelve una lista en donde CatID que se esta pasando como parametro incluye el modelo categoria donde categoriaID es igual al Catid que se pasa como parametro 
            return _bd.Peliculas.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == CatId).ToList();
        }

        public bool Guardar()
        {
            //cuando sea >= 0 retorna un true pero si falla retorna un false
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
