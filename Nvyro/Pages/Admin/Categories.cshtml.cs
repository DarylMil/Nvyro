using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.Admin
{
    public class CategoriesModel : PageModel
    {
        private readonly INotyfService _toastNotification;
        private readonly CategoryService _categorySerivce;

        public static List<RecycleCategory> AllRecycleCategories { get; set; } = new List<RecycleCategory>();
        [BindProperty]
        public RecycleCategory RecycleCategory { get; set; }
        public static List<RecycleCategory> RecycleCategories { get; set; }
        public static int PageSize { get; set; } = 5;
        public static int PageCount { get; set; }
        public static int CurrentPage { get; set; }

        public CategoriesModel(INotyfService toastNotification, CategoryService categoryService)
        {
            _toastNotification = toastNotification;
            _categorySerivce = categoryService;
        }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            AllRecycleCategories = _categorySerivce.GetRecycleCategoriesAsync().ToList();
            RecycleCategories = AllRecycleCategories.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();
            PageCount = Convert.ToInt32(Math.Ceiling((double)AllRecycleCategories.Count() / PageSize));
            return Page();
        }
        public IActionResult OnPostPageSize(int pageSize)
        {

            PageSize = pageSize;
            return Redirect("./Categories");
        }
        public async Task<IActionResult> OnPostSearchAsync(string searchQuery)
        {
            AllRecycleCategories = _categorySerivce.GetRecycleCategoriesAsync(searchQuery).ToList();
            RecycleCategories = AllRecycleCategories.Skip((1 - 1) * PageSize).Take(PageSize).ToList();
            PageCount = Convert.ToInt32(Math.Ceiling((double)AllRecycleCategories.Count() / PageSize));
            return Page();
        }
        public async Task<IActionResult> OnPostCreateAsync()
        {
            var isSuccess = await _categorySerivce.AddRecycleCategoryAsync(RecycleCategory);
            if(isSuccess >= 0)
            {
                AllRecycleCategories = _categorySerivce.GetRecycleCategoriesAsync().ToList();
                _toastNotification.Success($"Successfully Created Category Name: {RecycleCategory.CategoryName}");
            }
            else if(isSuccess < 0)
            {
                _toastNotification.Error($"Category Name: {RecycleCategory.CategoryName}, Already Exist");
            }
            else
            {
                _toastNotification.Error($"Failed To Create Category Name: {RecycleCategory.CategoryName}");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDisableAsync(string catId)
        {
            var isSuccess = await _categorySerivce.DisableRecycleCategoryAsync(catId);
            if (isSuccess == 1)
            {
                _toastNotification.Success($"Successfully Enabled Category");
            }else if(isSuccess == 2)
            {
                _toastNotification.Success($"Successfully Disabled Category");
            }
            else
            {
                _toastNotification.Error($"Failed To Disable Category");
            }
            return RedirectToPage("./Categories");
        }
    }
}
