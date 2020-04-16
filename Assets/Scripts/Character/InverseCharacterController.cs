using Kultie.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseCharacterController : CharacterControllerBase
{
    public InverseCharacterController(RigidEntity e) : base(e)
    {

    }

    protected override Dictionary<CharacterState, IState<CharacterStateContext>> CreateStates()
    {
        Dictionary<CharacterState, IState<CharacterStateContext>> a = new Dictionary<CharacterState, IState<CharacterStateContext>>();
        a[CharacterState.IDLE] = new CharacterIdleState();
        a[CharacterState.WALK] = new InverseCharacterWalkState();
        return a;
    }
}
