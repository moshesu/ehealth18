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
    public class DefaultExerciseController : TableController<DefaultExercise>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Personal_TrainerContext context = new Personal_TrainerContext();
            DomainManager = new EntityDomainManager<DefaultExercise>(context, Request);
        }

        // GET tables/Exercise
        public IQueryable<DefaultExercise> GetAllDefaultExercises()
        {
            return Query();
        }

        // GET tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<DefaultExercise> GetDefaultExercise(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<DefaultExercise> PatchDefaultExercise(string id, Delta<DefaultExercise> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Exercise
        public async Task<IHttpActionResult> PostDefaultExercise(DefaultExercise item)
        {
            DefaultExercise current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDefaultExercise(string id)
        {
            return DeleteAsync(id);
        }
    }
}