using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Authorization
{
    //The ContactIsOwnerAuthorizationHandler verifies that the user acting on a resource owns the resource.
    //An authorization handler is responsible for the evaluation of a requirement's properties.
    //The authorization handler evaluates the requirements against a provided AuthorizationHandlerContext to determine if access is allowed.
    public class ContactIsOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement,Contact>
    {
        UserManager<IdentityUser> _userManager;
        public ContactIsOwnerAuthorizationHandler(UserManager<IdentityUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
        {
            if(context.User==null || resource==null)
            {
                return Task.CompletedTask;
            }

            // If not asking for CRUD permission, return
            if(requirement.Name!= Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName
                )
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
