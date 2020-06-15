using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    public class CellestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CellestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
