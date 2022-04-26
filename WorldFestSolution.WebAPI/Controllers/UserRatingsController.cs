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
    public class UserRatingsController : ApiController
    {
        private WorldFestBaseEntities db = new WorldFestBaseEntities();

        // GET: api/UserRatings
        public IQueryable<UserRating> GetUserRating()
        {
            return db.UserRating;
        }

        // GET: api/UserRatings/5
        [ResponseType(typeof(UserRating))]
        public async Task<IHttpActionResult> GetUserRating(int id)
        {
            UserRating userRating = await db.UserRating.FindAsync(id);
            if (userRating == null)
            {
                return NotFound();
            }

            return Ok(userRating);
        }

        // PUT: api/UserRatings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserRating(int id, UserRating userRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userRating.Id)
            {
                return BadRequest();
            }

            db.Entry(userRating).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRatingExists(id))
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

        // POST: api/UserRatings
        [ResponseType(typeof(SerializedUserRating))]
        [Authorize]
        public async Task<IHttpActionResult> PostUserRating(UserRating userRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User rater = db.User.First(u =>
              u.Login == HttpContext.Current.User.Identity.Name);
            userRating.RaterId = rater.Id;

            db.UserRating.Add(userRating);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi",
                                  new { id = userRating.Id },
                                  new SerializedUserRating(userRating));
        }

        // DELETE: api/UserRatings/5
        [ResponseType(typeof(UserRating))]
        public async Task<IHttpActionResult> DeleteUserRating(int id)
        {
            UserRating userRating = await db.UserRating.FindAsync(id);
            if (userRating == null)
            {
                return NotFound();
            }

            db.UserRating.Remove(userRating);
            await db.SaveChangesAsync();

            return Ok(userRating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserRatingExists(int id)
        {
            return db.UserRating.Count(e => e.Id == id) > 0;
        }
    }
}