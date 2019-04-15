
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Contact.Models;
namespace Contact.Models


{
    public class contact
    {
         [Key]
        public int id { get; set; }
        [Required, StringLength(15)]
        public string Name { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email format is not valid")]
        public string Email { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Format is not valid")]
        public string Phone { get; set; }

       
        [ForeignKey("DepartementId")]
        public   Departement departement { get; set; }
        public string Message { get; set; }
    }
}