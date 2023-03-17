using System;
using System.ComponentModel.DataAnnotations;

//Byt till ert projektnamn.Models
namespace Frisk_2._0.Models
{
    public class LogIn
    {
        //Använder valideringsregler för att kontrollera om data matchar det förväntade formatet

        //Input från logga in formulär
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Z|a-z]{2,}$", ErrorMessage = "Ange en giltig email-adress")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Lösenord { get; set; }
        
    }
    public class UserData
    {
        //Värden som hämtas från API sparas i dessa och kan användas på era sidor
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}
