namespace Runtime.MainScenes.StateMachine.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.MainScenes.StateMachine.Interface;
    using Zenject;

    public abstract class StateMachine : IStateMachine
    {
        protected readonly SignalBus signalBus;
        public             IState    CurrentState { get; private set; }

        private readonly Dictionary<Type, IState> typeToState;

        protected StateMachine(List<IState> listState, SignalBus signalBus)
        {
            this.signalBus   = signalBus;
            this.typeToState = listState.ToDictionary(state => state.GetType(), state => state);
        }

        public void TransitionTo(Type stateType)
        {
            this.CurrentState?.Exit();

            if (!this.typeToState.TryGetValue(stateType, out var nextState)) return;

            this.CurrentState = nextState;
            nextState.Enter();
        }
    }
}