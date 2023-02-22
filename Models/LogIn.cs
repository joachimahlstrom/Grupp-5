using System;

namespace Frisk_2._0.Models
{
    public class LogIn
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
    }
    public class UserData
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
