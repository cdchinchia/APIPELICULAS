using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace APIPeliculas.Controllers
{
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepository _pelRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;//subida de archivos


        public PeliculasController(IPeliculaRepository pelRepo, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;

        }

        //Metodo.Obteniendo Peliculas
        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRepo.GetPeliculas(); // esta hace referencia al model Pelicula pero este no es el que debe mostrarse al usuario si no el DTO

            //trabajndo con el DTO
            var listaPeliculasDTO = new List<PeliculaDTO>();

            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDTO.Add(_mapper.Map<PeliculaDTO>(lista));
            }
            return Ok(listaPeliculasDTO);
        }

        //Metodo.Obteniendo una sola pelicula
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]//añadiendo la ruta 
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);
            // verificando si el id es nulo
            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDTO = _mapper.Map<PeliculaDTO>(itemPelicula);
            return Ok(itemPeliculaDTO);
        }

        //Metodo.Buscar peliculas en una categoria recibiendo el id de la categoria 
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPelicula = _pelRepo.GetPeliculasEnCategoria(categoriaId);
            //validacion si es nulo
            if (listaPelicula == null)
                return NotFound();

            var itemPelicula = new List<PeliculaDTO>();
            foreach (var item in listaPelicula)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDTO>(item));

            }
            return Ok(itemPelicula);
        }

        //Metodo.Buscar peliculas por el nombre en la base de datos
        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombre);
                if (resultado.Any())
                    return Ok(resultado);

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");

            }
        }

        //Metodo.Creacion de una nueva pelicula
        [HttpPost]
        public IActionResult CrearPelicula([FromForm] PeliculaCreateDTO PeliculaDTO)//[FromForm] indica que lo va obtener de un formulario, esto para utilizar la subida de archivos
        {
            // validar que la pelicula no sea nulo
            if (PeliculaDTO == null)
                return BadRequest(ModelState);
            //Validar si existe una pelicula con el mismo nombre
            if (_pelRepo.ExistePelicula(PeliculaDTO.Nombre))
            {
                ModelState.AddModelError("", "La pelicula existe");
                return StatusCode(404, ModelState);
            }
            //subida de archivos
            var archivo = PeliculaDTO.Foto;
            string rutaPrincipal = _hostingEnvironment.WebRootPath;//@"D:\CRISTIAN-PERSONAL\ASP.NET Core 3.1 y5\Proyectos\APIPELICULAS\APIPeliculas\wwwroot\fotos\";
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                //Nueva imagen
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto, extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                PeliculaDTO.RutaImagen = @"\fotos\" + nombreFoto + extension;
            }                   

            // luego de pasar las validaciones crea la var pelicula y mapearla
            var pelicula = _mapper.Map<Pelicula>(PeliculaDTO);
            //validar que no haya salido un error a la hora de validar el registro
            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal en el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            //retorna el ultimo registro creado
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);


        }

        //Metodo.Actualizacion de pelicula 
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaUpdateDTO peliculaUpdateDTO)
        {
            //valida si pelicula es null o ai peliculaId no es igual peliculaDTO.Id
            if (peliculaUpdateDTO == null || peliculaId != peliculaUpdateDTO.Id)
                return BadRequest(ModelState);
            // luego de pasar las validaciones crea la var pelicula
            var pelicula = _mapper.Map<Pelicula>(peliculaUpdateDTO);
            //validar que no haya salido un error a la hora de validar el registro
            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            //retorna el ultimo registro creado
            return NoContent();

        }

        //Metodo.Eliminar una pelicula 
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        public IActionResult BorrarPelicula(int peliculaId)
        {

            //validar el PeliculaId exista en _pelRepo.ExistePelicula
            if (!_pelRepo.ExistePelicula(peliculaId))
                return NotFound();
            // si la pelicula existe la busca y ejecuta la accion 
            var pelicula = _pelRepo.GetPelicula(peliculaId);
            //validar que no haya salido un error a la hora de validar el registro
            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal Borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            //retorna el ultimo registro creado
            return NoContent();

        }

    }
}
