using System;
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
        public IQueryable<User> GetUser()
        {
            return db.User;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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

        // GET: api/Users/Authenticate
        [ResponseType(typeof(SerializedUser))]
        [Route("api/users/authenticate")]
        [Authorize(Roles = "Организатор, Участник")]
        [HttpGet]
        public async Task<IHttpActionResult> Authenticate()
        {
            string login = HttpContext.Current.User.Identity.Name;
            User user = await db.User.FirstAsync(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            return Ok(
                new SerializedUser(user));
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

            PasswordHashGenerator passwordHashGenerator =
                new PasswordHashGenerator();
            byte[] salt = Guid.NewGuid()
                    .ToByteArray();
            byte[] passwordHash = passwordHashGenerator
                .Encrypt(serializedUser.Password, salt);
            User user = new User
            {
                Login = serializedUser.Login,
                PasswordHash = passwordHash,
                Salt = salt,
                FirstName = serializedUser.FirstName,
                LastName = serializedUser.LastName,
                Patronymic = serializedUser.Patronymic,
                UserTypeId = serializedUser.UserTypeId,
                Image = serializedUser.Image
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
            PasswordHashGenerator passwordHashGenerator =
                new PasswordHashGenerator();

            byte[] oldPasswordHash = passwordHashGenerator.Encrypt(
                credentials.OldPassword,
                user.Salt);
            if (!Enumerable.SequenceEqual(oldPasswordHash, user.PasswordHash))
            {
                return Content(
                    HttpStatusCode.Forbidden,
                    "Вы ввели неверный старый пароль");
            }

            byte[] salt = Guid.NewGuid()
                  .ToByteArray();
            byte[] passwordHash = passwordHashGenerator
               .Encrypt(credentials.NewPassword, salt);

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
    }
}