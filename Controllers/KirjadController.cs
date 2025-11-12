using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kirjad.Data;
using Kirjad.Models;

namespace Kirjad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KirjadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KirjadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Kirjad
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kiri>>> GetKirjad()
        {
            return await _context.Kirjad
                .Include(k => k.TagasiViited)
                .ToListAsync();
        }

        // GET: api/Kirjad/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kiri>> GetKiri(int id)
        {
            var kiri = await _context.Kirjad
                .Include(k => k.SeotudKirjad)
                .Include(k => k.TagasiViited)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kiri == null)
            {
                return NotFound();
            }

            return kiri;
        }

        // GET: api/Kirjad/kood/KIRI-001
        [HttpGet("kood/{kood}")]
        public async Task<ActionResult<Kiri>> GetKiriByKood(string kood)
        {
            var kiri = await _context.Kirjad
                .Include(k => k.SeotudKirjad)
                .Include(k => k.TagasiViited)
                .FirstOrDefaultAsync(k => k.UnikaalsekKood == kood);

            if (kiri == null)
            {
                return NotFound();
            }

            return kiri;
        }

        // GET: api/Kirjad/populaarsed
        [HttpGet("populaarsed")]
        public async Task<ActionResult<IEnumerable<object>>> GetPopulaarsedKirjad()
        {
            var populaarsedKirjad = await _context.Kirjad
                .Include(k => k.TagasiViited)
                .Select(k => new
                {
                    k.Id,
                    k.Pealkiri,
                    k.Sisu,
                    k.UnikaalsekKood,
                    ViideteArv = k.TagasiViited.Count,
                    KiriTyyp = k is Veebiuudis ? "Veebiuudis" : "Kiri",
                    URL = k is Veebiuudis ? ((Veebiuudis)k).URL : null
                })
                .OrderByDescending(k => k.ViideteArv)
                .ToListAsync();

            return Ok(populaarsedKirjad);
        }

        // POST: api/Kirjad
        [HttpPost]
        public async Task<ActionResult<Kiri>> PostKiri(Kiri kiri)
        {
            // Genereeri unikaalne kood, kui puudub
            if (string.IsNullOrEmpty(kiri.UnikaalsekKood))
            {
                kiri.UnikaalsekKood = $"KIRI-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            }

            _context.Kirjad.Add(kiri);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetKiri), new { id = kiri.Id }, kiri);
        }

        // POST: api/Kirjad/veebiuudis
        [HttpPost("veebiuudis")]
        public async Task<ActionResult<Veebiuudis>> PostVeebiuudis(Veebiuudis uudis)
        {
            // Genereeri unikaalne kood, kui puudub
            if (string.IsNullOrEmpty(uudis.UnikaalsekKood))
            {
                uudis.UnikaalsekKood = $"UUDIS-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            }

            _context.Veebiuudised.Add(uudis);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetKiri), new { id = uudis.Id }, uudis);
        }

        // POST: api/Kirjad/5/viide/3
        [HttpPost("{allikaId}/viide/{sihtId}")]
        public async Task<IActionResult> LisaViide(int allikaId, int sihtId)
        {
            var allikas = await _context.Kirjad
                .Include(k => k.SeotudKirjad)
                .FirstOrDefaultAsync(k => k.Id == allikaId);

            var siht = await _context.Kirjad.FindAsync(sihtId);

            if (allikas == null || siht == null)
            {
                return NotFound("Üks või mõlemad kirjad ei eksisteeri.");
            }

            if (allikas.SeotudKirjad.Any(k => k.Id == sihtId))
            {
                return BadRequest("See viide on juba olemas.");
            }

            allikas.SeotudKirjad.Add(siht);
            await _context.SaveChangesAsync();

            return Ok("Viide lisatud!");
        }

        // DELETE: api/Kirjad/5/viide/3
        [HttpDelete("{allikaId}/viide/{sihtId}")]
        public async Task<IActionResult> KustutaViide(int allikaId, int sihtId)
        {
            var allikas = await _context.Kirjad
                .Include(k => k.SeotudKirjad)
                .FirstOrDefaultAsync(k => k.Id == allikaId);

            if (allikas == null)
            {
                return NotFound();
            }

            var siht = allikas.SeotudKirjad.FirstOrDefault(k => k.Id == sihtId);
            if (siht == null)
            {
                return NotFound("Viide ei eksisteeri.");
            }

            allikas.SeotudKirjad.Remove(siht);
            await _context.SaveChangesAsync();

            return Ok("Viide kustutatud!");
        }

        // PUT: api/Kirjad/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKiri(int id, Kiri kiri)
        {
            if (id != kiri.Id)
            {
                return BadRequest();
            }

            _context.Entry(kiri).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KiriExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Kirjad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKiri(int id)
        {
            var kiri = await _context.Kirjad.FindAsync(id);
            if (kiri == null)
            {
                return NotFound();
            }

            _context.Kirjad.Remove(kiri);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Kirjad/5/kirjuta
        [HttpGet("{id}/kirjuta")]
        public async Task<ActionResult<string>> KirjutaKiri(int id)
        {
            var kiri = await _context.Kirjad.FindAsync(id);
            if (kiri == null)
            {
                return NotFound();
            }

            return Ok(kiri.Kirjuta());
        }

        private bool KiriExists(int id)
        {
            return _context.Kirjad.Any(e => e.Id == id);
        }
    }
}