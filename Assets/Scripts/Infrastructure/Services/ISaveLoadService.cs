using Data;

namespace Infrastructure.Services
{
    public interface ISaveLoadService : IService
    {
        public void SaveProgress();

        public void ResetProgress();

        public PlayerProgress LoadProgress();
    }
}
