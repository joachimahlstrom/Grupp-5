using Frisk_2._0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Frisk_2._0.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        public async Task<ActionResult> Index()
        {
            List<SignUp> userList = new List<SignUp>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users"))
                {
                    string respons = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<SignUp>>(respons);
                }
            }
            return View(userList);
        }

        // GET: AdminController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
            {
                return BadRequest("Något har blivit fel. Vänligen försök igen.");
            }
            else
            {
                SignUp user = new SignUp();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users/" + id))
                    {
                        string respons = await response.Content.ReadAsStringAsync();
                        user = JsonConvert.DeserializeObject<SignUp>(respons);
                    }
                }
                return View(user);
            }
            return View(id);
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id, FirstName, LastName, Email, Password")] SignUp user)
        {
            string url = @"http://193.10.202.75/FriskAPI/Users";
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            //länk för att skicka det skapade värdet till profilgruppen
            string absolutePath = response.Headers.Location.AbsolutePath;
            string idFromDb = absolutePath.Substring(absolutePath.LastIndexOf('/')+1);
            content = new StringContent(JsonConvert.SerializeObject(idFromDb), Encoding.UTF8, "application/json");
            response = await httpClient.PostAsync("http://193.10.202.71/Profil/api/UserInfos/Register/" + idFromDb, content);
            return RedirectToAction(nameof(Index));

        }

        // GET: AdminController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            SignUp user = new SignUp();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users/" + id))
                {
                    string respons = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<SignUp>(respons);
                }
            }
            return View(user);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SignUp user)
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
            SignUp user = new SignUp();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://193.10.202.75/FriskAPI/Users/" + id))
                {
                    string respons = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<SignUp>(respons);
                }
            }
            return View(user);
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int chosenID, SignUp user)
        {
            string url = @"http://193.10.202.75/FriskAPI/Users/" + chosenID;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction(nameof(Index));
        }
    }
}
