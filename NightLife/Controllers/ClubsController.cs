using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NightLife.Models;
using NightLife.Models.Response;
using NightLife.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClubsController : ControllerBase
    {
        private readonly INightLifeRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ClubsController(INightLifeRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClubModel>>> GetAllClubs()
        {
            try
            {
                var results = await _repository.GetAllClubs();

                return _mapper.Map<List<ClubModel>>(results);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }
        }

        [HttpGet("{clubId:int}")]
        public async Task<ActionResult<ClubModel>> GetClub(int clubId)
        {
            try
            {
                var result = await _repository.GetClub(clubId);

                if (result == null)
                {
                    return NotFound();
                }

                return _mapper.Map<ClubModel>(result);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClubModel>> PostClub(ClubModel model)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction("GetClub", "Clubs", new { clubId = model.Id });

                if (String.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current club");
                }

                var club = _mapper.Map<Club>(model);
                _repository.AddClub(club);

                if (await _repository.SaveChangesAsync())
                {
                    return Created(location.ToString(), _mapper.Map<ClubModel>(club));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }

            return BadRequest();
        }

        [HttpPut("{clubId:int}")]
        public async Task<ActionResult<ClubModel>> PutClub(ClubModelPost model, int clubId)
        {
            try
            {
                var oldClub = await _repository.GetClub(clubId);

                if (oldClub == null)
                {
                    return NotFound($"Could not find club with id: {clubId}");
                }

                _mapper.Map(model, oldClub);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ClubModel>(oldClub);
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }

            return BadRequest();
        }

        [HttpDelete("{clubId:int}")]
        public async Task<IActionResult> DeleteClub(int clubId)
        {
            try
            {
                var club = await _repository.GetClub(clubId);

                if (club == null)
                {
                    return NotFound("Club doesn't exist!");
                }

                _repository.DeleteClub(club);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }

            return BadRequest();
        }
    }
}
