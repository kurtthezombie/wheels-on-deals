using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using WheelsOnDeals.Models;

namespace WheelsOnDeals.ViewModels
{
    public class SignUpViewModel
    {
        public USER_INFO User { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("User.USER_PASSWORD", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}