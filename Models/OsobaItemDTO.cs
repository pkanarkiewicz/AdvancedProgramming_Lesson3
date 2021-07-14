using System.ComponentModel.DataAnnotations;
namespace AdvancedProgramming_Lesson3.Models
{
    public class OsobaItemDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "To pole nie może być puste")]
        public string Name { get; set; }
        [Required(ErrorMessage = "To pole nie może być puste")]
        public string Secname { get; set; }
    }
}
