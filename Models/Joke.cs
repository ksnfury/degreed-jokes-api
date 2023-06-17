using System.ComponentModel.DataAnnotations;

namespace JokeApi.Models
{
    public class Joke
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }
        public int Length { get; set; }
    }
}