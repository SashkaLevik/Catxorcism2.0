using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.States
{
    public interface IGameStateMachine : IService
    {
        public void Enter<TState>() where TState : class, IState;
        public void Enter<TState, TPayload>(TPayload sceneName) where TState : class, IPayloadedState<TPayload>;
        public void Enter<TState, TPayload>(TPayload sceneName, CardData cardData) where TState : class, IPayloadedState1<TPayload, CardData>;
    }
}
