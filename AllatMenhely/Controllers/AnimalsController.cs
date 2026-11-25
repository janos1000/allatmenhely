using AllatMenhely.Models;
using AllatMenhely.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllatMenhely.Controllers
{
    [Route("animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalshelterContext _context;

        public AnimalsController(AnimalshelterContext context)
        {
            _context = context;
        }

        
        [HttpPost("newAnimal")]
        public async Task<IActionResult> NewAnimal([FromBody] NewAnimalDto dto)
        {
            try
            {
                var shelterExists = await _context.Shelters.AnyAsync(s => s.Id == dto.ShelterId);
                if (!shelterExists)
                {
                    return NotFound(new { message = "A megadott menhely nem található.", result = (object?)null });
                }

                var animal = new Animal
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Species = dto.Species,
                    Age = dto.Age,
                    Weight = dto.Weight,
                    RegTime = DateTime.Now,
                    ShelterId = dto.ShelterId
                };

                _context.Animals.Add(animal);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Sikeres rögzítés.", result = animal });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }

        
        [HttpGet("allAnimal")]
        public async Task<IActionResult> GetAllAnimals()
        {
            try
            {
                var animals = await _context.Animals.ToListAsync();
                if (animals.Count == 0)
                {
                    return NotFound(new { message = "Nincs megjeleníthető állat az adatbázisban.", result = (object?)null });
                }

                return StatusCode(201, new { message = "Sikeres lekérdezés.", result = animals });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }

        
        [HttpGet("byId")]
        public async Task<IActionResult> GetAnimalById([FromQuery] Guid id)
        {
            try
            {
                var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
                if (animal == null)
                {
                    return NotFound(new { message = "A megadott azonosítóval nem található állat.", result = (object?)null });
                }

                return StatusCode(201, new { message = "Sikeres lekérdezés.", result = animal });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }

        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAnimal([FromQuery] Guid id, [FromBody] UpdateAnimalDto dto)
        {
            try
            {
                var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
                if (animal == null)
                {
                    return NotFound(new { message = "A módosítandó állat nem található.", result = (object?)null });
                }

                var shelterExists = await _context.Shelters.AnyAsync(s => s.Id == dto.ShelterId);
                if (!shelterExists)
                {
                    return NotFound(new { message = "A megadott menhely nem található.", result = (object?)null });
                }

                animal.Name = dto.Name;
                animal.Species = dto.Species;
                animal.Age = dto.Age;
                animal.Weight = dto.Weight;
                animal.ShelterId = dto.ShelterId;

                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Sikeres módosítás.", result = animal });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }

        
        [HttpDelete("deleteAnimal")]
        public async Task<IActionResult> DeleteAnimal([FromQuery] Guid id)
        {
            try
            {
                var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
                if (animal == null)
                {
                    return NotFound(new { message = "A törlendő állat nem található.", result = (object?)null });
                }

                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Sikeres törlés.", result = animal });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hiba történt: {ex.Message}", result = (object?)null });
            }
        }
    }
}