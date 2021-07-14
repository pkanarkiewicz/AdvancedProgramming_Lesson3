using System.ComponentModel.DataAnnotations;
namespace AdvancedProgramming_Lesson3.Models

{
    public class FilmyItem
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "To pole nie może być puste")]
        public string Filmyname { get; set; }
        [Required(ErrorMessage = "To pole nie może być puste")]
        public string Author { get; set; }
        [Range(1, 10)]
        [Required(ErrorMessage = "To pole nie może być puste i musi zawierać liczbę od 1 do 10")]
        public string Rating { get; set; }
        public string Secret { get; set; }
    }
}
