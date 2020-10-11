using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IFTurist.Models.TourModel
{
    public class AboutTour
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string VideoLink { get; set; }

       // private List<string> ImageLinks { get; set; }

    }
}
