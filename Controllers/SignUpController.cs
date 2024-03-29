﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Frisk_2._0.Models;
using System.Security.Cryptography;

namespace Frisk_2._0.Controllers
{
    public class SignUpController : Controller
    {
        //Skapa en statisk HttpClient för att skicka requests till vårt API
        private static readonly HttpClient _httpClient = new HttpClient();

        public IActionResult Index()
        {
            //Returnera vyn för att visa formuläret för registrering
            return View();
        }

        //Funktionen gör en POST-request till en angiven URL med en serialiserad JSON-representation
        //av en instans av modellen SignUp i request-bodyn

        [HttpPost]
        // [ValidateAntiForgeryToken] används för att skydda mot CSRF-attack (Cross-Site Request Forgery)
        [ValidateAntiForgeryToken]

        // [Bind("FirstName,LastName,Email,Password,UserType")] betyder att vi bestämmer vilka delar av informationen som fylls i från formuläret som vi vill använda i vår databas
        public async Task<IActionResult> SignUp([Bind("FirstName,LastName,Email,Password,ConfirmPassword,UserType")] SignUp signUp)
        {

            //Skapa en ny instans av vår SignUp-modell med de parametrar som har angivits från formuläret
            var user = signUp;

            //Hascha lösenordet med SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                user.Password = hash;
            }

            //Serialisera vår modell till JSON
            var json = JsonConvert.SerializeObject(user);

            //Skapa en ny instans av en StringContent med vår JSON och specificera att den är av typen "application/json"
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //Skicka en POST-request till vårt API med vår JSON i request-bodyn
            var response = await _httpClient.PostAsync("http://193.10.202.75/FriskAPI/Users", content);


            //länk för att skicka det skapade värdet till profilgruppen
            string absolutePath = response.Headers.Location.AbsolutePath;
            string idFromDb = absolutePath.Substring(absolutePath.LastIndexOf('/') + 1);
            content = new StringContent(JsonConvert.SerializeObject(idFromDb), Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("https://informatik1.ei.hv.se/Profiluserinfos/api/UserInfos/Register/" + idFromDb, content);
            //response.EnsureSuccessStatusCode();

            //Redirect till Index-vyn i Home-controllern
            return RedirectToAction("Index", "SignUp");
        }
        // Hasha lösenord med SHA256-algoritmen
        
    }
}