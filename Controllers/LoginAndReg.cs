using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrackMyApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace TrackMyApplication.Controllers;

public class LoginController : Controller
{

    private MyContext _context;

    public LoginController(MyContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult LoginandReg()
    {
        bool insesh = false;
        if(HttpContext.Session.GetInt32("userid") != null){
            insesh = true;
            return RedirectToAction("Dashboard", "Home");
        }
        ViewBag.insesh = insesh;
        return View();
    }

    [HttpPost("access")]
    public IActionResult Access(UserInLogin userSubmission)
    {
        bool insesh = false;
        if(HttpContext.Session.GetInt32("userid") != null){
            insesh = true;
            return RedirectToAction("Dashboard", "Home");
        }
        ViewBag.insesh = insesh;
        if(ModelState.IsValid)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == userSubmission.LoginUserName);
            if(userInDb == null)
            {
                ModelState.AddModelError("LoginUserName", "Invalid Username/Password");
                return View("LoginandReg");
            }
            else 
            {
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(userInDb, userInDb.Password, userSubmission.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Username/Password");
                    return View("LoginandReg");
                }
                else {
                    if(HttpContext.Session.GetInt32("userid") == null)
                    {
                        HttpContext.Session.SetInt32("userid", userInDb.UserId);
                    }
                    return RedirectToAction("Dashboard", "Home");
                }
            }
        }
        else
        {
            ModelState.AddModelError("UserName", "Something went wrong");
            return View("LoginandReg");
        }
    }

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        HttpContext.Session.Clear();
        if(ModelState.IsValid)
        {
            if(_context.Users.Any(u => u.UserName == user.UserName))
            {
                ModelState.AddModelError("UserName", "Username is already in use!");
                return View("LoginandReg");
            }
            else
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _context.Add(user);
                _context.SaveChanges();
                var newuser = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);
                if(HttpContext.Session.GetInt32("userid") == null && newuser != null)
                {
                    HttpContext.Session.SetInt32("userid", newuser.UserId);
                }
                return RedirectToAction("Dashboard", "Home");
            }
        }
        else
        {
            return View("LoginandReg");
        }  
    }

    [HttpGet("logout")]
    public IActionResult logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("LoginandReg");
    }
}