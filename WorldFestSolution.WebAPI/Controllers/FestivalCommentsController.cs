using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WorldFestSolution.WebAPI.Models.Entities;
using WorldFestSolution.WebAPI.Models.Filters;
using WorldFestSolution.WebAPI.Models.Serialized;

namespace WorldFestSolution.WebAPI.Controllers
{
    public class FestivalCommentsController : ApiController
    {
        private readonly WorldFestBaseEntities db = new WorldFestBaseEntities();
        private readonly SwearingFilter swearingFilter = new SwearingFilter();

        // GET: api/FestivalComments
        public IQueryable<FestivalComment> GetFestivalComment()
        {
            return db.FestivalComment;
        }

        // GET: api/FestivalComments/5
        [ResponseType(typeof(SerializedComment))]
        [Authorize(Roles = "Участник, Организатор")]
        public async Task<IHttpActionResult> GetFestivalComment(int id)
        {
            FestivalComment festivalComment = await db
                .FestivalComment
                .FindAsync(id);
            if (festivalComment == null)
            {
                return NotFound();
            }

            return Ok(
                new SerializedComment(festivalComment));
        }

        // PUT: api/FestivalComments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFestivalComment(int id, FestivalComment festivalComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != festivalComment.Id)
            {
                return BadRequest();
            }

            db.Entry(festivalComment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FestivalCommentExists(id))
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

        // POST: api/FestivalComments
        [ResponseType(typeof(SerializedComment))]
        [Authorize(Roles = "Участник, Организатор")]
        public async Task<IHttpActionResult> PostFestivalComment(FestivalComment festivalComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (swearingFilter.IsTextContainsSwearing(festivalComment.Text))
            {
                return Content(HttpStatusCode.Forbidden,
                               "Текст содержит нецензурную брань");
            }

            festivalComment.CreationDateTime = System.DateTime.Now;
            festivalComment.UserId = db.User
                        .First(u =>
                            u.Login == HttpContext.Current.User.Identity.Name)
                        .Id;

            db.FestivalComment.Add(festivalComment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi",
                                  new { id = festivalComment.Id },
                                  new SerializedComment(festivalComment));
        }

        // DELETE: api/FestivalComments/5
        [ResponseType(typeof(string))]
        [Authorize(Roles = "Участник, Организатор")]
        public async Task<IHttpActionResult> DeleteFestivalComment(int id)
        {
            FestivalComment festivalComment = await db.FestivalComment.FindAsync(id);
            if (festivalComment == null)
            {
                return NotFound();
            }

            bool isUserIsOwnerOfComment = festivalComment.User.Login
                                          == HttpContext.Current.User.Identity.Name;
            bool isUserOwnerOfFestivalCommentRelatedTo =
                festivalComment
                    .Festival
                    .User
                    .First(u => u.UserType.Title == "Организатор")
                    .Login == HttpContext.Current.User.Identity.Name;
            if (isUserIsOwnerOfComment || isUserOwnerOfFestivalCommentRelatedTo)
            {
                db.FestivalComment.Remove(festivalComment);
                await db.SaveChangesAsync();

                return Ok("Комментарий удалён");
            }
            else
            {
                return Content(HttpStatusCode.Forbidden,
                               "У вас недостаточно привилегий для удаления");
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

        private bool FestivalCommentExists(int id)
        {
            return db.FestivalComment.Count(e => e.Id == id) > 0;
        }
    }
}