using AutoMapper;
using Azure;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using MagicVilla_API.Repository.IRepository;
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
        private readonly ILogger<VillaController> _logger; 
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        protected APIResponse _responseAPI;

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepository, 
            IMapper mapper)
        {
            _logger = logger;
            _villaRepository = villaRepository;
            _mapper = mapper;
            _responseAPI = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las Villas");
                IEnumerable<Villa> villaList = await _villaRepository.GetAll();
                _responseAPI.Result = _mapper.Map<IEnumerable<VillaDTO>>(villaList);
                _responseAPI.statusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _responseAPI;
        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al consultar la villa id" + id);
                    _responseAPI.statusCode = HttpStatusCode.BadRequest;
                    _responseAPI.isSuccess = false;
                    return BadRequest(_responseAPI);
                }

                var villa = await _villaRepository.Get(x => x.Id == id);

                if (villa == null)
                {
                    _responseAPI.statusCode = HttpStatusCode.NotFound;
                    _responseAPI.isSuccess = false;
                    return NotFound(_responseAPI);
                }

                _responseAPI.Result = _mapper.Map<VillaDTO>(villa);
                _responseAPI.statusCode = HttpStatusCode.OK;
                return Ok(_responseAPI);
            }
            catch (Exception ex)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseAPI;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepository.Get(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "Name already exists");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest();
                }

                Villa model = _mapper.Map<Villa>(createDTO); //se mapea al modelo original y no el DTO ya que se crea un new model
                await _villaRepository.CreateEntity(model);
                _responseAPI.Result = model;
                _responseAPI.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.ErrorMessages = new List<string>() { ex.Message };
            }

            return _responseAPI;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responseAPI.isSuccess = false;
                    _responseAPI.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responseAPI);
                }

                var villa = await _villaRepository.Get(v => v.Id == id);

                if (villa == null)
                {
                    _responseAPI.isSuccess = false;
                    _responseAPI.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseAPI);
                }

                await _villaRepository.Remove(villa);
                _responseAPI.statusCode = HttpStatusCode.NoContent;
                return Ok(_responseAPI);
                
            }
            catch (Exception ex)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.ErrorMessages = new List<string>() { ex.Message };
            }

            return BadRequest(_responseAPI);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseAPI);
            }

            Villa model = _mapper.Map<Villa>(updateDTO);
            await _villaRepository.Update(model);
            _responseAPI.statusCode = HttpStatusCode.NoContent;

            return Ok(_responseAPI);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseAPI);
            }
            
            var villa = await _villaRepository.Get(v => v.Id == id, tracked:false);
            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDTO);
            await _villaRepository.Update(modelo);
            _responseAPI.statusCode = HttpStatusCode.NoContent;

            return Ok(_responseAPI);
        }
    }
}
