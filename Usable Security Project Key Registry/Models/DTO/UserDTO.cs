using System.ComponentModel.DataAnnotations;

namespace Usable_Security_Project_Key_Registry.Models.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string? Email { get; set; } = null!;
       
        public string? PublicKey { get; set; }

        public string? phoneNumber { get; set; }

    }

   
}
