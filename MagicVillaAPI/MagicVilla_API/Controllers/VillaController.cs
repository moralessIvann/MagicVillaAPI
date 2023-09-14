using AutoMapper;
using Azure;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger; //se inyecta para usarlo
        private readonly ApplicationDbContext _dbContext; //se inyecta el dbcontext para usar los datos de esta clase y ya no usar los de VillaStore
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Obtener las Villas");

            IEnumerable<Villa> villaList = await _dbContext.Villas.ToListAsync();

            //return Ok(await _dbContext.Villas.ToListAsync()); //esto como hace select*from desde la tabla en la bd
            return Ok(_mapper.Map<IEnumerable<VillaDTO>>(villaList));
        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (id == 0)
            {
                _logger.LogError("Error al consultar la villa id" + id);
                return BadRequest();
            }

            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            if (await _dbContext.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "Name already exists");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest();
            }
            //if (villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            //villaDTO.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //VillaStore.villaList.Add(villaDTO);

            //Villa modelo = new()
            //{
            //    //Id = villaDTO.Id, //no se necesita especificar ya que se agrega e incremnetea solo (ya se especifico en el modelo)
            //    Name = villaDTO.Name,
            //    Detail = villaDTO.Detail,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Occupants = villaDTO.Occupants,
            //    Fee = villaDTO.Fee,
            //    SquareMeters = villaDTO.SquareMeters,
            //    Comfort = villaDTO.Comfort,
            //};

            Villa model = _mapper.Map<Villa>(createDTO); //se mapea al modelo original y no el DTO ya que se crea un new model

            await _dbContext.Villas.AddAsync(model); //insert in db
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            //VillaStore.villaList.Remove(villa);
            _dbContext.Villas.Remove(villa);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }

            /*
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            villa.Name = villaDTO.Name;
            villa.Occupants = villaDTO.Occupants;
            villa.SquareMeters = villaDTO.SquareMeters;
            */

            //Villa modelo = new()
            //{
            //    Id = villaDTO.Id,
            //    Name = villaDTO.Name,
            //    Detail = villaDTO.Detail,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Occupants = villaDTO.Occupants,
            //    Fee = villaDTO.Fee,
            //    SquareMeters = villaDTO.SquareMeters,
            //    Comfort = villaDTO.Comfort,
            //};
            
            Villa model = _mapper.Map<Villa>(updateDTO);

            _dbContext.Villas.Update(model); //update in db
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            //VillaUpdateDTO villaDTO = new()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Detail = villa.Detail,
            //    ImageUrl = villa.ImageUrl,
            //    Occupants = villa.Occupants,
            //    Fee = villa.Fee,
            //    SquareMeters = villa.SquareMeters,
            //    Comfort = villa.Comfort,
            //};

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDTO);

            //Villa modelo = new()
            //{
            //    Id = villaDTO.Id,
            //    Name = villaDTO.Name,
            //    Detail = villaDTO.Detail,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Occupants = villaDTO.Occupants,
            //    Fee = villaDTO.Fee,
            //    SquareMeters = villaDTO.SquareMeters,
            //    Comfort = villaDTO.Comfort,
            //};

            _dbContext.Villas.Update(modelo);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
