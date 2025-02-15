using Data;

namespace Infrastructure.Services
{
    public interface ILoadProgress
    {
        public void Load(PlayerProgress progress);
    }
}
