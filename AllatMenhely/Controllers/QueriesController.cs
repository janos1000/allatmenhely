using AllatMenhely.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllatMenhely.Controllers
{
    [Route("queries")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly AnimalshelterContext _context;

        public QueriesController(AnimalshelterContext context)
        {
            _context = context;
        }

        
        [HttpGet("allSheltersWithAnimals")]
        public async Task<IActionResult> GetAllSheltersWithAnimals()
        {
            try
            {
                var result = await _context.Shelters
                    .Include(s => s.Animals)
                    .ToListAsync();

                return StatusCode(201, new { message = "Sikeres lekérdezés.", result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }

        
        [HttpGet("allAnimalsWithShelter")]
        public async Task<IActionResult> GetAllAnimalsWithShelter()
        {
            try
            {
                var result = await _context.Animals
                    .Join(
                        _context.Shelters,
                        animal => animal.ShelterId,
                        shelter => shelter.Id,
                        (animal, shelter) => new
                        {
                            animal.Id,
                            animal.Name,
                            animal.Species,
                            animal.Age,
                            animal.Weight,
                            animal.RegTime,
                            ShelterName = shelter.Name
                        })
                    .ToListAsync();

                return StatusCode(201, new { message = "Sikeres lekérdezés.", result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }

        
        [HttpGet("animalsOfShelter")]
        public async Task<IActionResult> GetAnimalsOfShelter([FromQuery] Guid shelterId)
        {
            try
            {
                var shelter = await _context.Shelters
                    .Include(s => s.Animals)
                    .Where(s => s.Id == shelterId)
                    .FirstOrDefaultAsync();

                if (shelter == null)
                {
                    return NotFound(new { message = "A megadott menhely nem található.", result = (object?)null });
                }

                return StatusCode(201, new { message = "Sikeres lekérdezés.", result = shelter });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }

        
        [HttpGet("animalCountByShelter")]
        public async Task<IActionResult> GetAnimalCountByShelter()
        {
            try
            {
                var result = await _context.Shelters
                    .Select(s => new
                    {
                        ShelterId = s.Id,
                        ShelterName = s.Name,
                        AnimalCount = s.Animals.Count()
                    })
                    .OrderByDescending(x => x.AnimalCount)
                    .ToListAsync();

                return StatusCode(201, new { message = "Sikeres lekérdezés.", result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }
    }
}