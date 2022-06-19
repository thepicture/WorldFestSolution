using System;
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
        private const int CountOfStarsForDeletingFestival = 3;
        private readonly WorldFestBaseEntities db = new WorldFestBaseEntities();

        // GET: api/Festivals?isRelatedToMe=false
        [Authorize]
        [ResponseType(typeof(IEnumerable<SerializedFestival>))]
        public IEnumerable<SerializedFestival> GetFestival(bool isRelatedToMe = false)
        {
            if (isRelatedToMe)
            {
                if (HttpContext.Current.User.IsInRole("Организатор"))
                {
                    List<SerializedFestival> festivals = db.User
                        .First(u => u.Login.Equals(HttpContext.Current.User.Identity.Name,
                                                   StringComparison.OrdinalIgnoreCase))
                        .Festival
                        .ToList()
                        .ConvertAll(f => new SerializedFestival(f));
                    return festivals;
                }
                else
                {
                    List<SerializedFestival> festivals = db.Festival
                        .Where(f =>
                            f.User.Any(u =>
                                u.Login.Equals(HttpContext.Current.User.Identity.Name,
                                               StringComparison.OrdinalIgnoreCase)))
                        .ToList()
                        .ConvertAll(f => new SerializedFestival(f));
                    return festivals;
                }
            }
            else
            {
                List<SerializedFestival> festivals = db.Festival
                    .ToList()
                    .ConvertAll(f =>
                    {
                        return new SerializedFestival(f);
                    });
                return festivals;
            }
        }

        // GET: api/Festivals/popularity
        [Authorize]
        [ResponseType(typeof(IEnumerable<SerializedFestivalPopularity>))]
        [Route("api/Festivals/popularity")]
        [HttpGet]
        public IEnumerable<SerializedFestivalPopularity> GetFestivalPopularity()
        {
            return db.Festival
                .ToList()
                .ConvertAll(f =>
                {
                    return new SerializedFestivalPopularity(f);
                });
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
            if (festival.Id == 0)
            {
                db.Festival.Add(festival);
            }
            else
            {
                DbEntityEntry<Festival> festivalFromDb = db
                    .Entry(
                        db.Festival.Find(festival.Id));
                festivalFromDb
                    .CurrentValues
                    .SetValues(festival);
                foreach (FestivalProgram postedProgram
                    in festival.FestivalProgram)
                {
                    FestivalProgram program = festivalFromDb
                        .Entity
                        .FestivalProgram
                        .FirstOrDefault(p => p.Id == postedProgram.Id);
                    if (program == null)
                    {
                        festivalFromDb.Entity.FestivalProgram.Add(postedProgram);
                    }
                    else
                    {
                        if (postedProgram.IsDeletedLocally)
                        {
                            db.FestivalProgram.Remove(program);
                        }
                        else
                        {
                            db.Entry(program).CurrentValues.SetValues(postedProgram);
                        }
                    }
                }
            }

            await db.SaveChangesAsync();

            return CreatedAtRoute(
                "DefaultApi",
                new { id = festival.Id },
                new SerializedFestival(
                    db.Festival.Find(festival.Id)));
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
                    CountOfStars = CountOfStarsForDeletingFestival
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

        // GET: api/festivalParticipants?festivalId=1
        [HttpGet]
        [ResponseType(typeof(List<User>))]
        [Authorize(Roles = "Организатор")]
        [Route("api/festivalParticipants")]
        public async Task<IHttpActionResult> GetFestivalParticipants(int festivalId)
        {
            Festival festival = await db.Festival.FindAsync(festivalId);
            if (festival == null)
            {
                return NotFound();
            }

            List<SerializedUser> serializedUsers = festival.User
                .Where(u => u.UserType.Title == "Участник")
                .ToList()
                .ConvertAll(u => new SerializedUser(u));
            return Ok(serializedUsers);
        }

        // GET: api/Festivals/1/users
        [HttpDelete]
        [ResponseType(typeof(List<User>))]
        [Authorize(Roles = "Организатор")]
        [Route("api/deleteParticipant")]
        public async Task<IHttpActionResult> DeleteParticipant(int participantId, int festivalId)
        {
            Festival festival = await db.Festival.FindAsync(festivalId);
            if (festival == null)
            {
                return NotFound();
            }
            festival.User.Remove(festival.User.First(u => u.Id == participantId));
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
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