﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvyro.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public string? ImageURL { get; set; }
  
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


        [Required(ErrorMessage = "Start postal code cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Postal Code", Prompt = "123456")]
        public string StartPostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start block number cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Block Number", Prompt = "118")]
        public string StartBlockNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start roadname cannot be empty"), MaxLength(100, ErrorMessage = "Maximum 100 characters"), Display(Name = "Roadname", Prompt = "Ang Mo Kio Ave 4")]
        public string StartRoadName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Start postal code cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Postal Code", Prompt = "123456")]
        public string EndPostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start block number cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Block Number", Prompt = "118")]
        public string EndBlockNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start roadname cannot be empty"), MaxLength(100, ErrorMessage = "Maximum 100 characters"), Display(Name = "Roadname", Prompt = "Ang Mo Kio Ave 4")]
        public string EndRoadName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event description cannot be empty"), Display(Name = "Description", Prompt = "Enter event description here"), MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

    }
}
