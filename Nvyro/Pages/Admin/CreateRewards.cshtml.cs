using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.Admin
{
    public class CreateRewardsModel : PageModel
    {
        private readonly RewardService _rewardService;
        public CreateRewardsModel(RewardService rewardService, IWebHostEnvironment environment)
        {
            _rewardService = rewardService;
            _environment = environment;
        }
        public List<Reward> RewardsList { get; set; } = new();
        [BindProperty]
        public AddReward Rewards { get; set; }

        private IWebHostEnvironment _environment;


        public void OnGet()
        {
            RewardsList = _rewardService.GetAll();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("valid");
                Reward? reward = _rewardService.GetRewardById(Rewards.RewardID.ToString());


                if (reward != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format(
                    $"Reward ID {0} alreay exists", Rewards.RewardID);
                    return Page();
                }
                else
                {
                    if (Rewards.PhotoPath != null)
                    {
                        var ImageURL = "";

                        if (Rewards.PhotoPath.Length > 2 * 1024 * 1024)
                        {
                            ModelState.AddModelError("Upload", "File size cannot exceed 2MB");
                            return Page();
                        }

                        var uploadsFolder = "assets/VoucherImage";
                        var imageFile = Guid.NewGuid() + Path.GetExtension(Rewards.PhotoPath.FileName);
                        var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                        using var fileStream = new FileStream(imagePath, FileMode.Create);
                        await Rewards.PhotoPath.CopyToAsync(fileStream);
                        ImageURL = String.Format("/{0}/{1}", uploadsFolder, imageFile);

                        var newreward = new Reward()
                        {
                            RewardID = Rewards.RewardID,
                            RewardName = Rewards.RewardName,
                            RewardDescription = Rewards.RewardDescription,
                            RewardPicURL = ImageURL,
                            requiredPoints = Rewards.requiredPoints,
                            availableQuantity = Rewards.availableQuantity
                        };

                        _rewardService.AddReward(newreward);
                    }
                    return Redirect("/Admin/Dashboard");
                }
            }
            Console.WriteLine("finish");
            return Page();
        }
    }
}
