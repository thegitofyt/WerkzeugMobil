using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WerkzeugMobil.Data;

namespace WerkzeugMobil.WerkzeugApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WerkzeugeController : ControllerBase
    {
        private readonly WerkzeugDbContext _context;

        public WerkzeugeController(WerkzeugDbContext context)
        {
            _context = context;
        }

        // GET /werkzeuge
        [HttpGet]
        public IActionResult GetAllWerkzeuge()
        {
            try
            {
                var werkzeuge = _context.Werkzeuge.ToList(); // no Include to avoid cycles
                return Ok(werkzeuge);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving Werkzeuge: {ex.Message}");
            }
        }

        // GET /werkzeuge/projekte
        [HttpGet("projekte")]
        public IActionResult GetProjekte()
        {
            try
            {
                var projekte = _context.Projekte.ToList();
                return Ok(projekte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving Projekte: {ex.Message}");
            }
        }
        // GET /werkzeuge/{id}
        [HttpGet("{id}")]
        public IActionResult GetWerkzeug(int id)
        {
            try
            {
                var werkzeug = _context.Werkzeuge.Find(id);
                if (werkzeug == null)
                    return NotFound();

                return Ok(werkzeug);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving Werkzeug with id {id}: {ex.Message}");
            }
        }
        // GET /werkzeuge/tools
        [HttpGet("tools")]
        public IActionResult GetTools()
        {
            try
            {
                var tools = _context.Tools.ToList();
                return Ok(tools);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving Tools: {ex.Message}");
            }
        }

        // GET /werkzeuge/benutzer
        [HttpGet("benutzer")]
        public IActionResult GetBenutzer()
        {
            try
            {
                var benutzer = _context.Benutzer.ToList();
                return Ok(benutzer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving Benutzer: {ex.Message}");
            }
        }

       

    }


}