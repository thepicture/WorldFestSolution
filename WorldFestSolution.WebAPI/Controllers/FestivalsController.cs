using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WorldFestSolution.WebAPI.Models.Entities;
using WorldFestSolution.WebAPI.Models.Serialized;

namespace WorldFestSolution.WebAPI.Controllers
{
    public class FestivalsController : ApiController
    {
        private readonly WorldFestBaseEntities db = new WorldFestBaseEntities();

        // GET: api/Festivals
        [Authorize(Roles = "Участник, Организатор")]
        [ResponseType(typeof(IEnumerable<SerializedFestival>))]
        public IEnumerable<SerializedFestival> GetFestival()
        {
            if (HttpContext.Current.User.IsInRole("Организатор"))
            {
                return db.User
                    .First(u => u.Login == HttpContext.Current.User.Identity.Name)
                    .Festival
                    .ToList()
                    .ConvertAll(f => new SerializedFestival(f));
            }
            return db.Festival
                .ToList()
                .ConvertAll(f => new SerializedFestival(f));
        }

        // GET: api/Festivals/5
        [ResponseType(typeof(Festival))]
        public async Task<IHttpActionResult> GetFestival(int id)
        {
            Festival festival = await db.Festival.FindAsync(id);
            if (festival == null)
            {
                return NotFound();
            }

            return Ok(festival);
        }

        // PUT: api/Festivals/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFestival(int id, Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != festival.Id)
            {
                return BadRequest();
            }

            db.Entry(festival).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FestivalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Festivals
        [ResponseType(typeof(Festival))]
        public async Task<IHttpActionResult> PostFestival(Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Festival.Add(festival);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = festival.Id }, festival);
        }

        // DELETE: api/Festivals/5
        [ResponseType(typeof(int))]
        [Authorize(Roles = "Организатор")]
        public async Task<IHttpActionResult> DeleteFestival(int id)
        {
            Festival festival = await db.Festival.FindAsync(id);
            if (festival == null)
            {
                return NotFound();
            }

            db.Festival.Remove(festival);
            db.User
                .First(u => u.Login == HttpContext.Current.User.Identity.Name)
                .UserRating.Add(new UserRating
                {
                    CountOfStars = 3
                });
            await db.SaveChangesAsync();

            return Ok(festival.Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FestivalExists(int id)
        {
            return db.Festival.Count(e => e.Id == id) > 0;
        }
    }
}