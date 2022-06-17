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
using WorldFestSolution.WebAPI.Models.Security;
using WorldFestSolution.WebAPI.Models.Serialized;
using WorldFestSolution.XamarinApp.Models.Serialized;

namespace WorldFestSolution.WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private readonly WorldFestBaseEntities db =
            new WorldFestBaseEntities();

        // GET: api/Users
        [ResponseType(typeof(List<SerializedUser>))]
        [Authorize]
        public IHttpActionResult GetUser()
        {
            List<SerializedUser> users = db.User
                .ToList()
                .ConvertAll(u => new SerializedUser(u));
            return base.Ok(users);
        }

        // GET: api/Users/5
        [ResponseType(typeof(SerializedUser))]
        [Authorize]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(
                new SerializedUser(user));
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // PUT: api/Users
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Участник")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.User.Find(user.Id).IsWantsInvites = user.IsWantsInvites;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.Id == id) > 0;
        }

        // POST: api/Users/login
        [ResponseType(typeof(SerializedUser))]
        [Route("api/users/login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> LogIn(SerializedUser loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = await db.User.FirstOrDefaultAsync(u =>
                u.Login.Equals(loginUser.Login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Unauthorized();
            }
            if (Enumerable.SequenceEqual(user.PasswordHash,
                                         PasswordHashGenerator.Encrypt(loginUser.Password,
                                                                       user.Salt)))
            {
                return Ok(
                    new SerializedUser(user));
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: api/Users/Register
        [AllowAnonymous]
        [HttpPost]
        [Route("api/users/register")]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> Register
            (SerializedUser serializedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool isUserExisting = await db.User
                .AnyAsync(u =>
                serializedUser.Login.Equals(u.Login,
                              StringComparison.OrdinalIgnoreCase));
            if (isUserExisting)
            {
                return Conflict();
            }

            byte[] salt = Guid.NewGuid()
                    .ToByteArray();
            byte[] passwordHash = PasswordHashGenerator.Encrypt(serializedUser.Password,
                                                                salt);
            User user = new User
            {
                Login = serializedUser.Login,
                PasswordHash = passwordHash,
                Salt = salt,
                FirstName = serializedUser.FirstName,
                LastName = serializedUser.LastName,
                Patronymic = serializedUser.Patronymic,
                UserTypeId = serializedUser.UserTypeId,
                Image = serializedUser.Image,
                Is18OrMoreYearsOld = serializedUser.Is18OrMoreYearsOld.Value
            };

            db.User.Add(user);

            await db.SaveChangesAsync();

            return Content(HttpStatusCode.Created, user.Id);
        }

        // POST: api/Users/changepassword
        [HttpPost]
        [Route("api/users/changepassword")]
        [ResponseType(typeof(string))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ChangePassword
            (ChangePasswordCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string login = credentials.Login;
            User user = await db.User.FirstAsync(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            byte[] oldPasswordHash = PasswordHashGenerator.Encrypt(credentials.OldPassword,
                                                                   user.Salt);
            if (!Enumerable.SequenceEqual(oldPasswordHash, user.PasswordHash))
            {
                return Content(
                    HttpStatusCode.Forbidden,
                    "Вы ввели неверный старый пароль");
            }

            byte[] salt = Guid.NewGuid()
                  .ToByteArray();
            byte[] passwordHash = PasswordHashGenerator.Encrypt(credentials.NewPassword,
                                                                salt);

            user.PasswordHash = passwordHash;
            user.Salt = salt;

            await db.SaveChangesAsync();

            return Content(HttpStatusCode.Created, "Пароль изменён");
        }

        // GET: api/Users/image
        [HttpGet]
        [Route("api/users/image")]
        [Authorize]
        public async Task<IHttpActionResult> GetImage()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await db.User.FirstAsync(u =>
                u.Login == HttpContext.Current.User.Identity.Name);
            return Ok(user.Image);
        }

        // POST: api/Users/image
        [HttpPost]
        [Route("api/users/image")]
        [Authorize]
        public async Task<IHttpActionResult> PostImage(byte[] imageBytes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await db.User.FirstAsync(u =>
                u.Login == HttpContext.Current.User.Identity.Name);
            user.Image = imageBytes;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/Users/myInvites
        [HttpGet]
        [Route("api/users/myInvites")]
        [Authorize]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetCountOfMyInvites()
        {
            User user = db.User.FirstOrDefault(u =>
                u.Login == HttpContext.Current.User.Identity.Name);
            if (user != null)
            {
                int countOfMyInvites = user.ParticipantInvite.Count;
                return Ok(countOfMyInvites);
            }
            else
            {
                return NotFound();
            }
        }
    }
}