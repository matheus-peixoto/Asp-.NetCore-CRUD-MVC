using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Treino3.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be at least {2} characters and at most {1} characters")]
        public string Name { get; set; }
        public List<Seller> Sellers { get; set; }

        public Department() { }

        public Department(string name)
        {
            Name = name;
        }
    }
}
