
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
        [Required, RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email format is not valid")]
        public string Email { get; set; }
        [Required, RegularExpression("(^05[0-9]{8}$)", ErrorMessage = "Phone Format is not valid")]

        public string Phone { get; set; }


        [ForeignKey("DepartementId")]
        public virtual Departement departement { get; set; }

        public string Message { get; set; }

        public int DepartementId { get; set; }

    }
}