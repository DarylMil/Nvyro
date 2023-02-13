using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Nvyro.Models;
using Nvyro.Services;
using Nvyro.Models;
using Nvyro.Models.DTO;

namespace Nvyro.Pages
{
    public class RewardsAddModel : PageModel
    {
        private readonly RewardService _rewardService;

        private readonly UserManager<ApplicationUser> _userManager;
        public RewardsAddModel(RewardService rewardService, UserManager<ApplicationUser> userManager)
        {
            _rewardService = rewardService;
            _userManager = userManager;
        }
        public List<Reward> RewardsList { get; set; } = new();
        [BindProperty]
        public Reward Rewards { get; set; } = new();

        [BindProperty]
        public UpdateUserModel AppUser { get; set; } = new UpdateUserModel();

        public async Task OnGetAsync()
        {

            var existingUser = await _userManager.GetUserAsync(User);

            Console.WriteLine(existingUser.Points);

            existingUser.Points = 100;

            AppUser.Points = existingUser.Points;


            RewardsList = _rewardService.GetAll();


        }
        public IActionResult OnPost()
        {
            Console.WriteLine("Redeem Activate 1.0");
            Console.WriteLine("Current Points : {0}", AppUser.Points);

            Reward? reward = _rewardService.GetRewardById(Rewards.RewardID.ToString());

            Console.WriteLine(reward);


            Console.WriteLine("Redeem Activate 2");
            if (AppUser.Points <= int.Parse(reward.requiredPoints))
            {
                Rewards.RewardName = reward.RewardName;
                Rewards.RewardDescription = reward.RewardDescription;
                Rewards.requiredPoints = reward.requiredPoints;
                Rewards.RewardPicURL = reward.RewardPicURL;
                

                int newQuantity = int.Parse(reward.availableQuantity) - 1;

                if(newQuantity > 0)
                {
                    Rewards.availableQuantity = newQuantity.ToString();

                    _rewardService.UpdateReward(Rewards);
                }
                else
                {
                    _rewardService.RemoveReward(Rewards.RewardID);
                }


                Console.WriteLine("Redeem Success");
                Console.WriteLine(reward.RewardDescription);
               
                return Redirect("/User/Rewards");
            }
            else
            {
                Console.WriteLine("Redeem Fail");
                return Redirect("/User/Rewards");
            }
            return Redirect("/User/Rewards");
        }
    }
}