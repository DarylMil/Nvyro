using Microsoft.EntityFrameworkCore;
using Nvyro.Data;
using Nvyro.Models;

namespace Nvyro.Services
{
    public class CategoryService
    {
        private readonly MyDbContext _context;
        public CategoryService(MyDbContext context)
        {
            _context = context;
        }
        public IQueryable<RecycleCategory> GetRecycleCategoriesAsync(string? query = null)
        {
            if (query == null)
            {
                return _context.RecycleCategory;
            }
            else
            {
                query = query.ToUpper();
                return _context.RecycleCategory.Where(r => r.CategoryName.ToUpper().Contains(query));
            }
        }

        public async Task<RecycleCategory> GetCategoryById(string id)
        {
            return await _context.RecycleCategory.FirstOrDefaultAsync(x => x.CategoryId == id);
        }
        public async Task<int> AddRecycleCategoryAsync(RecycleCategory recycleCategory)
        {
            var isExist = _context.RecycleCategory.Where(r => r.CategoryName == recycleCategory.CategoryName);
            if(isExist.Count() > 0)
            {
                return -1;
            }
            _context.RecycleCategory.Add(recycleCategory);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DisableRecycleCategoryAsync(string catId)
        {
            var existCat = await _context.RecycleCategory.FindAsync(catId);
            if (existCat != null)
            {
                if (existCat.IsDisabled)
                {
                    existCat.IsDisabled = false;
                    await _context.SaveChangesAsync();
                    return 1;
                }
                else
                {
                    existCat.IsDisabled = true;
                    await _context.SaveChangesAsync();
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
