using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class CalculationRequest
    {
        [Required(ErrorMessage = "Input is required")]
        [PositiveDecimal]
        public decimal Input { get; set; }
    }
}
