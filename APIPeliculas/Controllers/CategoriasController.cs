using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIPeliculas.Controllers
{
    [Route("api/Categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
            
        }

        //Obteniendo Categorias
        [HttpGet]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias(); // esta hace referencia al model Categoria pero este no es el que debe mostrarse al usuario si no el DTO

            //trabajndo con el DTO
            var listaCategoriasDTO = new List<CategoriaDTO>();

            foreach (var lista in listaCategorias)
            {
                listaCategoriasDTO.Add(_mapper.Map<CategoriaDTO>(lista));
            }
            return Ok(listaCategoriasDTO);
        }

        //obteniendo una sola categoria
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]//añadiendo la ruta 
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(categoriaId);
            // verificando si el id es nulo
            if (itemCategoria == null)
            {
                return NotFound();
            }

            var itemCategoriaDTO = _mapper.Map<CategoriaDTO>(itemCategoria);
            return Ok(itemCategoriaDTO);


        }

        //Creacion de una nueva categoria
        [HttpPost]
        public IActionResult CrearCategoria([FromBody] CategoriaDTO categoriaDTO)//[FromBody] indica que el cuerpo que se envia en la peticion esta vinculado a los datos de la CateriaDTO
        {
            // validar que la categoria no sea nulo
            if (categoriaDTO == null)
                return BadRequest(ModelState);
            //Validar si existe una categoria con el mismo nombre
            if (_ctRepo.ExisteCategoria(categoriaDTO.Nombre))
            {
                ModelState.AddModelError("", "La categoria existe");
                return StatusCode(404, ModelState);
            }
            // luego de pasar las validaciones crea la var categoria
            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            //validar que no haya salido un error a la hora de validar el registro
            if (!_ctRepo.CrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal en el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            //retorna el ultimo registro creado
            return CreatedAtRoute("GetCategoria", new {categoriaId = categoria.Id}, categoria);     
                                  

        }

        //Actualizacion de categoria 
        [HttpPatch("{categoriaId:int}", Name = "ActualizarCategoria")]
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody]CategoriaDTO categoriaDTO)
        {
            //valida si categoria es null o ai categoriaId no es igual categoriaDTO.Id
            if (categoriaDTO == null || categoriaId != categoriaDTO.Id)
                return BadRequest(ModelState);
            // luego de pasar las validaciones crea la var categoria
            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            //validar que no haya salido un error a la hora de validar el registro
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            //retorna el ultimo registro creado
            return NoContent();

        }

        //Eliminar una categoria 
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        public IActionResult BorrarCategoria(int categoriaId)
        {

            //validar el CategoriaId exista en _ctRepo.ExisteCategoria
            if (!_ctRepo.ExisteCategoria(categoriaId))
                return NotFound();
            // si la categoria existe la busca y ejecuta la accion 
            var categoria = _ctRepo.GetCategoria(categoriaId);
            //validar que no haya salido un error a la hora de validar el registro
            if (!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal Borrando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            //retorna el ultimo registro creado
            return NoContent();

        }
    }
}
