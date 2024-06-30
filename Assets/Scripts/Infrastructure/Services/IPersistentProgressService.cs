using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}
