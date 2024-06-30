using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services
{
    public interface ILoadProgress
    {
        public void Load(PlayerProgress progress);
    }
}
