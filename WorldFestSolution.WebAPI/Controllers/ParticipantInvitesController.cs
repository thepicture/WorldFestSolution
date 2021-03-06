using System.Collections.Generic;
using System.Data;
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
    public class ParticipantInvitesController : ApiController
    {
        private readonly WorldFestBaseEntities db = new WorldFestBaseEntities();

        // GET: api/ParticipantInvites
        [Authorize(Roles = "Организатор")]
        public IHttpActionResult GetParticipantInvite()
        {
            List<ParticipantInvite> invites = db.ParticipantInvite.ToList();
            return Ok(invites);
        }

        // GET: api/ParticipantInvites?festivalId=1
        [ResponseType(typeof(List<SerializedInvite>))]
        [Authorize(Roles = "Организатор, Участник")]
        public IHttpActionResult GetParticipantInvite(
            int festivalId)
        {
            List<SerializedInvite> invites = new List<SerializedInvite>();
            if (HttpContext.Current.User.IsInRole("Организатор"))
            {
                List<User> users = db.User
                    .Where(u =>
                        !u.Festival.Select(f => f.Id)
                                   .Contains(festivalId))
                    .Where(u => u.UserType.Title == "Участник")
                    .Where(u => u.IsWantsInvites)
                    .Where(u => u.Login != HttpContext.Current.User.Identity.Name)
                    .ToList();
                invites = users.ConvertAll(u =>
                {
                    ParticipantInvite exitingInvite = u
                    .ParticipantInvite
                    .FirstOrDefault(pi =>
                    {
                        return pi.User1.Login == HttpContext.Current.User
                        .Identity.Name;
                    });
                    SerializedUser serializedUser = new SerializedUser(u);
                    return new SerializedInvite
                    {
                        Id = exitingInvite?.Id ?? 0,
                        ParticipantId = u.Id,
                        FestivalId = festivalId,
                        IsAccepted = exitingInvite != null
                                     && exitingInvite.IsAccepted,
                        IsSent = exitingInvite != null,
                        Participant = serializedUser,
                        IsParticipantWantsInvites = u.IsWantsInvites
                    };
                });
            }
            else
            {
                User currentParticipant = db.User.First(u => u.Login == HttpContext.Current.User.Identity.Name);
                invites = currentParticipant
                    .ParticipantInvite
                    .Where(i => !i.IsLookedByParticipant)
                    .Select(i =>
                    {
                        return new SerializedInvite
                        {
                            Id = i.Id,
                            OrganizerId = i.OrganizerId,
                            ParticipantId = i.ParticipantId,
                            FestivalId = i.FestivalId,
                            IsAccepted = i.IsAccepted,
                            Organizer = new SerializedUser(i.User1),
                            FestivalTitle = i.Festival.Title,
                            Festival = new SerializedFestival(i.Festival)
                        };
                    })
                    .ToList();
            }
            return Ok(invites);
        }

        // PUT: api/ParticipantInvites/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParticipantInvite
            (int id, ParticipantInvite participantInvite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participantInvite.Id)
            {
                return BadRequest();
            }

            db.Entry(participantInvite).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantInviteExists(id))
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

        // POST: api/ParticipantInvites
        [ResponseType(typeof(SerializedInvite))]
        [Authorize(Roles = "Организатор, Участник")]
        public async Task<IHttpActionResult> PostParticipantInvite
            (ParticipantInvite participantInvite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (participantInvite.Id == 0)
            {

                participantInvite.OrganizerId =
                    db.User
                    .First(u => u.Login == HttpContext.Current.User.Identity.Name)
                    .Id;

                db.ParticipantInvite.Add(participantInvite);
            }
            else
            {
                db.ParticipantInvite.Find(participantInvite.Id)
                    .IsLookedByParticipant = true;
                db.ParticipantInvite.Find(participantInvite.Id)
                    .IsAccepted = participantInvite.IsAccepted;
                db.User
                   .First(u => u.Login == HttpContext.Current.User.Identity.Name)
                   .Festival.Add(
                    db.Festival.Find(participantInvite.FestivalId));
            }
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi",
                                  new { id = participantInvite.Id },
                                  participantInvite.Id);
        }

        // DELETE: api/ParticipantInvites/5
        [ResponseType(typeof(ParticipantInvite))]
        public async Task<IHttpActionResult> DeleteParticipantInvite(int id)
        {
            ParticipantInvite participantInvite = await db.ParticipantInvite.FindAsync(id);
            if (participantInvite == null)
            {
                return NotFound();
            }

            db.ParticipantInvite.Remove(participantInvite);
            await db.SaveChangesAsync();

            return Ok(participantInvite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParticipantInviteExists(int id)
        {
            return db.ParticipantInvite.Count(e => e.Id == id) > 0;
        }
    }
}