using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kultie.StateMachine;
using Kultie.Animation;

public class CharacterController : CharacterControllerBase
{
    public Facing currentFacing { private set; get; }
    public CharacterController(RigidEntity e, string src) : base(e, src) { }

    public override void Update(float dt)
    {
        stateMachine.Update(dt);
        anim.Update(dt);
        entity.SetSprite(anim.Frame());
        entity.MoveBody(dt);
    }

    protected override void LoadInfo(string src)
    {
        info = new CharacterInfo(src);
        currentFacing = info.startFacing;
        SetFacingSprite();
        entity.SetupRigidInfo(info.accelerationRate, info.maxSpeed, info.friction);
        entity.SetScaleForCollider(0.5f);
    }

    protected override Dictionary<CharacterState, IState<CharacterStateContext>> CreateStates()
    {
        Dictionary<CharacterState, IState<CharacterStateContext>> a = new Dictionary<CharacterState, IState<CharacterStateContext>>();
        a[CharacterState.IDLE] = new CharacterIdleState();
        a[CharacterState.WALK] = new CharacterWalkState();
        return a;
    }

    public void SetCurrentFacing(Facing f)
    {
        currentFacing = f;
    }

    public void SetFacingSprite()
    {
        switch (currentFacing)
        {
            case Facing.DOWN:
                RequestAnimation("idle_down", true);
                break;
            case Facing.UP:
                RequestAnimation("idle_up", true);
                break;
            case Facing.LEFT:
                RequestAnimation("idle_left", true);
                break;
            case Facing.RIGHT:
                RequestAnimation("idle_right", true);
                break;
        }
    }

    public override void OnTriggerEnter(RigidEntity e)
    {
        Debug.Log("Enter" + e.GetHashCode());
    }

    public override void OnTriggerExit(RigidEntity e)
    {
        Debug.Log("Exit" + e.GetHashCode());
    }
}
