using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace APIPeliculas.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        //instaciar cadena de conexion 
        private readonly ApplicationDbContext _bd;

        public CategoriaRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }
       //Metodos
        public bool ActualizarCategoria(Categoria categoria)
        {
            _bd.Categorias.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool CrarCategoria(Categoria categoria)
        {
            _bd.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            //Buscar en la base de datos categorias(c) si en nombre convertido a minuscula y cortandole los espacios existe un nombre igual al que se esta pasando como parametro entonces retorna el valor (valida si existe o  no ese nombre de categoria)
            //Buscar con Any si exite c => c.Nombre, ToLower()pasa a miniscula, Trim() Corta si tiene espacio
            bool valor = _bd.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExisteCategoria(int id)
        {
            //Busca si existe una categoria que tenga el mismo id que se esta pasando como parametro 
            return _bd.Categorias.Any(c => c.Id == id);
        }

        public Categoria GetCategoria(int CategoriaId)
        {
            // Busca el id de la categoria que se pasa como parametro 
            return _bd.Categorias.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            //Devuelve todas las categorias ordenadas de manera ascendente
            return _bd.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            //cuando sea >= 0 retorna un true pero si falla retorna un false
            return _bd.SaveChanges() >= 0 ? true : false; 
        }
    }
}
//en la interface ICategoriaRepository se escriben los metodos y se implementan en CategoriaRepository