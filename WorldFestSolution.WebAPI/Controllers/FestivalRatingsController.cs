using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Controllers
{
    public class FestivalRatingsController : ApiController
    {
        private readonly WorldFestBaseEntities db = new WorldFestBaseEntities();

        // GET: api/FestivalRatings
        public IQueryable<FestivalRating> GetFestivalRating()
        {
            return db.FestivalRating;
        }

        // GET: api/FestivalRatings/5
        [ResponseType(typeof(FestivalRating))]
        public async Task<IHttpActionResult> GetFestivalRating(int id)
        {
            FestivalRating festivalRating = await db.FestivalRating.FindAsync(id);
            if (festivalRating == null)
            {
                return NotFound();
            }

            return Ok(festivalRating);
        }

        // PUT: api/FestivalRatings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFestivalRating(int id, FestivalRating festivalRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != festivalRating.Id)
            {
                return BadRequest();
            }

            db.Entry(festivalRating).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FestivalRatingExists(id))
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

        // POST: api/FestivalRatings
        [ResponseType(typeof(string))]
        [Authorize(Roles = "Участник")]
        public async Task<IHttpActionResult> PostFestivalRating(FestivalRating festivalRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User rater = db.User.First(u =>
              u.Login == HttpContext.Current.User.Identity.Name);
            festivalRating.RaterId = rater.Id;

            db.FestivalRating.Add(festivalRating);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi",
                                  new { id = festivalRating.Id },
                                  "Оценка сохранена");
        }

        // DELETE: api/FestivalRatings/5
        [ResponseType(typeof(FestivalRating))]
        public async Task<IHttpActionResult> DeleteFestivalRating(int id)
        {
            FestivalRating festivalRating = await db.FestivalRating.FindAsync(id);
            if (festivalRating == null)
            {
                return NotFound();
            }

            db.FestivalRating.Remove(festivalRating);
            await db.SaveChangesAsync();

            return Ok(festivalRating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FestivalRatingExists(int id)
        {
            return db.FestivalRating.Count(e => e.Id == id) > 0;
        }
    }
}