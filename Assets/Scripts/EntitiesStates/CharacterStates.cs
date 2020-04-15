using UnityEngine;
using Kultie.StateMachine;
public class CharacterStateContext : StateContextBase
{
    public Vector2 moveVelocity { private set; get; }
    public CharacterController controller { private set; get; }
    public StateMachine<CharacterState, CharacterStateContext> sm { private set; get; }
    public CharacterStateContext(CharacterController c, StateMachine<CharacterState, CharacterStateContext> s)
    {
        controller = c;
        sm = s;
    }

    public void SetMoveVelocity(Vector2 value)
    {
        moveVelocity = value;
    }
}
public class CharacterIdleState : StateBase<CharacterStateContext>
{
    protected override void Enter()
    {
        context.controller.SetFacingSprite();
    }

    protected override void Exit()
    {
    }

    protected override void Update(float dt)
    {
        KeyCode lastInput = InputHandleUtilities.GetLastInput();
        Vector2 moveVelocity = Vector2.zero;
        switch (lastInput)
        {
            case KeyCode.UpArrow:
                context.controller.SetCurrentFacing(Facing.UP);
                moveVelocity.y = 1;
                break;
            case KeyCode.DownArrow:
                context.controller.SetCurrentFacing(Facing.DOWN);
                moveVelocity.y = -1;
                break;
            case KeyCode.RightArrow:
                context.controller.SetCurrentFacing(Facing.RIGHT);
                moveVelocity.x = 1;
                break;
            case KeyCode.LeftArrow:
                context.controller.SetCurrentFacing(Facing.LEFT);
                moveVelocity.x = -1;
                break;
        }
        if (moveVelocity != Vector2.zero)
        {
            context.SetMoveVelocity(moveVelocity);
            context.sm.Change(CharacterState.WALK, context);
        }
    }
}

public class CharacterWalkState : StateBase<CharacterStateContext>
{
    protected override void Enter()
    {

        context.controller.AddForce(context.moveVelocity);
        FacingResolve(true);
    }

    void FacingResolve(bool resetIndex) {
        string animPrefix = "walk_";
        switch (context.controller.currentFacing)
        {
            case Facing.LEFT:
                animPrefix += "left";
                break;
            case Facing.DOWN:
                animPrefix += "down";
                break;
            case Facing.RIGHT:
                animPrefix += "right";
                break;
            case Facing.UP:
                animPrefix += "up";
                break;
        }
        context.controller.RequestAnimation(animPrefix, true, resetIndex);
    }

    protected override void Exit()
    {
    }

    protected override void Update(float dt)
    {
        KeyCode lastInput = InputHandleUtilities.GetLastInput();
        Vector2 moveVelocity = Vector2.zero;
        Facing lastFacing = context.controller.currentFacing;
        switch (lastInput)
        {
            case KeyCode.UpArrow:
                context.controller.SetCurrentFacing(Facing.UP);
                moveVelocity.y = 1;
                break;
            case KeyCode.DownArrow:
                context.controller.SetCurrentFacing(Facing.DOWN);
                moveVelocity.y = -1;
                break;
            case KeyCode.RightArrow:
                context.controller.SetCurrentFacing(Facing.RIGHT);
                moveVelocity.x = 1;
                break;
            case KeyCode.LeftArrow:
                context.controller.SetCurrentFacing(Facing.LEFT);
                moveVelocity.x = -1;
                break;
        }
        FacingResolve(lastFacing != context.controller.currentFacing);
        if (moveVelocity != Vector2.zero)
        {
            context.controller.AddForce(moveVelocity);
        }
        else
        {
            context.sm.Change(CharacterState.IDLE, context);
        }
    }
}