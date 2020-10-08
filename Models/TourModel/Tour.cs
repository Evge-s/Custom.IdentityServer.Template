using System.ComponentModel.DataAnnotations;

namespace IFTurist.Models.TourModel
{
    public class Tour
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageLink { get; set; }

    }
}
