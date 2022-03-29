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

        // GET: api/Festivals?isRelatedToMe=false
        [Authorize(Roles = "Участник, Организатор")]
        [ResponseType(typeof(IEnumerable<SerializedFestival>))]
        public IEnumerable<SerializedFestival> GetFestival(bool isRelatedToMe = false)
        {
            if (isRelatedToMe)
            {
                if (HttpContext.Current.User.IsInRole("Организатор"))
                {
                    return db.User
                        .First(u => u.Login == HttpContext.Current.User.Identity.Name)
                        .Festival
                        .Where(f => f.User.First().Login == HttpContext.Current.User.Identity.Name)
                        .ToList()
                        .ConvertAll(f => new SerializedFestival(f));
                }
                else
                {
                    return db.User
                     .First(u => u.Login == HttpContext.Current.User.Identity.Name)
                     .Festival
                     .ToList()
                     .ConvertAll(f => new SerializedFestival(f));
                }
            }
            return db.Festival
                .ToList()
                .ConvertAll(f => new SerializedFestival(f));
        }

        // GET: api/Festivals/5
        [ResponseType(typeof(SerializedFestival))]
        public async Task<IHttpActionResult> GetFestival(int id)
        {
            Festival festival = await db.Festival.FindAsync(id);
            if (festival == null)
            {
                return NotFound();
            }

            return Ok(
                new SerializedFestival(festival));
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
        [Authorize(Roles = "Организатор")]
        public async Task<IHttpActionResult> PostFestival(Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User organizer = db.User.First(u =>
                u.Login == HttpContext.Current.User.Identity.Name);
            festival.User.Add(organizer);
            db.Festival.Add(festival);

            await db.SaveChangesAsync();

            return CreatedAtRoute(
                "DefaultApi",
                new { id = festival.Id },
                new SerializedFestival(festival));
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

        // GET: api/Festivals/toggleParticipateState?festivalId=5
        [HttpGet]
        [ResponseType(typeof(bool))]
        [Authorize(Roles = "Участник")]
        [Route("api/festivals/toggleparticipatestate")]
        public async Task<IHttpActionResult> ToggleParticipateState(int festivalId)
        {
            Festival festival = await db.Festival.FindAsync(festivalId);
            if (festival == null)
            {
                return NotFound();
            }
            User user = await db.User
                .FirstAsync(u =>
                    u.Login == HttpContext.Current.User.Identity.Name);
            if (festival.User.Any(u => u.Id == user.Id))
            {
                festival.User.Remove(user);
                await db.SaveChangesAsync();
                return Ok(false);
            }
            else
            {
                festival.User.Add(user);
                await db.SaveChangesAsync();
                return Ok(true);
            }
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