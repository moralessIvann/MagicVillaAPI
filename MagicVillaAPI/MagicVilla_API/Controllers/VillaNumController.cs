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
    public class VillaNumController : ControllerBase
    {
        private readonly ILogger<VillaNumController> _logger; 
        private readonly IVillaNumRepository _villaNumRepository;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        protected APIResponse _responseAPI;

        public VillaNumController(ILogger<VillaNumController> logger, IVillaNumRepository villaNumRepository,
            IVillaRepository villaRepository, IMapper mapper)
        {
            _logger = logger;
            _villaNumRepository = villaNumRepository;
            _villaRepository = villaRepository;
            _mapper = mapper;
            _responseAPI = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillasNum()
        {
            try
            {
                _logger.LogInformation("Obtener num villas");
                IEnumerable<VillaNum> villaList = await _villaNumRepository.GetAll();
                _responseAPI.Result = _mapper.Map<IEnumerable<VillaNumDTO>>(villaList);
                _responseAPI.statusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _responseAPI;
        }

        [HttpGet("id", Name = "GetVillaNum")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNum(int id)
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

                var villa = await _villaNumRepository.Get(x => x.VillaNo == id);

                if (villa == null)
                {
                    _responseAPI.statusCode = HttpStatusCode.NotFound;
                    _responseAPI.isSuccess = false;
                    return NotFound(_responseAPI);
                }

                _responseAPI.Result = _mapper.Map<VillaNumDTO>(villa);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNum([FromBody] VillaNumCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaNumRepository.Get(v => v.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("NumExiste", "Num already exists");
                    return BadRequest(ModelState);
                }

                if (await _villaRepository.Get(v => v.Id == createDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "Villa num doesnt exists");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest();
                }

                VillaNum model = _mapper.Map<VillaNum>(createDTO); //se mapea al modelo original y no el DTO ya que se crea un new model
                model.CreatedDate = DateTime.Now;
                model.ActualDate = DateTime.Now;
                await _villaNumRepository.CreateEntity(model);
                _responseAPI.Result = model;
                _responseAPI.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNum", new { id = model.VillaNo }, _responseAPI);
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
        public async Task<IActionResult> DeleteVillaNum(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responseAPI.isSuccess = false;
                    _responseAPI.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responseAPI);
                }

                var villa = await _villaNumRepository.Get(v => v.VillaNo == id);

                if (villa == null)
                {
                    _responseAPI.isSuccess = false;
                    _responseAPI.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseAPI);
                }

                await _villaNumRepository.Remove(villa);
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
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaNumUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.VillaNo)
            {
                _responseAPI.isSuccess = false;
                _responseAPI.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseAPI);
            }

            if(await _villaRepository.Get(v => v.Id == updateDTO.VillaId) == null)
            {
                ModelState.AddModelError("ForeignKey", "id doesnt exists");
                return BadRequest(ModelState);
            }

            VillaNum model = _mapper.Map<VillaNum>(updateDTO);
            await _villaNumRepository.Update(model);
            _responseAPI.statusCode = HttpStatusCode.NoContent;

            return Ok(_responseAPI);
        }

/*
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
            
            var villa = await _villaNumRepository.Get(v => v.Id == id, tracked:false);
            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDTO);
            await _villaNumRepository.Update(modelo);
            _responseAPI.statusCode = HttpStatusCode.NoContent;

            return Ok(_responseAPI);
        }
*/
    }
}
