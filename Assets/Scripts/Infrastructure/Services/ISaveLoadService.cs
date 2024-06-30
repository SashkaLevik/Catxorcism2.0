using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services
{
    public interface ISaveLoadService : IService
    {
        public void SaveProgress();

        public void ResetProgress();

        public PlayerProgress LoadProgress();
    }
}
