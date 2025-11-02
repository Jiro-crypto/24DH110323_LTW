using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBE1.Models.ViewModel
{

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        [NotMapped]
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
    }
}