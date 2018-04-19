using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Personal_TrainerService.DataObjects;
using Personal_TrainerService.Models;

namespace Personal_TrainerService.Controllers
{
    public class UserWorkoutController : TableController<UserWorkout>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Personal_TrainerContext context = new Personal_TrainerContext();
            DomainManager = new EntityDomainManager<UserWorkout>(context, Request);
        }

        // GET tables/Exercise
        public IQueryable<UserWorkout> GetAllUserWorkouts()
        {
            return Query();
        }

        // GET tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserWorkout> GetUserWorkout(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserWorkout> PatchUserWorkout(string id, Delta<UserWorkout> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Exercise
        public async Task<IHttpActionResult> PostUserWorkout(UserWorkout item)
        {
            UserWorkout current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserWorkout(string id)
        {
            return DeleteAsync(id);
        }
    }
}