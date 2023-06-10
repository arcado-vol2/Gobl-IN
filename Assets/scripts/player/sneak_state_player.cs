using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sneak_state_player : move_state_player
{
    bool crouch_held = Input.GetKey(KeyCode.C);
    public sneak_state_player(contoller_player _character, state_machine_player _SM, character_auto_controller _CAC) : base(_character, _SM, _CAC)
    {

    }

    public override void Enter()
    {
        character.speed = character.sneak_speed;
        character.rotation_speed = character.sneak_rotation_speed;
    }
    public override void HandleInput()
    {
        vertical_input = Input.GetAxis("Vertical");
        horizontal_input = Input.GetAxis("Horizontal");
        crouch_held = Input.GetKey(KeyCode.C);

    }
    public override void LogicUpdate()
    {
        if (!crouch_held)
        {
            SM.change_state(character.s_move);
        }
    }
}
