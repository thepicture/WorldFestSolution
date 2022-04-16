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
using WorldFestSolution.WebAPI.Services;

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
        public IHttpActionResult GetParticipantInvite(
            int festivalId)
        {
            List<User> users = db.User
                .Where(u =>
                    !u.Festival.Select(f => f.Id)
                               .Contains(festivalId))
                .Where(u => u.UserType.Title == "Участник")
                .Where(u => u.Login != HttpContext.Current.User.Identity.Name)
                .ToList();
            List<SerializedInvite> invites = users.ConvertAll(u =>
            {
                ParticipantInvite exitingInvite = u
                .ParticipantInvite
                .FirstOrDefault(pi =>
                {
                    return pi.User.Login == HttpContext.Current.User
                    .Identity.Name;
                });
                SerializedUser serializedUser = new SerializedUser(u);
                serializedUser.Image = ImageResizerService.Resize(serializedUser.Image);
                return new SerializedInvite
                {
                    Id = exitingInvite?.Id ?? 0,
                    ParticipantId = u.Id,
                    FestivalId = festivalId,
                    IsAccepted = exitingInvite != null && exitingInvite.IsAccepted,
                    User = serializedUser
                };
            });
            return Ok(invites);
        }

        // PUT: api/ParticipantInvites/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParticipantInvite(int id, ParticipantInvite participantInvite)
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
        [ResponseType(typeof(ParticipantInvite))]
        public async Task<IHttpActionResult> PostParticipantInvite(ParticipantInvite participantInvite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ParticipantInvite.Add(participantInvite);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = participantInvite.Id }, participantInvite);
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