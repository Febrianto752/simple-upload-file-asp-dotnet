using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using UploadFile.Models;
using UploadFile.Repositories;

namespace UploadFile.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IWebHostEnvironment _webHostEnvironment;
        private readonly ICustomerRepo _customerRepo;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, ICustomerRepo customer)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _customerRepo = customer;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile formFile)
        {
            string path = _customerRepo.DocumentUpload(formFile);
            DataTable dt = _customerRepo.CustomerDataTable(path);
            _customerRepo.ImportCustomer(dt);
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
    }
}