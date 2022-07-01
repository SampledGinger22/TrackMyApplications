using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HireMe.Models;
using Microsoft.EntityFrameworkCore;

namespace HireMe.Controllers;

public class HomeController : Controller
{
    private MyContext _context;

    public HomeController(MyContext context)
    {
        _context = context;
    }
    [HttpGet("Dashboard")]
    public IActionResult Dashboard(string search)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        bool insesh = true;
        ViewBag.insesh = insesh;
        int? userid = HttpContext.Session.GetInt32("userid");

        List<Application> Applications = _context.Applications
            .Include(a => a.Interviews)
            .Where(a => a.UserId == userid && a.Active == true)
            .OrderByDescending(a => a.Interviews.All(i => i.IntDate > DateTime.Now))
            .ToList();


        if(!String.IsNullOrEmpty(search))
        {
            List<Application> apps = _context.Applications.Where(x => x.BusinessName.ToLower().Contains(search.ToLower()) && x.UserId == userid && x.Active == true).ToList();
            return View(apps);
        }

        return View("Dashboard", Applications);
    }

    [HttpGet("Applications/Interviews/ViewUpcoming")]
    public IActionResult Interviews()
    {
        bool insesh = true;
        @ViewBag.insesh = insesh;
        int? seshid = HttpContext.Session.GetInt32("userid");

        List<Interview> interviews = _context.Interviews.OrderByDescending(i => i.IntDate > DateTime.Now).Where(i => i.Application.UserId == seshid).ToList();

        return View("Interviews", interviews);
    }

    [HttpGet("Applications/Interviews/View/Edit/{id}")]
    public IActionResult EditInterview(int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }

        Interview interview = _context.Interviews.Where(i => i.InterviewId == id).First();
        return View("EditInterview", interview);
    }

    [HttpGet("Applications/New")]
    public IActionResult NewApp()
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        bool insesh = true;
        ViewBag.insesh = insesh;
        return View("NewApp");
    }

    [HttpPost("Applications/New/Save")]
    public IActionResult SaveApp(Application app)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        if(ModelState.IsValid)
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            app.UserId = (int)userid;
            _context.Applications.Add(app);
            _context.SaveChanges();
            Application? viewapp = _context.Applications.FirstOrDefault(a => a.UserId == userid);
            return RedirectToAction("Dashboard");
        }
        return View("NewApp");
    }

    //ADD "ADD NEW INTERVIEW" ON VIEW APP PAGE

    [HttpGet("Applications/View/{id}")]
    public IActionResult ViewApp(int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        bool insesh = true;
        ViewBag.insesh = insesh;
        int? userid = HttpContext.Session.GetInt32("userid");
        Application? application = _context.Applications
            .OrderBy(a => a.ApplicationId == id && a.UserId == userid).First();
        ViewBag.application = application;

        Console.WriteLine(application.ApplicationId);
        
        ViewBag.appid = application.ApplicationId;
        string appDate = application.AppDate.Value.Date.ToString("yyyy-MM-dd");

        ViewBag.appDate = appDate;


        List<Interview> pastInts = _context.Interviews.Where(i => i.IntDate < DateTime.Now && i.ApplicationId == id).ToList();
        ViewBag.interviews = pastInts;
        Interview? interview = _context.Interviews.Where(i => i.IntDate > DateTime.Now && i.ApplicationId == id).FirstOrDefault();
        if(interview != null)
        {
            Interview nextInt = (Interview)interview;
            ViewBag.NextIntId = nextInt.InterviewId;
            ViewBag.nextInt = nextInt;
        }
        else
        {
            ViewBag.NextInt = null;
            ViewBag.NextIntId = null;
        }

        
        return View("ViewApp");
    }

    [HttpPost("Applications/Update/{id}/Commit")]
    public IActionResult UpdateAppCommit(Application app, int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        int? userid = HttpContext.Session.GetInt32("userid");

        Application application = _context.Applications.OrderBy(a => a.ApplicationId == app.ApplicationId && a.UserId == userid).First();

        application.BusinessName = app.BusinessName;
        application.Location = app.Location;
        application.JobTitle = app.JobTitle;
        application.Contact = app.Contact;
        application.CPhone = app.CPhone;
        application.CEmail = app.CEmail;
        application.CTitle = app.CTitle;
        application.AddContacts = app.AddContacts;
        application.Pros = app.Pros;
        application.Cons = app.Cons;
        application.Notes = app.Notes;
        application.JobURL = app.JobURL;
        application.MinSalary = app.MinSalary;
        application.MaxSalary = app.MaxSalary;
        application.HourlyRate = app.HourlyRate;
        application.AppDate = app.AppDate;

        _context.Applications.Update(application);
        _context.SaveChanges();

        return Redirect($"/Applications/View/{id}");
    }

    // ID From main page, but using a partial. Will have to use ViewBag
    [HttpPost("Applications/{id}/new/interview/commit")]
    public IActionResult SaveInterview(Interview interview, int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        if(ModelState.IsValid)
        {
            interview.ApplicationId = id;
            _context.Interviews.Add(interview);
            _context.SaveChanges();
            return Redirect($"/Applications/View/{id}");
        }
        return Redirect($"/Applications/View/{id}");
    }

    [HttpPost("Applications/Interviews/{id}/Update/Commit/FromApp")]
    public IActionResult UpdateInterviewApp(Interview interview, int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        Interview nextInt = _context.Interviews.OrderByDescending(i => i.ApplicationId == id).First();
        nextInt.Title = interview.Title;
        nextInt.IntDate = interview.IntDate;
        nextInt.Notes = interview.Notes;

        _context.Interviews.Update(nextInt);
        _context.SaveChanges();
        return Redirect($"/Applications/View/{id}");
    }

    [HttpPost("Applications/Interviews/{id}/Update/Commit")]
    public IActionResult UpdateInterview(Interview interview, int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        Interview nextInt = _context.Interviews.OrderByDescending(i => i.InterviewId == id).First();
        nextInt.Title = interview.Title;
        nextInt.IntDate = interview.IntDate;
        nextInt.Notes = interview.Notes;

        _context.Interviews.Update(nextInt);
        _context.SaveChanges();
        return RedirectToAction("Interviews");
    }

    [HttpGet("Applications/View/Archive")]
    public IActionResult Archive(string search)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        bool insesh = true;
        ViewBag.insesh = insesh;
        int? userid = HttpContext.Session.GetInt32("userid");
        List<Application> Application = _context.Applications
            .Include(a => a.Interviews)
            .Where(a => a.UserId == userid && a.Active == false)
            .ToList();

        if(!String.IsNullOrEmpty(search))
        {
            List<Application> apps = _context.Applications.Where(x => x.BusinessName.ToLower().Contains(search.ToLower()) && x.UserId == userid && x.Active == false).ToList();
            return View(apps);
        }

        return View("Archive", Application);
    }

    [HttpPost("Applications/{id}/Archive")]
    public IActionResult ArchiveApp(int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }

        int? userid = HttpContext.Session.GetInt32("userid");
        Application? Application = _context.Applications.FirstOrDefault(a => a.ApplicationId == id && a.UserId == userid);
        Application.Active = false;
        // _context.Applications.Add(Application);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpPost("Applications/{id}/UnArchive")]
    public IActionResult UnArchiveApp(int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        int? userid = HttpContext.Session.GetInt32("userid");
        List<Application> Application = _context.Applications.Where(a => a.ApplicationId == id && a.UserId == userid).ToList();
        Application? App = Application.FirstOrDefault();
        App.Active = true;
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpPost("Applications/{id}/delete")]
    public IActionResult DeleteApp(int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        int? userid = HttpContext.Session.GetInt32("userid");
        List<Interview> interviews = _context.Interviews.Where(i => i.ApplicationId == id).ToList();
        foreach(var interview in interviews)
        {
            _context.Interviews.Remove(interview);
        }
        Application application = _context.Applications.Where(a => a.ApplicationId == id && a.UserId == userid).First();
        _context.Applications.Remove(application);
        _context.SaveChanges();
        return RedirectToAction("Archive");
    }

    [HttpGet("Applications/Interviews/{id}/delete")]
    public IActionResult DeleteInt(int id)
    {
        if(HttpContext.Session.GetInt32("userid") == null)
        {
            return RedirectToAction("LoginandReg", "Login");
        }
        Interview interview = _context.Interviews.Where(i => i.InterviewId == id).First();
        int appId = interview.ApplicationId;
        _context.Interviews.Remove(interview);
        _context.SaveChanges();
        return Redirect($"/Applications/View/{appId}");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
