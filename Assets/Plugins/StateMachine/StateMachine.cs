using System.Collections.Generic;
namespace Kultie.StateMachine
{
    public class StateMachine<T1, T2> : StateMachineBase<T2> where T1 : System.Enum where T2 : StateContextBase
    {
        private Dictionary<T1, IState<T2>> states;

        public IState<T2> currentState;
        T1 currentStateName;

        public StateMachine(Dictionary<T1, IState<T2>> states)
        {
            this.states = states;
        }

        public void Change(T1 name, T2 context)
        {
            if (!states.ContainsKey(name))
            {
                return;
            }

            if (currentState != null)
            {
                currentState.InternalExit();
            }

            currentState = states[name];
            currentStateName = name;
            currentState.InternalEnter(context);
        }

        public override void Update(float dt)
        {
            if (currentState != null)
            {
                currentState.InternalUpdate(dt);
            }
        }

        public T1 CurrentState()
        {
            return currentStateName;
        }
    }

    public abstract class StateBase<T> : IState<T> where T : StateContextBase
    {
        protected T context;
        public void InternalUpdate(float dt)
        {
            Update(dt);
        }
        public void InternalEnter(T context)
        {
            this.context = context;
            Enter();
        }
        public void InternalExit()
        {
            Exit();
        }
        protected abstract void Enter();
        protected abstract void Exit();
        protected abstract void Update(float dt);
    }
}


