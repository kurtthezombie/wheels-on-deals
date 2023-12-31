//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WheelsOnDeals.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web;

    public partial class CarListing
    {
        [Display(Name = "Listing ID")]
        public int CarID { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "This field is required.")]
        public string Make { get; set; }

        [Display(Name = "Model")]
        [Required(ErrorMessage = "This field is required.")]
        public string Model { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "This field is required.")]
        public int CarYear { get; set; }

        [Display(Name = "Color")]
        [Required(ErrorMessage = "This field is required.")]
        public string Color { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "This field is required.")]

        public decimal Price { get; set; }

        [Display(Name = "Mileage")]
        [Required(ErrorMessage = "This field is required.")]

        public int Mileage { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "This field is required.")]

        public string CarDescription { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [Display(Name = "Owner")]
        public int OwnerID { get; set; }

        [Display(Name = "Listed at")]
        public Nullable<System.DateTime> CreatedAt { get; set; }
    
        public virtual USER_INFO USER_INFO { get; set; }
    }
}
