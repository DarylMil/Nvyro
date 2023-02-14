using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nvyro.Models;
using Nvyro.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Nvyro.Pages.OrganizerEvent
{
    public class AddModel : PageModel
    {
        private readonly EventService _eventService;
        private readonly INotyfService _toastNotification;
        private IWebHostEnvironment _environment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CategoryService _categoryService;

        public AddModel(EventService eventService, INotyfService toastNotification, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, CategoryService categoryService)
        {
            _eventService = eventService;
            _toastNotification = toastNotification;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _categoryService = categoryService;
        }

        [BindProperty]
        public addForm MyEvent { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }


        public static List<RecycleCategory> CategoryList { get; set; } = new(); 
        

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            MyEvent.UserId = userId;

            CategoryList = _categoryService.GetRecycleCategoriesAsync().ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB");
                        return Page();
                    }

                    var uploadsFolder = "assets/EventImage";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    MyEvent.ImageURL = String.Format("/{0}/{1}", uploadsFolder, imageFile);
                }

                var CheckStartDate = MyEvent.StartDate;
                var CheckEndDate = MyEvent.EndDate;

                var CheckStartTime = MyEvent.StartTime;
                var CheckEndTime = MyEvent.EndTime;

                if(CheckEndDate < CheckStartDate)
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
                    var eventCat = MyEvent.Categories;
                    var catName = await _categoryService.GetCategoryById(eventCat);

                    var events = new Event()
                    {
                        Id = MyEvent.Id,
                        User = await _userManager.FindByIdAsync(MyEvent.UserId),
                        UserId = MyEvent.UserId,
                        ImageURL = MyEvent.ImageURL,
                        EventTitle = MyEvent.EventTitle,
                        StartDate = MyEvent.StartDate,
                        EndDate = MyEvent.EndDate,
                        StartTime = MyEvent.StartTime,
                        EndTime = MyEvent.EndTime,
                        StartBlockNumber = MyEvent.StartBlockNumber,
                        StartPostalCode = MyEvent.StartPostalCode,
                        StartRoadName = MyEvent.StartRoadName,
                        EndBlockNumber = MyEvent.EndBlockNumber,
                        EndPostalCode = MyEvent.EndPostalCode,
                        EndRoadName = MyEvent.EndRoadName,
                        Description = MyEvent.Description,
                        Categories = catName

                    };
                    _eventService.AddEvent(events);
                    _toastNotification.Success("Event is created");
                    return Redirect("/OrganizerEvent/Index");
                }

                
            }
            _toastNotification.Error("Event is not created. Please try again.");
            return Page();
        }
    }
    public class addForm
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
