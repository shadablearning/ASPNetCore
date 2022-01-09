using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ContactManager.Authorization;

namespace ContactManager.Pages.Contacts
{
    public class CreateModel : DI_BasePageModel
    {
        private readonly ContactManager.Data.ApplicationDbContext _context;

        public CreateModel(
           ApplicationDbContext context,
           IAuthorizationService authorizationService,
           UserManager<IdentityUser> userManager)
           : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Contact.OwnerID = UserManager.GetUserId(User);

            var isAuthorized=await AuthorizationService.AuthorizeAsync(
                                                            User,Contact,
                                                            ContactOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }


            _context.Contact.Add(Contact);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
