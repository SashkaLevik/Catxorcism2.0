using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services
{
    public interface ISaveProgress : ILoadProgress
    {
        public void Save(PlayerProgress progress);

    }
}