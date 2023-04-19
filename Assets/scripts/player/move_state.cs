using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class move_state : State
{
    protected float horizontal_input;
    protected float vertical_input;

    private bool crouch;
    private bool run;

    public move_state(contoller _character, state_machine _SM) : base(_character, _SM)
    {

    }
    public override void Enter()
    {
        base.Enter();
        character.speed = character.walk_speed;
        character.rotation_speed = character.walk_rotation_speed;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        vertical_input = Input.GetAxis("Vertical");
        horizontal_input = Input.GetAxis("Horizontal");
        crouch = Input.GetKeyDown(KeyCode.C);
        run = Input.GetKeyDown(KeyCode.V);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontal_input, vertical_input);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (crouch)
        {
            SM.change_state(character.s_sneak);
        }
        else if (run)
        {
            SM.change_state(character.s_run);
        }
    }
}
