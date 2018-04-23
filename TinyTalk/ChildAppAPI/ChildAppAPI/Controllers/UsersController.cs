using System;
using System.Linq;
using System.Web.Http;

namespace ChildAppAPI.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet]
        public user GetUser([FromUri]string email)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var user = db.users
                    .Where(b => b.email.Equals(email))
                    .FirstOrDefault();

                    return user;
                }
            }
            catch (Exception ex)
            {
                return (null);
            }
        }

        [HttpDelete]
        public bool DeleteUser([FromUri] string email)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var user = db.users
                    .Where(b => b.email.Equals(email))
                    .FirstOrDefault();
                    db.users.Remove(user);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public bool CreateUser([FromBody] user user)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    db.users.Add(user);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        public user[] GetTeacherUsers([FromUri]string garden)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var users = db.users
                    .Where(b => b.gardenname.Equals(garden) && b.type.ToLower().Equals("child"));
                    user[] res = users.ToArray();
                    return res;
                }
            }
            catch (Exception)
            {
                return (null);
            }
        }

        [HttpGet]
        public user GetTeacher([FromUri] string email, [FromUri] string type)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var user = db.users
                    .Where(b => b.email.Equals(email))
                    .FirstOrDefault();
                    var garden = user.gardenname;
                    var teacher = db.users
                    .Where(b => b.type.ToLower().Equals("teacher") && b.gardenname.Equals(garden))
                    .FirstOrDefault();

                    return teacher;
                }
            }
            catch (Exception)
            {
                return (null);
            }
        }
    }
}
