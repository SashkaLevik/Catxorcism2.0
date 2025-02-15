using Data;

namespace Infrastructure.Services
{
    public interface ISaveProgress : ILoadProgress
    {
        public void Save(PlayerProgress progress);

    }
}