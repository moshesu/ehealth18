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
    public class DefaultTrainingProgramController : TableController<DefaultTrainingProgram>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Personal_TrainerContext context = new Personal_TrainerContext();
            DomainManager = new EntityDomainManager<DefaultTrainingProgram>(context, Request);
        }

        // GET tables/Exercise
        public IQueryable<DefaultTrainingProgram> GetAllDefaultTrainingPrograms()
        {
            return Query();
        }

        // GET tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<DefaultTrainingProgram> GetDefaultTrainingProgram(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<DefaultTrainingProgram> PatchDefaultTrainingProgram(string id, Delta<DefaultTrainingProgram> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Exercise
        public async Task<IHttpActionResult> PostDefaultTrainingProgram(DefaultTrainingProgram item)
        {
            DefaultTrainingProgram current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDefaultTrainingProgram(string id)
        {
            return DeleteAsync(id);
        }
    }
}