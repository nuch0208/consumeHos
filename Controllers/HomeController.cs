using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using consumeHos.Models;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace consumeHos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public ActionResult Get()
    {
        IEnumerable<HosViewModel> hos = null;

        using(var client = new HttpClient()) 
        {
            client.BaseAddress = new Uri("http://172.16.200.202:1380");

            var responseTask = client.GetAsync("/api/Hos/Get");
            responseTask.Wait();

            var result = responseTask.Result;

            if(result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<HosViewModel>>();
                readTask.Wait();

                hos = readTask.Result;
            }
            else
            {
                hos = Enumerable.Empty<HosViewModel>();
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View(hos);
        }
    }

    public ActionResult GetPatient()
    {
        IEnumerable<Patient> patient = null;

        using(var client = new HttpClient()) 
        {
            client.BaseAddress = new Uri("http://172.16.200.202:1380");

            var responseTask = client.GetAsync("/api/Hos/getHos?paraHN=3");
            responseTask.Wait();

            var result = responseTask.Result;

            if(result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Patient>>();
                readTask.Wait();

                patient = readTask.Result;
            }
            else
            {
                patient = Enumerable.Empty<Patient>();
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View(patient);
        }
    }

     public ActionResult GetPatientById(string para)
    {
        Patient patient = new Patient();

        using(var client = new HttpClient()) 
        {
            client.BaseAddress = new Uri("http://172.16.200.202:1380");

            var responseTask = client.GetAsync($"/api/Hos/getHos/{para}");
            responseTask.Wait();

            var result = responseTask.Result;

            if(result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Patient>();
                readTask.Wait();

                patient = readTask.Result;
            }
        
            return View(patient);
        }
    }
    
    [HttpGet]
     public ActionResult GetPatient2([FromQuery] string _para)
    {
        IEnumerable<Patient> patient = null;

        using(var client = new HttpClient()) 
        {
            client.BaseAddress = new Uri("http://172.16.200.202:1380");

            var responseTask = client.GetAsync($"/api/Hos/getHos?paraHN={_para}");
            responseTask.Wait();

            var result = responseTask.Result;

            if(result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Patient>>();
                readTask.Wait();

                patient = readTask.Result;
            }
            else
            {
                patient = Enumerable.Empty<Patient>();
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View(patient);
        }
    }
}
