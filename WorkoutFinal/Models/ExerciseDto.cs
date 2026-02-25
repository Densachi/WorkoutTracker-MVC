using System.ComponentModel.DataAnnotations;

namespace WorkoutFinal.Models
{
    public class ExerciseDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Type { get; set; } = "";

        [Required]
        public int Set { get; set; }
        [Required]
        public int Rep { get; set; }

        [MaxLength(100)]
        public string Description { get; set; } = "";
        public IFormFile? ImageFile { get; set; }



    }
}
