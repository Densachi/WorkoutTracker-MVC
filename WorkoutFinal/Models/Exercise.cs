using System.ComponentModel.DataAnnotations;

namespace WorkoutFinal.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Type { get; set; } = "";

        public int Set {  get; set; }
        public int Rep {  get; set; }

        [MaxLength(100)] 
        public string Description { get; set; } = "";
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }



    }
}
