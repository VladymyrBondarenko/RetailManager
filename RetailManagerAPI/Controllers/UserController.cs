using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RetailManagerAPI.Data;
using RetailManagerAPI.Models;
using RMDataManager.Library.Repositories;
using System.Security.Claims;

namespace RetailManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserData _userData;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IUserData userData, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userData = userData;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userData.GetUserById(
                User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(user.First());
        }

        [HttpGet]
        [Route("Admin/GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();

            var userRoles = from ur in _context.UserRoles
                                join r in _context.Roles on ur.RoleId equals r.Id
                                select new { ur.UserId, ur.RoleId, r.Name };

            var output = users.Select(user => new ApplicationUserModel
            {
                Id = user.Id,
                EmailAddress = user.Email,
                UserRoles = userRoles
                        .Where(x => x.UserId == user.Id)
                        .ToDictionary(x => x.RoleId, x => x.Name)
            }).ToList();
            
            return Ok(output);
        }

        [HttpGet]
        [Route("Admin/GetAllRoles")]
        [Authorize(Roles = "Admin")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [HttpPost]
        [Route("Admin/AddToRole")]
        [Authorize(Roles = "Admin")]
        public async Task AddUserToRole(UserRolePairModel userRolePairModel)
        {
            var user = await _userManager.FindByIdAsync(userRolePairModel.UserId);
            await _userManager.AddToRoleAsync(user, userRolePairModel.RoleName);
        }

        [HttpPost]
        [Route("Admin/RemoveFromRole")]
        [Authorize(Roles = "Admin")]
        public async Task RemoveRoleFromUser(UserRolePairModel userRolePairModel)
        {
            var user = await _userManager.FindByIdAsync(userRolePairModel.UserId);
            await _userManager.RemoveFromRoleAsync(user, userRolePairModel.RoleName);
        }
    }
}