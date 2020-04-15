using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.StateMachine
{
    public class StateStack<T> : StateMachineBase<T> where T : StateContextBase
    {

        private List<IStateStackState<T>> states;

        public StateStack()
        {
            states = new List<IStateStackState<T>>();
        }

        public void Push(IStateStackState<T> state, T context)
        {
            states.Add(state);
            state.InternalEnter(context);
        }

        public IStateStackState<T> Pop()
        {
            IStateStackState<T> state = states[states.Count - 1];
            states.Remove(state);
            state.InternalExit();
            return state;
        }

        public IStateStackState<T> Top()
        {
            if (states.Count > 0)
            {
                return states[states.Count - 1];
            }
            return null;
        }

        public void PopAll()
        {
            while (states.Count != 0)
            {
                Pop();
            }
        }

        public override void Update(float dt)
        {
            for (int i = states.Count - 1; i >= 0; i--)
            {
                IStateStackState<T> state = states[i];
                state.InternalUpdate(dt);
                bool isBlock = state.IsBlock();
                if (isBlock)
                {
                    break;
                }
            }

            if (states.Count == 0)
            {
                return;
            }
        }
    }

    public abstract class IStateStackState<T> : StateBase<T> where T : StateContextBase
    {
        public abstract bool IsBlock();
    }
}