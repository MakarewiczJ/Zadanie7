using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uwierzytelanie.Data;
using Uwierzytelanie.Models;

namespace Uwierzytelanie.Pages
{
    public class IndexModel : PageModel
    {
        SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _userManager;
        private readonly LiczbaContext _context;

        private readonly ILogger<IndexModel> _logger;



        [BindProperty]
        public Class Class { get; set; }


        public IndexModel(ILogger<IndexModel> logger, LiczbaContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                Class.FizzBuzz(Class.Liczba);
                Class.UserID = null;
                if (_signInManager.IsSignedIn(User))
                {
                    Class.UserID = _userManager.GetUserId(HttpContext.User);
                }
                _context.Class.Add(Class);
                _context.SaveChanges();
                HttpContext.Session.SetString("SessionClass",
                JsonConvert.SerializeObject(Class));
            }
        }
    }
}
