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
using WorldFestSolution.WebAPI.Models.Filters;
using WorldFestSolution.WebAPI.Models.Serialized;

namespace WorldFestSolution.WebAPI.Controllers
{
    public class FestivalCommentsController : ApiController
    {
        private readonly WorldFestBaseEntities db = new WorldFestBaseEntities();
        private readonly SwearingFilter swearingFilter = new SwearingFilter();
        private readonly TimeSpan TimeoutBeforeNextComment = TimeSpan.FromSeconds(30);

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

        // GET: api/FestivalComments?festivalId=1
        [ResponseType(typeof(SerializedComment))]
        [Authorize(Roles = "Участник, Организатор")]
        [Route("api/festivalComments")]
        [HttpGet]
        public IHttpActionResult GetFestivalCommentsByFestivalId(
            int festivalId)
        {
            List<SerializedComment> comments = db
                .Festival
                .Find(festivalId)
                .FestivalComment
                .ToList()
                .ConvertAll(c => new SerializedComment(c));
            return Ok(comments);
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
        [ResponseType(typeof(int))]
        [Authorize(Roles = "Участник, Организатор")]
        [Route("api/festivalComments")]
        [HttpPost]
        public async Task<IHttpActionResult> PostFestivalComment(
            FestivalComment festivalComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (swearingFilter.IsTextContainsSwearing(festivalComment.Text))
            {
                return Content(HttpStatusCode.Forbidden,
                               "Текст не должен содержать "
                               + "нецензурную лексику");
            }

            var lastComment = db
                .FestivalComment
                .OrderByDescending(fc => fc.CreationDateTime)
                .FirstOrDefault(fc =>
                    fc.User.Login == HttpContext.Current.User.Identity.Name);
            if (lastComment != null)
            {
                bool isUserIsSendingCommentsTooOften =
                    DateTime.Now - lastComment.CreationDateTime
                    < TimeoutBeforeNextComment;
                if (isUserIsSendingCommentsTooOften)
                {
                    return Content(HttpStatusCode.Forbidden,
                        "Вы не можете комментировать "
                        + "слишком часто");
                }
            }

            festivalComment.CreationDateTime = System.DateTime.Now;
            festivalComment.UserId = db.User
                        .First(u =>
                            u.Login == HttpContext.Current.User.Identity.Name)
                        .Id;

            db.FestivalComment.Add(festivalComment);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.Created);
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

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
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