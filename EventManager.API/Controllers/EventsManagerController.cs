using DevEvents.API.Entities;
using DevEvents.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevEvents.API.Controllers
{
    [Route("api/event-manager")]
    [ApiController]
    public class EventsManagerController : ControllerBase
    {
        private readonly EventManagerDbContext _context;
        public EventsManagerController(EventManagerDbContext context)
        { 
           _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var eventManager = _context.EventManager.Where(d => !d.IsDeleted).ToList();
            return Ok(eventManager); 
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var eventManager = _context.EventManager
                .Include(de => de.Speakers)
                .SingleOrDefault(d => d.Id == id);
                

            if (eventManager == null)
            {
                return NotFound();
            }
            return Ok(eventManager);
        }

        [HttpPost]
        public IActionResult Post(EventManager eventManager)
        {
            _context.EventManager.Add(eventManager);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = eventManager.Id}, eventManager);
        }

        [HttpPut("{id}")]
        public IActionResult Update(EventManager input)
        {
            var eventManager = _context.EventManager.SingleOrDefault(d => d.Id == input.Id && !d.IsDeleted);

            if(eventManager == null)
            {
                return NotFound();
            }

            eventManager.Update(input.Title, input.Description, input.StartDate,input.EndDate);
            _context.EventManager.Update(eventManager);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.EventManager.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Delete();
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(Guid id, EventManagerSpeaker speaker)
        {
            speaker.EventManagerId= id;
            var devEvent = _context.EventManager.Any(i => i.Id == id);

            if(!devEvent)
            {
                return NotFound();
            }

            _context.EventManagerSpeaker.Add(speaker);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
