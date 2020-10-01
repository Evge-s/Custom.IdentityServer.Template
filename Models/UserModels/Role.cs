using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IFTurist.Models.UserModels
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // for future functionality
        public List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }
}
