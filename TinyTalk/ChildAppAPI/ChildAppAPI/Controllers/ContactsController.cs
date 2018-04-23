using System;
using System.Linq;
using System.Web.Http;

namespace ChildAppAPI.Controllers
{
    public class ContactsController : ApiController
    {
        [HttpGet]
        public userContacts GetChildContacts([FromUri]string email)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var userContacts = db.contacts
                    .Where(b => b.email.Equals(email))
                    .FirstOrDefault();
                    return userContacts;
                }
            }
            catch (Exception ex)
            {
                return (null);
            }
        }

        [HttpGet]
        public string GetParentContact([FromUri]string Parentemail, [FromUri]bool isParent)
        {
            if (!isParent)
            {
                return ("This is not a parent");
            }
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var userContacts = db.contacts
                    .Where(b => b.father.Equals(Parentemail) || b.mother.Equals(Parentemail))
                    .FirstOrDefault();
                    return (userContacts.email);
                }
            }
            catch (Exception ex)
            {
                return (null);
            }
        }

        [HttpPost]
        public bool AddChildContacts([FromBody] string[] emails)
        {
            //position 0 for user type : parent or child
            //position 1 will be for the user email itself
            if (emails[0].ToLower() != "child")
            {
                return false;
            }
            string userEmail = emails[1];
            userContacts contacts = new userContacts
            {
                email = userEmail,
                father = emails[2],
                mother = emails[3],
                friend = emails[4]
            };
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var userContacts = db.contacts
                    .Where(b => b.email.Equals(contacts.email))
                    .FirstOrDefault();
                    if(userContacts == null)
                    {
                        var userNewContacts = db.contacts.Add(contacts);
                        db.SaveChanges();
                        return true;
                    }
                    userContacts.father = contacts.father;
                    userContacts.mother = contacts.mother;
                    userContacts.friend = contacts.friend;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return (false);
            }
        }

    }
}