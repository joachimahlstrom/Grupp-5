using Frisk_2._0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace Frisk_2._0.Controllers
{
    public class AdminController : Controller
    {

        //Skapar en HttpClient för att skicka requests till API:et
        HttpClient _httpClient = new HttpClient();

        // GET: AdminController
        public async Task<ActionResult> Index()
        {
            // deklarerar en ny lista 
            List<User> userList = new List<User>();

            // hämtar data från API:et
            using (var response = await _httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users"))
            {
                // definierar att det är strängdata
                string responseContent = await response.Content.ReadAsStringAsync();

                // konverterar listan med hjälp av json
                userList = JsonConvert.DeserializeObject<List<User>>(responseContent);
            }

            // returnerar listan
            return View(userList);
        }

        // GET: AdminController/Details/5
        
        public async Task<ActionResult> Details(int id)
        {
            // deklarerar en nytt objekt
            User user = new User();
            var response = await _httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users/" + id);

            var responseContent = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<User>(responseContent);

            return View(user);
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return PartialView("_PartialCreateView");
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id, FirstName, LastName, Email, Password, UserType")] User user)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                user.Password = hash;
            }

            string url = @"http://193.10.202.75/FriskAPI/Users";
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            //länk för att skicka det skapade värdet till profilgruppen
            // skapar en variabel för att hämta värdet av id i url-strängen
            string absolutePath = response.Headers.Location.AbsolutePath;

            //definerar att det är en substring och väljer vilken del av hela strängen som skall tas, placerar i en ny variabel
            string idFromDb = absolutePath.Substring(absolutePath.LastIndexOf('/')+1);

            // Skapar en instans av en stringContent med hjälp av Json och specifierar jsontypen
            content = new StringContent(JsonConvert.SerializeObject(idFromDb), Encoding.UTF8, "application/json");

            //Skickar en Post-request till API för profilgruppen endast av id-värdet 
            response = await httpClient.PostAsync("https://informatik1.ei.hv.se/Profiluserinfos/api/UserInfos/Register/" + idFromDb, content);

            //returnerar till indexsidan 
            return RedirectToAction(nameof(Index));

        }

        // GET: AdminController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            User user = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users/" + id))
                {
                    string respons = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(respons);
                }
            }
            return PartialView("_PartialEditView",user);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user)
        {
            string url = @"http://193.10.202.75/FriskAPI/Users/" + user.Id;
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            User user = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users/" + id))
                {
                    string respons = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(respons);
                }
            }
            return View(user);
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, User user)
        {
            string url = @"http://193.10.202.75/FriskAPI/Users/" + id;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction(nameof(Index));
        }
    }
    
}
