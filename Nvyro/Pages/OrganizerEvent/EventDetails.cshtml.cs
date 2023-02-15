using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvyro.Pages.OrganizerEvent
{
    public class EventDetailsModel : PageModel
    {
        private readonly EventService _eventService;
        private readonly INotyfService _toastNotification;
        private IWebHostEnvironment _environment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CategoryService _categoryService;

        public EventDetailsModel(EventService eventService, INotyfService toastNotification, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, CategoryService categoryService)
        {
            _eventService = eventService;
            _toastNotification = toastNotification;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _categoryService = categoryService;
        }

        [BindProperty]

        public editForm myEvents { get; set; } = new editForm();
/*        [BindProperty]
        public Event myEvent { get; set; }*/


        [BindProperty]
        public IFormFile? Upload { get; set; }

        public static List<RecycleCategory> CategoryList { get; set; } = new();

        public async Task<IActionResult> OnGet(int id)
        {
            CategoryList = _categoryService.GetRecycleCategoriesAsync().ToList();            
            Event? events = _eventService.GetEventById(id);
            if (events != null)
            {

                myEvents.Id = events.Id;
                myEvents.UserId = events.UserId;
                myEvents.ImageURL = events.ImageURL;
                myEvents.EventTitle = events.EventTitle;
                myEvents.StartDate = events.StartDate;
                myEvents.EndDate = events.EndDate;
                myEvents.StartTime = events.StartTime;
                myEvents.EndTime = events.EndTime;
                myEvents.StartBlockNumber = events.StartBlockNumber;
                myEvents.StartPostalCode = events.StartPostalCode;
                myEvents.StartRoadName = events.StartRoadName;
                myEvents.EndBlockNumber = events.EndBlockNumber;
                myEvents.EndPostalCode = events.EndPostalCode;
                myEvents.EndRoadName = events.EndRoadName;
                myEvents.Description = events.Description;
                myEvents.Categories = events.Categories.ToString();


                /*myEvents = events;*/
                return Page();
            }
            else
            {
                _toastNotification.Error("Event not found");
                return Redirect("/OrganizerEvent/Index");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "assets/EventImage";
                    if (myEvents.ImageURL != null)
                    {
                        var oldImageFile = Path.GetFileName(myEvents.ImageURL);
                        var oldImagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    myEvents.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }

                var CheckStartDate = myEvents.StartDate;
                var CheckEndDate = myEvents.EndDate;

                var CheckStartTime = myEvents.StartTime;
                var CheckEndTime = myEvents.EndTime;

                if (CheckEndDate < CheckStartDate)
                {
                    _toastNotification.Error("Event end date cannot be before event start date. Please try again.");
                    return Page();
                }

                if (CheckEndTime < CheckStartTime)
                {
                    _toastNotification.Error("Event end time cannot be before event start time. Please try again.");
                    return Page();
                }

                else
                {
                    var events = new Event()
                    {
                        Id = myEvents.Id,
                        User = await _userManager.FindByIdAsync(myEvents.UserId),
                        UserId = myEvents.UserId,
                        ImageURL = myEvents.ImageURL,
                        EventTitle = myEvents.EventTitle,
                        StartDate = myEvents.StartDate,
                        EndDate = myEvents.EndDate,
                        StartTime = myEvents.StartTime,
                        EndTime = myEvents.EndTime,
                        StartBlockNumber = myEvents.StartBlockNumber,
                        StartPostalCode = myEvents.StartPostalCode,
                        StartRoadName = myEvents.StartRoadName,
                        EndBlockNumber = myEvents.EndBlockNumber,
                        EndPostalCode = myEvents.EndPostalCode,
                        EndRoadName = myEvents.EndRoadName,
                        Description = myEvents.Description

                    };

                    /*                var user = await _userManager.FindByIdAsync(myEvent.UserId);
                                    myEvent.User = user;
                                    _eventService.UpdateEvent(myEvent);*/
                    _eventService.UpdateEvent(events);
                    _toastNotification.Success("Event updated");
                    return Redirect("/OrganizerEvent/Index");
                }

                
            }
            _toastNotification.Error("Event not updated, try again.");
            return Page();
        }
    }
    public class editForm
    {
        [Key]
        public int Id { get; set; }
        public string? ImageURL { get; set; }

        [Required(ErrorMessage = "Event title cannot be empty"), Display(Name = "Event title", Prompt = "Eg. Recyclable collection Feb 2023"), MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
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


        [Required(ErrorMessage = "Start postal code cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Postal Code", Prompt = "Postal code")]
        public string StartPostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start block number cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Block Number", Prompt = "Block number")]
        public string StartBlockNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start roadname cannot be empty"), MaxLength(100, ErrorMessage = "Maximum 100 characters"), Display(Name = "Roadname", Prompt = "Start roadname")]
        public string StartRoadName { get; set; } = string.Empty;


        [Required(ErrorMessage = "End postal code cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Postal Code", Prompt = "Postal code")]
        public string EndPostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "End block number cannot be empty"), RegularExpression("[0-9]+", ErrorMessage = "Please only enter digits"), MaxLength(6, ErrorMessage = "Maximum 6 digits"), Display(Name = "Block Number", Prompt = "Block number")]
        public string EndBlockNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "End roadname cannot be empty"), MaxLength(100, ErrorMessage = "Maximum 100 characters"), Display(Name = "Roadname", Prompt = "End roadname")]
        public string EndRoadName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event description cannot be empty"), Display(Name = "Description", Prompt = "Enter event description here"), MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string UserId { get; set; }

        [Required, Display(Name = "Recyclable category", Prompt = "Choose a category")]
        public string Categories { get; set; }
    }
}
