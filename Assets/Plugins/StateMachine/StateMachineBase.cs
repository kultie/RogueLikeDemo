using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.StateMachine
{
    public class StateContextBase
    {
    }

    public abstract class StateMachineBase<T> where T : StateContextBase
    {
        private static EmptyState<T> emptyState;

        public static IState<T> Empty()
        {
            return emptyState;
        }

        public abstract void Update(float dt);

    }

    public interface IState<T> where T : StateContextBase
    {
        void InternalEnter(T context);
        void InternalUpdate(float dt);
        void InternalExit();
    }

    class EmptyState<T> : IState<T> where T : StateContextBase
    {

        public void InternalUpdate(float dt)
        {

        }
        public void InternalEnter(T context)
        {

        }
        public void InternalExit()
        {

        }
    }
}