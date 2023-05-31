namespace Runtime.MainScenes.StateMachine.Interface
{
    using System;

    public interface IStateMachine
    {
        IState CurrentState { get; }

        void TransitionTo(Type stateType);
    }
}