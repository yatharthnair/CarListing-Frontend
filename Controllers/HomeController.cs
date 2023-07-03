using Car_Listing.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace Car_Listing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpContext? _httpContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        string baseurl = "https://localhost:7027";

        public HomeController(ILogger<HomeController> logger,IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _httpContext = contextAccessor.HttpContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> form2(int id)
        {   
            CarDetails CarInfo = new CarDetails();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync($"api/ToDoItems/GetTodoItem/{id}");


                if (Res.IsSuccessStatusCode)
                {

                    var Response = Res.Content.ReadAsStringAsync().Result;
                    CarInfo = JsonConvert.DeserializeObject<CarDetails>(Response);

                }

                string imreBase64Data = Convert.ToBase64String(CarInfo.image);
                CarInfo.imgURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

                ViewData["EditItem"] = CarInfo;
                return View();
            }
        }
        public async Task<IActionResult> edit(int id)
        {
            List<CarDetails> CarInfo = new List<CarDetails>();
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage Res = await client.GetAsync("api/ToDoItems/GetTodoItems");
                
                if (Res.IsSuccessStatusCode)
                {
                    
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    CarInfo = JsonConvert.DeserializeObject<List<CarDetails>>(Response);
                    foreach (var item in CarInfo)
                    {
                        
                        string imreBase64Data = Convert.ToBase64String(item.image);
                        item.imgURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                        
                    }
                }
                ViewData["CarInfo"] = CarInfo;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(CarDetails x)
        {
            if (ModelState.IsValid)
            {
                List<CarDetails> CarInfo = new List<CarDetails>();
                if (x.formFile.Length > 0)
                {
                    string uploads = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\assets");
                    Directory.CreateDirectory(uploads);
                    if (x.formFile.Length > 0)
                    {
                        string filepath = Path.Combine(uploads, x.formFile.FileName);
                        MemoryStream filestream = new MemoryStream();
                        x.formFile.CopyTo(filestream);
                        x.image = filestream.ToArray();
                    }
                }
            
                using (var client = new HttpClient())
                {
                
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    x.formFile = null;
                    var jsonconsume = JsonConvert.SerializeObject(x);
                    Encoding encoding = Encoding.UTF8;
                    var content = new StringContent(jsonconsume, encoding, "application/json");
               
                    HttpResponseMessage Res = await client.PostAsync($"api/ToDoItems/PostTodoItem", content);
                
                    if (Res.IsSuccessStatusCode)
                    {
                    
                        Console.WriteLine("done");
                    }
                }
            }   
            return RedirectToAction("Index");
        }
        [HttpPut]
        public async Task<IActionResult> Form2(CarDetails x)
        {
            using (var client = new HttpClient())
            {
               
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
               
                var id = x.Id;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync($"api/ToDoItems/GetImage/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    byte[] bytes = JsonConvert.DeserializeObject<byte[]>(Response);
                    x.image = bytes;

                }
                
                var jsonconsume = JsonConvert.SerializeObject(x);
                Encoding encoding = Encoding.UTF8;
                var content = new StringContent(jsonconsume, encoding, "application/json");
                HttpResponseMessage Res1 = await client.PutAsync($"api/ToDoItems/PutTodoItem/{id}", content);
               
                if (Res.IsSuccessStatusCode)
                {
                   
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        public async Task<ActionResult> Index()
        {

            List<CarDetails> CarInfo = new List<CarDetails>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ToDoItems/GetTodoItems");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    CarInfo = JsonConvert.DeserializeObject<List<CarDetails>>(Response);
                    foreach (var item in CarInfo)
                    {

                        string imreBase64Data = Convert.ToBase64String(item.image);
                        item.imgURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

                    }
                }
                ViewData["CarInfo"] = CarInfo;
                return View();
            }
        }
                //else{
                //    if (Res.IsSuccessStatusCode)
                //    {
                //        var Response = Res.Content.ReadAsStringAsync().Result;
                //        CarInfo = JsonConvert.DeserializeObject<List<CarDetails>>(Response);
                //        foreach (var item in CarInfo)
                //        {

                //            string imreBase64Data = Convert.ToBase64String(item.image);
                //            item.imgURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

                //        }


                //    }
                //    ViewData["CarInfo"] = CarInfo.Where(CarInfo.);
                //    return View();
                //}
        [HttpDelete]
        public async Task<ActionResult> delete(int id)
        {
            using (var client = new HttpClient())
            {
               
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
              
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              
                HttpResponseMessage Res = await client.DeleteAsync($"api/ToDoItems/DeleteTodoItem/{id}");
                
                if (Res.IsSuccessStatusCode)
                {
                    Console.WriteLine("Done");
                }
                return RedirectToAction("edit");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Form_Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Form_Add(CarDetails x)
        {
            return View();
        }
        [HttpPost]
        public void Addtocart(int id, int range) {
            var cookieoptions = new CookieOptions();
            cookieoptions.Path = "/";
            cookieoptions.Expires = DateTime.Now.AddDays(1);
            cookieoptions.IsEssential = true;
            cookieoptions.Secure = false;
            _httpContext.Response.Cookies.Append("id", id.ToString(), cookieoptions);
            _httpContext.Response.Cookies.Append("range",range.ToString(),cookieoptions);

        }
        [HttpGet]
        public async Task<ActionResult> table()
        {

            List<Demo> CarInfo = new List<Demo>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync($"api/ToDoItems/GetDetails");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    CarInfo = JsonConvert.DeserializeObject<List<Demo>>(Response);
                    return View(CarInfo);
                }
                else
                {
                    return View(null);
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}