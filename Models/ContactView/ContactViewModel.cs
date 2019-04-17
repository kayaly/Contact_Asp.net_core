using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Contact.Models.ContactViewModel
{
    public class ContactViewModel
    {
        
public int id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        

        public string Phone { get; set; }

        public virtual Departement departement { get; set; }

        public string Message { get; set; }

        public int DepartementId { get; set; }
      
    }
}