using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : RigidEntity
{
    Vector2 movement;
    public override void ManualUpdate(float dt)
    {
        base.ManualUpdate(dt);
        InputHandle();

    }

    void InputHandle() {
        InputHandleUtilities.UpdateInput();
        movement = Vector2.zero;
        switch (InputHandleUtilities.GetLastInput()) {
            case KeyCode.UpArrow:
                movement.y = 1;
                break;
            case KeyCode.DownArrow:
                movement.y = -1;
                break;
            case KeyCode.RightArrow:
                movement.x = 1;
                break;
            case KeyCode.LeftArrow:
                movement.x = -1;
                break;
        }
        AddForce(movement);
    }
}
