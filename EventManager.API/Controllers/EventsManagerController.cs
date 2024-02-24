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
        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code="200">Sucesso</response>
        /// /// <response code="404">Não encontrado</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var eventManager = _context.EventManager.Where(d => !d.IsDeleted).ToList();
            return Ok(eventManager); 
        }

        /// <summary>
        /// Obter um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Dados do evento</returns>
        /// <respose code="200">Sucesso</respose>
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
        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks> 
        /// Objeto Json
        /// </remarks>
        /// <param name="eventManager">Dados do evento</param>
        /// <returns>Objeto recém criado</returns>
        /// /// <respose code="201">Sucesso</respose>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(EventManager eventManager)
        {
            _context.EventManager.Add(eventManager);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = eventManager.Id}, eventManager);
        }
        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks> 
        /// Objeto Json
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Nada.</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Deleta um evento
        /// </summary>
        /// <param name="id">Identificador de evento</param>
        /// <returns>Nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Cadastra palestrante
        /// </summary>
        /// <remarks>
        /// Objeto Json
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="speaker">Dados do palestrante</param>
        /// <returns>Nada</returns>
        ///  <response code="204">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpPost("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
