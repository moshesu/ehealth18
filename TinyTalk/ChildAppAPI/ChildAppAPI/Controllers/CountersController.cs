using System;
using System.Linq;
using System.Web.Http;

namespace ChildAppAPI.Controllers
{
    public class CountersController : ApiController
    {
        [HttpPatch]
        public bool UpdateUserCounter([FromBody] symbol data)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    db.symbols.Add(data);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet]
        public symbol[] GetUserSymbolUsage([FromUri] string email, [FromUri] string symbolName)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var userSymbol = db.symbols.Where(u => u.email.Equals(email) && u.symbolName.Equals(symbolName));

                    if (userSymbol != null)
                    {
                        return userSymbol.ToArray();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public symbol[] GetUserAllSymbolsUsage([FromUri] string email)
        {
            try
            {
                using (APP_DBEntities db = new APP_DBEntities())
                {
                    var userSymbol = db.symbols.Where(u => u.email.Equals(email));

                    if (userSymbol != null)
                    {
                        return userSymbol.ToArray();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}