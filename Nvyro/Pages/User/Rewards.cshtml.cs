using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages
{
    public class RewardsAddModel : PageModel
    {
        private readonly RewardService _rewardService;
        public RewardsAddModel(RewardService rewardService)
        {
            _rewardService = rewardService;
        }
        public List<Reward> RewardsList { get; set; } = new();
        [BindProperty]
        public Reward Rewards { get; set; } = new();



        public void OnGet()
        {
            RewardsList = _rewardService.GetAll();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Reward? reward = _rewardService.GetRewardById(Rewards.RewardID.ToString());
                if (reward != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format(
                    "Reward ID {0} alreay exists", Rewards.RewardID);
                    return Page();
                }
                else
                {
                    _rewardService.AddReward(Rewards);
                    return Redirect("/User/Rewards");
                }
            }
            return Page();
        }
    }
}