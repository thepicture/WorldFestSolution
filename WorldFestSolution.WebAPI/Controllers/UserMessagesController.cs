using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WorldFestSolution.WebAPI.Models.Entities;
using WorldFestSolution.WebAPI.Models.Serialized;

namespace WorldFestSolution.WebAPI.Controllers
{
    public class UserMessagesController : ApiController
    {
        private readonly WorldFestBaseEntities db = new WorldFestBaseEntities();

        // GET: api/UserMessages
        public IQueryable<UserMessage> GetUserMessage()
        {
            return db.UserMessage;
        }

        // GET: api/UserMessages
        [ResponseType(typeof(List<SerializedUserMessage>))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDialog(int receiverId)
        {
            List<SerializedUserMessage> messages = db.UserMessage
                .Where(m => m.SenderId == Identity.Id
                            && m.ReceiverId == receiverId
                            || m.SenderId == receiverId
                            && m.ReceiverId == Identity.Id)
                .ToList()
                .ConvertAll(m =>
                {
                    return new SerializedUserMessage(m);
                });

            return Ok(messages);
        }

        // GET: api/UserMessages/5
        [ResponseType(typeof(UserMessage))]
        public async Task<IHttpActionResult> GetUserMessage(int id)
        {
            UserMessage userMessage = await db.UserMessage.FindAsync(id);
            if (userMessage == null)
            {
                return NotFound();
            }

            return Ok(userMessage);
        }

        // PUT: api/UserMessages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserMessage(int id, UserMessage userMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userMessage.Id)
            {
                return BadRequest();
            }

            db.Entry(userMessage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMessageExists(id))
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

        // POST: api/UserMessages
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostUserMessage(UserMessage userMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userMessage.SenderId = Identity.Id;
            userMessage.PostDateTime = DateTime.Now;

            db.UserMessage.Add(userMessage);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.Created);
        }

        // DELETE: api/UserMessages/5
        [ResponseType(typeof(UserMessage))]
        public async Task<IHttpActionResult> DeleteUserMessage(int id)
        {
            UserMessage userMessage = await db.UserMessage.FindAsync(id);
            if (userMessage == null)
            {
                return NotFound();
            }

            db.UserMessage.Remove(userMessage);
            await db.SaveChangesAsync();

            return Ok(userMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserMessageExists(int id)
        {
            return db.UserMessage.Count(e => e.Id == id) > 0;
        }
    }
}