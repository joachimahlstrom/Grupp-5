using System;
using System.ComponentModel.DataAnnotations;

namespace Frisk_2._0.Models
{ 
public class SignUp
{

    // Dessa variabler sparas i API databasen
    public int Id { get; set; }

        //Använder valideringsregler för att kontrollera om data matchar det förväntade formatet

        [RegularExpression(@"^[A-Öa-ö\s]+$", ErrorMessage = "Förnamnet får bara innehålla bokstäver")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Öa-ö\s]+$", ErrorMessage = "Efternamnet får bara innehålla bokstäver")]
        public string? LastName { get; set; }

        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Z|a-z]{2,}$", ErrorMessage = "Ange en giltig email-adress")]
        public string Email { get; set; }
        
        public string UserType { get; set; }

        [Required(ErrorMessage = "Ange ditt lösenord")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z]).{5,}$", ErrorMessage = "Lösenordet måste vara minst 5 tecken långt och innehålla en stor bokstav")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte.")]
        public string ConfirmPassword { get; set; }


    }
}

   


