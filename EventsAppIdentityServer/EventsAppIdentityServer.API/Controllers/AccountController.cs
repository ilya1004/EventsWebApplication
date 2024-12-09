//using EventsAppIdentityServer.Domain.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace EventsAppIdentityServer.API.Controllers;

//[ApiController]
//[Route("[controller]")]
//public class AccountController : ControllerBase
//{
//    private readonly UserManager<AppUser> _userManager;
//    private readonly SignInManager<AppUser> _signInManager;
//    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
//    {
//        _userManager = userManager;
//        _signInManager = signInManager;
//    }

//    [HttpGet]
//    [Route("Login")]
//    public IActionResult Login(string returnUrl)
//    {
//        //_signInManager.


//        return RedirectPermanent(returnUrl);
//    }
//}
