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
        private readonly CategoryService _categoryService;

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
            _categoryService = categoryService;
        }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            AllRecycleCategories = _categoryService.GetRecycleCategoriesAsync().ToList();
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
            AllRecycleCategories = _categoryService.GetRecycleCategoriesAsync(searchQuery).ToList();
            RecycleCategories = AllRecycleCategories.Skip((1 - 1) * PageSize).Take(PageSize).ToList();
            PageCount = Convert.ToInt32(Math.Ceiling((double)AllRecycleCategories.Count() / PageSize));
            return Page();
        }
        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                var isSuccess = await _categoryService.AddRecycleCategoryAsync(RecycleCategory);
                if(isSuccess >= 0)
                {
                    AllRecycleCategories = _categoryService.GetRecycleCategoriesAsync().ToList();
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
                return Redirect("./Categories");
            }
            catch (Exception)
            {
                _toastNotification.Error($"Failed To Create Category");
                return Page();
            }
        }

    }
}
