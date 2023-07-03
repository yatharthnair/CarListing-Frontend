using Car_Listing.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;


namespace Car_Listing.Controllers
{
    public class ManageController : Controller
    {
        string baseurl = "https://localhost:7027";

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            
            List<Cart> Info = new List<Cart>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ToDoItems/ShowCart");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    Info = JsonConvert.DeserializeObject<List<Cart>>(Response);
                    foreach (var item in Info)
                    {

                        string imreBase64Data = Convert.ToBase64String(item.image);
                        item.imgURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

                    }
                }
                ViewData["Cartdata"] = Info;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id,int range)
        {

            using (var client = new HttpClient())
            {
                HttpResponseMessage Res = new HttpResponseMessage();
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                for (int i = 0; i < range; i++)
                {
                    Res = await client.PostAsJsonAsync("api/ToDoItems/AddCart", id);
                }
                if (Res.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return View();
            }
        }

        [HttpGet]
        public async Task<int> Count()
        {
            int count = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ToDoItems/GetCartCount");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    count = JsonConvert.DeserializeObject<int>(Response);
                    
                }
                return count;
            }
        }

    }
}
