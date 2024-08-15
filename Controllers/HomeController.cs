using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using consumeHos.Models;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace consumeHos.Controllers;

public class HomeController : Controller
{
    Uri DatabaseAddress = new Uri("http://172.16.200.202:9810");
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
        string number = "2000";

        using(var client = new HttpClient()) 
        {
            client.BaseAddress = new Uri("http://172.16.200.202:9810");

            var responseTask = client.GetAsync($"/api/Hos/getost?_para={number}");
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

     [HttpGet]
     public ActionResult GetPatient2([FromQuery] string _para)
    {
        IEnumerable<Patient> patient = null;

        using(var client = new HttpClient()) 
        {
            client.BaseAddress = new Uri("http://172.16.200.202:9810");

            var responseTask = client.GetAsync($"/api/Hos/getost?_para={_para}");
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
