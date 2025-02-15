using Data;

namespace Infrastructure.Services
{
    class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}
