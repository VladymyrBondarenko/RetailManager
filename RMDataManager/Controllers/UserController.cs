using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using RMDataManager.Library.Repositories;
using RMDataManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            var user = await _userData.GetUserById(
                RequestContext.Principal.Identity.GetUserId());
            return Ok(user.First());
        }

        [HttpGet]
        [Route("Admin/GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            using(var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                var appUserModels =
                    users.Select(u => 
                    new ApplicationUserModel
                    {
                        Id = u.Id,
                        EmailAddress = u.Email,
                        UserRoles = u.Roles.Select(r =>
                            new KeyValuePair<string, string>(
                                r.RoleId, 
                                roles.FirstOrDefault(role => role.Id == r.RoleId)?.Name)
                        ).ToDictionary(x => x.Key, x => x.Value)
                    }).ToList();

                return appUserModels;
            }
        }
    }
}
