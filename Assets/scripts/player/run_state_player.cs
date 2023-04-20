using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class run_state_player : move_state_player
{
    bool run_held = Input.GetKey(KeyCode.V);
    public run_state_player(contoller_player _character, state_machine_player _SM) : base(_character, _SM)
    {

    }

    public override void Enter()
    {
        character.speed = character.run_speed;
        character.rotation_speed = character.run_rotation_speed;
    }

    public override void HandleInput()
    {
        vertical_input = Input.GetAxis("Vertical");
        horizontal_input = Input.GetAxis("Horizontal");
        run_held = Input.GetKey(KeyCode.V);

    }
    public override void LogicUpdate()
    {
        if (!run_held)
        {
            SM.change_state(character.s_move);
        }
    }
}

