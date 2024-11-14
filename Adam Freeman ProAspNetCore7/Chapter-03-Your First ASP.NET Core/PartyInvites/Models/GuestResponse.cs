#region Listing 3.6 The contents of the GuestResponse.cs file in the Models folder

//using System.ComponentModel.DataAnnotations;

//namespace PartyInvites.Models
//{
//    public class GuestResponse
//    {
//        public string? Name { get; set; }
//        public string? Email { get; set; }
//        public string? Phone { get; set; }
//        public bool? WillAttend { get; set; }
//    }
//}

#endregion

#region Listing 3.17 Applying validation in the GuestResponse.cs file in the Models folder

using System.ComponentModel.DataAnnotations;

namespace PartyInvites.Models
{
    public class GuestResponse
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Please specify whether you'll attend")]
        public bool? WillAttend { get; set; }
    }
}

#endregion