using Nvyro.Data;
using Nvyro.Models;


namespace Nvyro.Services
{
    public class RewardService
    {
        private readonly MyDbContext _context;
        public RewardService(MyDbContext context)
        {
            _context = context;
        }
        public List<Reward> GetAll()
        {
            return _context.Reward.OrderBy(m => m.RewardName).ToList();
        }
        public Reward? GetRewardById(string id)
        {
            Reward? reward = _context.Reward.FirstOrDefault(
            x => x.RewardID.Equals(id));
            return reward;
        }
        public void AddReward(Reward reward)
        {
            _context.Reward.Add(reward);
            _context.SaveChanges();
        }

        public void RemoveReward(string id)
        {
            Reward? reward = _context.Reward.FirstOrDefault(
            x => x.RewardID.Equals(id));
            _context.Reward.Remove(reward);
            _context.SaveChanges();
        }
    }
}