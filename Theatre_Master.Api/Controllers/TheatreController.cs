using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class TheatreController : ControllerBase
    {
        #region configuration
        private readonly TheatreMasterDbContext _context;

        public TheatreController(TheatreMasterDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetTheatre
        [HttpGet]

        public async Task<IActionResult> GetTheatre()
        {
            var theatre = await _context.Theatres.ToListAsync();
            return Ok(theatre);
        }
        #endregion


        #region AddTheatre
        [HttpPost]
        public async Task<IActionResult> AddTheatre(Theatre theatre)
        {
            theatre.Created = DateTime.Now;
            theatre.Modified = DateTime.Now;
            await _context.Theatres.AddAsync(theatre);
            await _context.SaveChangesAsync();
            return Ok(theatre);
        }
        #endregion

        #region GetTheatreById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheatreById(int id)
        {
            var theatre = await _context.Theatres.FindAsync(id);
            if (theatre == null)
            {
                return NotFound();
            }
            return Ok(theatre);
        }
        #endregion

        #region UpdateTheatre
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheatre(int id, Theatre theatre)
        {
            if (id != theatre.TheatreId)
            {
                return BadRequest();
            }
            var existingTheatre = await _context.Theatres.FindAsync(id);
            if (existingTheatre == null)
            {
                return NotFound();
            }
            existingTheatre.TheatreName = theatre.TheatreName;
            existingTheatre.City = theatre.City;
            existingTheatre.Modified = DateTime.Now;
            _context.Theatres.Update(existingTheatre);
            await _context.SaveChangesAsync();
            return Ok(existingTheatre);
        }
        #endregion

        #region DeleteTheatre
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheatre(int id)
        {
            var theatre = await _context.Theatres.FindAsync(id);
            if (theatre == null)
            {
                return NotFound();
            }
            _context.Theatres.Remove(theatre);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion



    }
}
