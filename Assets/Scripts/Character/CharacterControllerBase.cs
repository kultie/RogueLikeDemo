﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kultie.StateMachine;
using Kultie.Animation;

public class CharacterControllerBase : EntityControllerBase, ITriggerCollision
{
    public RigidEntity rigidEntity
    {
        get
        {
            return (RigidEntity)entity;
        }
    }
    StateMachine<CharacterState, CharacterStateContext> stateMachine;
    public CharacterInfo info { private set; get; }
    CharacterStateContext context;
    AnimationSystem anim;
    public Facing currentFacing { private set; get; }
    public CharacterControllerBase(RigidEntity e, string src) : base(e, src) { }

    protected override void Initialize(string src)
    {
        LoadInfo(src);
        CreateStateMachine();
        entity.SetOrderInLayer(1);
    }

    public override void Update(float dt)
    {
        stateMachine.Update(dt);
        anim.Update(dt);
        entity.SetSprite(anim.Frame());
    }

    void LoadInfo(string src)
    {
        info = new CharacterInfo(src);
        currentFacing = info.startFacing;
        SetFacingSprite();
        rigidEntity.SetupRigidInfo(info.accelerationRate, info.maxSpeed, info.friction);
        rigidEntity.SetScaleForCollider(0.5f);
    }

    void CreateStateMachine()
    {
        stateMachine = new StateMachine<CharacterState, CharacterStateContext>(CreateStates());
        context = new CharacterStateContext(this, stateMachine);
        stateMachine.Change(CharacterState.IDLE, context);
    }

    protected virtual Dictionary<CharacterState, IState<CharacterStateContext>> CreateStates()
    {
        Dictionary<CharacterState, IState<CharacterStateContext>> a = new Dictionary<CharacterState, IState<CharacterStateContext>>();
        a[CharacterState.IDLE] = new CharacterIdleState();
        a[CharacterState.WALK] = new CharacterWalkState();
        return a;
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

    public void EnterTriggerCollision(Entity e)
    {
        Debug.Log("Enter" + e);
    }

    public void ExitTriggerCollision(Entity e)
    {
        Debug.Log("Exit" + e);
    }
}
