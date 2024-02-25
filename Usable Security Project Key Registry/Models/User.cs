using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Usable_Security_Project_Key_Registry.Models
{
    public class User :IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string Email { get; set; } = null!;
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }

        public string? QRSignature { get; set; }
        public string? PINSignature { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }



    }
}
