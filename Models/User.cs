using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frisk_2._0.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Förnamn")]
        public string FirstName { get; set; }
        [DisplayName("Efternamn")]
        public string LastName { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DisplayName("Lösenord")]
        public string Password { get; set; }
    }
}
