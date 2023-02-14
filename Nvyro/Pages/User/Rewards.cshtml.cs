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
        private readonly EmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public RewardsAddModel(RewardService rewardService, UserManager<ApplicationUser> userManager, EmailSender emailSender)
        {
            _rewardService = rewardService;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public List<Reward> RewardsList { get; set; } = new();
        [BindProperty]
        public Reward Rewards { get; set; } = new();

        [BindProperty]
        public ApplicationUser AppUser { get; set; } = new ApplicationUser();

        public async Task OnGetAsync()
        {

            var existingUser = await _userManager.GetUserAsync(User);

            Console.WriteLine(existingUser.Points);


            RewardsList = _rewardService.GetAll();


        }
        public async Task<IActionResult> OnPost()
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

                int newPoints = int.Parse(reward.requiredPoints);

                var updatedUser = await _userManager.FindByNameAsync(User.Identity.Name);

                updatedUser.Points -= newPoints;

                _userManager.UpdateAsync(updatedUser);

                int newQuantity = int.Parse(reward.availableQuantity) - 1;


                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var codechars = new char[25];
                var rnd = new Random();

                for (int i = 0; i < codechars.Length; i++)
                {
                    codechars[i] = chars[rnd.Next(chars.Length)];
                }

                var finalcode = new String(codechars);

                Console.WriteLine(finalcode);


                var sendCode = await _emailSender.SendEmailAsync(updatedUser.Email, $"Redemption Code for {Rewards.RewardName} by {updatedUser.UserName}", $"The code for your {Rewards.RewardName} reward is {finalcode}. Enjoy your reward!");


                if (newQuantity > 0)
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