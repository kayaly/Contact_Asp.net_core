using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contact.Models
{
    public class Departement
    {
        [Key]
        public int DepartementId { get; set; }
        public string Name { get; set; }
        
    }
}