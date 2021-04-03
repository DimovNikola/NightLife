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
using System.Threading.Tasks;

namespace NightLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly INightLifeRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public EventsController(INightLifeRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventModel>>> GetEvents()
        {
            try
            {
                var events = await _repository.GetAllClubEvents();

                if (events == null)
                {
                    return NotFound();
                }

                return _mapper.Map<List<EventModel>>(events);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }
        }

        [HttpGet("{clubId:int}/{eventId:int}")]
        public async Task<ActionResult<EventModel>> GetEvent(int clubId, int eventId)
        {
            try
            {
                var clubEvent = await _repository.GetClubEvent(clubId, eventId);

                if (clubEvent == null)
                {
                    return NotFound();
                }

                return _mapper.Map<EventModel>(clubEvent);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventModel>> PostEvent(EventModel model)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction(
                    "GetEvent",
                    "Events",
                    new
                    {
                        clubId = model.ClubId,
                        eventId = model.Id
                    }
                );

                if (String.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current event");
                }

                var clubEvent = _mapper.Map<Event>(model);
                _repository.AddEvent(clubEvent);

                if (await _repository.SaveChangesAsync())
                {
                    return Created("", _mapper.Map<EventModel>(clubEvent));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }

            return BadRequest();
        }

        [HttpPut("{clubId:int}/{eventId:int}")]
        public async Task<ActionResult<EventModel>> PutEvent(EventModel model, int clubId, int eventId)
        {
            try
            {
                var oldClub = await _repository.GetClubEvent(clubId, eventId);

                if(oldClub == null)
                {
                    return NotFound($"Event not found with id: {eventId}");
                }

                _mapper.Map(model, oldClub);

                if(await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<EventModel>(oldClub);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }

            return BadRequest();

        }

        [HttpDelete("{clubId:int}/{eventId:int}")]
        public async Task<IActionResult> DeleteEvent(int clubId, int eventId)
        {
            try
            {
                var clubEvent = await _repository.GetClubEvent(clubId, eventId);

                if(clubEvent == null)
                {
                    return NotFound("Event does not exist");
                }

                _repository.DeleteEvent(clubEvent);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message.ToString());
            }

            return BadRequest();
        }
    }
}
