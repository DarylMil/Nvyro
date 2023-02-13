using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvyro.Models
{
    public class Event
    {
        [Key]
        [Required(ErrorMessage = "Event title cannot be empty"), Display(Name = "Event title"), MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
        public string EventTitle { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        [Display(Name = "Start Date"), Required(ErrorMessage = "Event start date cannot be empty")]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        [DataType(DataType.Date)]
        [Display(Name = "End Date"), Required(ErrorMessage = "Event end date cannot be empty")]
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
        [DataType(DataType.Time)]
        [Display(Name = "Start Time"), Required(ErrorMessage = "Event start time cannot be empty")]
        [Column(TypeName = "Time")]
        public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;
        [DataType(DataType.Time)]
        [Display(Name = "End Time"), Required(ErrorMessage = "Event end time cannot be empty")]
        [Column(TypeName = "Time")]
        public TimeSpan EndTime { get; set; } = DateTime.Now.TimeOfDay;
        [Required, MaxLength(6)]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        public List<Request>? Requests { get; set; }
        public List<Recyclables>? Recyclable { get; set; }
    }
}
