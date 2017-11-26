using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Piranha.Areas.Manager.Controllers;
using Piranha.AspNetCore.Identity.EF;
using Piranha.AspNetCore.Identity.EF.Manager.Areas.Manager.Models;
using Piranha.Manager;

namespace Piranha.AspNetCore.Identity.EF.Manager.Area.Manager.Controllers
{
    [Area("Manager")]
    public class UserController : ManagerAreaControllerBase
    {
        private readonly AppUserRepository repo;
        private readonly UserManager<IdentityAppUser> userManager;

        public UserController(IApi api, AppUserRepository userRepository, UserManager<IdentityAppUser> userManager) : base(api)
        {
            this.repo = userRepository;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get the list view of the users.
        /// </summary>
        [Route("manager/users")]
        [Authorize(Policy = Permission.Users)]
        public IActionResult List()
        {
            var model = UserListModel.Get(repo, userManager);

            return View(model);
        }

        /// <summary>
        /// Gets the edit view for the user with the given id
        /// </summary>
        /// <param name="id"></param>
        [Route("manager/edit/user/{id?}")]
        [Authorize(Policy = Permission.UsersEdit)]
        public async Task<IActionResult> Edit(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
                return View(await UserEditModel.GetById(userManager, id));

            return View(UserEditModel.Create(userManager));
        }

        /// <summary>
        /// Saves the given model.
        /// </summary>
        /// <param name="model">The user model</param>
        [HttpPost]
        [Route("manager/user/save")]
        [Authorize(Policy = Permission.UsersSave)]
        public async Task<IActionResult> Save(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await model.Save(userManager);

                if (result.Item1)
                {
                    SuccessMessage("The user has been updated.");
                    return RedirectToAction("List");
                }
                    

                ErrorMessage(result.Item2);
                return View("Edit", model);

            }

            ErrorMessage("The user could not be saved.", false);
            return View("Edit", model);
        }

        /// <summary>
        /// Deletes the user of the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("manager/user/delete/{id}")]
        [Authorize(Policy = Permission.UsersDelete)]
        public async Task<IActionResult> Delete(string id)
        {
            if(string.IsNullOrEmpty(id))
                ErrorMessage($"No valid user for id: {id}", false);

            var user = await userManager.FindByIdAsync(id);
            if(user == null)
                ErrorMessage($"No valid user for id: {id}", false);
            try
            {
                await userManager.DeleteAsync(user);
                SuccessMessage("The user has been deleted");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorMessage($"Unaable to delete the user, error: {e.Message}");

                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// Updates the user claim
        /// </summary>
        /// <param name="model">The update claim model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("manager/user/claims/update")]
        [Authorize(Policy = Permission.UsersSave)]
        public async Task<IActionResult> Update(UserClaimModel model)
        {
            var claim = new Claim(model.ClaimValue, model.ClaimValue);

            if (string.IsNullOrEmpty(model.Id)) return Json(false);

            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null) return Json(false);

            if (model.Add)
            {
                if (userManager.GetClaimsAsync(user).Result.Any(x => x.Type == model.ClaimValue))
                    return Json(true);

                var result = await userManager.AddClaimAsync(user, claim);

                return Json(result.Succeeded);
            }
            else
            {
                var result = await userManager.RemoveClaimAsync(user, claim);

                return Json(result.Succeeded);
            }
            
        }
    }
}