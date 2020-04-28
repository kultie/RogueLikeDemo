using Kultie.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kultie.Animation;
public abstract class CharacterControllerBase : EntityControllerBase<RigidEntity>
{
    protected StateMachine<CharacterState, CharacterStateContext> stateMachine;
    public CharacterInfo info { protected set; get; }
    protected CharacterStateContext context;
    protected AnimationSystem anim;
    protected override void Initialize(string src)
    {
        LoadInfo(src);
        CreateStateMachine();
        entity.SetOrderInLayer(1);
        entity.RegisterPhysicEvent(this);
    }

    protected abstract void LoadInfo(string src);

    void CreateStateMachine()
    {
        stateMachine = new StateMachine<CharacterState, CharacterStateContext>(CreateStates());
        context = new CharacterStateContext(this, stateMachine);
        stateMachine.Change(CharacterState.IDLE, context);
    }

    protected abstract Dictionary<CharacterState, IState<CharacterStateContext>> CreateStates();


    public CharacterControllerBase(RigidEntity e, string resourceName) : base(e, resourceName)
    {
    }

    public void RequestAnimation(string animID, bool loop = false, bool resetIndex = true, float spf = 0.12f)
    {
        if (anim == null)
        {
            anim = new AnimationSystem(info.animationData[animID], loop, spf);
        }
        else
        {
            anim.SetFrames(info.animationData[animID]);
            anim.SetLoop(loop);
            anim.SetSpf(spf);
        }
        if (resetIndex)
        {
            anim.ResetIndex();
        }
    }

    public abstract void OnTriggerEnter(RigidEntity e);
    public abstract void OnTriggerExit(RigidEntity e);
}
