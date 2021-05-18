using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Uwierzytelanie.Data;
using Uwierzytelanie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Uwierzytelanie.Pages
{
    [Authorize]
    public class ClassModel : PageModel
    {
        SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _userManager;
        private readonly LiczbaContext _context;
        public ClassModel(LiczbaContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [BindProperty]
        public Class Class { get; set; }
        public IEnumerable<Class> Wyszukania { get; set; }
        public string temp { get; set; }
        public async Task OnGetAsync()
        {
            Wyszukania = await _context.Class.OrderByDescending(m => m.Data).Take(20).ToListAsync();
            temp = _userManager.GetUserId(HttpContext.User);
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if ((Class.UserID == null) || (Class.UserID == temp))
            {
                if (id == null)
                {
                    return NotFound();
                }
                Class = await _context.Class.FindAsync(id);
                if (Class != null)
                {
                    _context.Class.Remove(Class);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToPage("./Wyszukania");
        }
    }
}