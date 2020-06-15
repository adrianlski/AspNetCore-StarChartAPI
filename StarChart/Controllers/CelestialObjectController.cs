using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.SingleOrDefault(x => x.Id == id);

            if (celestialObject == null)
            {
                return NotFound();
            }

            var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            celestialObject.Satellites = satellites;

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObject.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObject.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new {Id = celestialObject.Id}, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var toUpdate = _context.CelestialObjects.SingleOrDefault(x => x.Id == id);

            if (toUpdate == null)
            {
                return NotFound();
            }

            toUpdate.Name = celestialObject.Name;
            toUpdate.OrbitalPeriod = celestialObject.OrbitalPeriod;
            toUpdate.Satellites = celestialObject.Satellites;

            _context.CelestialObjects.Update(toUpdate);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var toPatch = _context.CelestialObjects.SingleOrDefault(x => x.Id == id);

            if (toPatch == null)
            {
                return NotFound();
            }

            toPatch.Name = name;

            _context.CelestialObjects.Update(toPatch);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var toDelete = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id).ToList();

            if (!toDelete.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(toDelete);
            _context.SaveChanges();

            return NoContent();

        }
    }
}
