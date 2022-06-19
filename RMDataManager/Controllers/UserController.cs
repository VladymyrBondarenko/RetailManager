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
                           new 
                           {
                               Key = r.RoleId,
                               Value = roles.FirstOrDefault(role => role.Id == r.RoleId)?.Name
                           }
                        ).ToDictionary(x => x.Key, x => x.Value)
                    }).ToList();

                return appUserModels;
            }
        }

        [HttpGet]
        [Route("Admin/GetAllRoles")]
        [Authorize(Roles = "Admin")]
        public Dictionary<string, string> GetAllRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;
            }
        }

        [HttpPost]
        [Route("Admin/AddToRole")]
        [Authorize(Roles = "Admin")]
        public async Task AddUserToRole(UserRolePairModel userRolePairModel)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                await userManager.AddToRoleAsync(userRolePairModel.UserId, userRolePairModel.RoleName);
            }
        }

        [HttpPost]
        [Route("Admin/RemoveFromRole")]
        [Authorize(Roles = "Admin")]
        public async Task RemoveRoleFromUser(UserRolePairModel userRolePairModel)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                await userManager.RemoveFromRoleAsync(userRolePairModel.UserId, userRolePairModel.RoleName);
            }
        }
    }
}