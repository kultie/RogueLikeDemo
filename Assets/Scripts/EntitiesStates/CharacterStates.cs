using UnityEngine;
using Kultie.StateMachine;
public class CharacterStateContext : StateContextBase
{
    public Vector2 moveVelocity { private set; get; }
    public CharacterControllerBase controller { private set; get; }
    public StateMachine<CharacterState, CharacterStateContext> sm { private set; get; }
    public CharacterStateContext(CharacterControllerBase c, StateMachine<CharacterState, CharacterStateContext> s)
    {
        controller = c;
        sm = s;
    }

    public void SetMoveVelocity(Vector2 value)
    {
        moveVelocity = value;
    }
}
public abstract class CharacterBaseState : StateBase<CharacterStateContext>
{
    protected CharacterController controller
    {
        get
        {
            return context.controller as CharacterController;
        }
    }

    protected virtual Vector2 MoveInput()
    {
        KeyCode lastInput = InputHandleUtilities.GetLastInput();
        Vector2 moveVelocity = Vector2.zero;
        switch (lastInput)
        {
            case KeyCode.UpArrow:
                controller.SetCurrentFacing(Facing.UP);
                moveVelocity.y = 1;
                break;
            case KeyCode.DownArrow:
                controller.SetCurrentFacing(Facing.DOWN);
                moveVelocity.y = -1;
                break;
            case KeyCode.RightArrow:
                controller.SetCurrentFacing(Facing.RIGHT);
                moveVelocity.x = 1;
                break;
            case KeyCode.LeftArrow:
                controller.SetCurrentFacing(Facing.LEFT);
                moveVelocity.x = -1;
                break;
        }
        return moveVelocity;
    }
}
public class CharacterIdleState : CharacterBaseState
{
    protected override void Enter()
    {
        context.controller.entity.ResetAcceleration();
        controller.SetFacingSprite();
    }

    protected override void Exit()
    {
    }

    protected override void Update(float dt)
    {
        Vector2 moveVelocity = MoveInput();
        if (moveVelocity != Vector2.zero)
        {
            context.SetMoveVelocity(moveVelocity);
            context.sm.Change(CharacterState.WALK, context);
        }
    }
}

public class CharacterWalkState : CharacterBaseState
{
    protected override void Enter()
    {

        context.controller.entity.AddForce(context.moveVelocity);
        FacingResolve();
    }

    void FacingResolve()
    {
        string animPrefix = "walk_";
        switch (controller.currentFacing)
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
        context.controller.RequestAnimation(animPrefix, true, true);
    }

    protected override void Exit()
    {
    }

    protected override void Update(float dt)
    {
        Facing lastFacing = controller.currentFacing;
        Vector2 moveVelocity = MoveInput();
        if (lastFacing != controller.currentFacing)
        {
            context.controller.entity.ResetAcceleration();
            FacingResolve();
        }
        if (moveVelocity != Vector2.zero)
        {
            context.controller.entity.AddForce(moveVelocity);
        }
        else
        {
            context.sm.Change(CharacterState.IDLE, context);
        }
    }
}