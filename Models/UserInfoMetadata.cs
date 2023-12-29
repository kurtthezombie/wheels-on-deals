using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WheelsOnDeals.Models
{
    [MetadataType(typeof(UserInfoMetadata))]
    public partial class USER_INFO
    {
    }
    public class UserInfoMetadata
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string USER_EMAIL { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string USER_FNAME { get; set; }
        [Required(ErrorMessage = "Lastname is required.")]
        public string USER_LNAME { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string USER_PASSWORD { get; set; }
    }
}