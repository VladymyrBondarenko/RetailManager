using Autofac;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.Repositories;
using System.Threading.Tasks;
using System.Web.Http;
using static RMDataManager.Startup;

namespace RMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserData _userData;

        public UserController()
        {
            _userData = ServiceTuner.Resolve<IUserData>();
        }

        public async Task<IHttpActionResult> GetCurrentUser()
        {
            var user = await _userData.GetUserById(
                RequestContext.Principal.Identity.GetUserId());
            return Ok(user);
        }
    }
}
