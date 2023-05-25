using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot_state_player : State_player
{
    public shoot_state_player(contoller_player _character, state_machine_player _SM) : base(_character, _SM)
    {
    }
    public override void Enter()
    {
        base.Enter();
        character.ToggleAim();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Aim();
        character.RefreshLaser();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            character.invertiry_manager.GetSelectedItem(true);
            character.Shoot();
            SM.change_state(character.s_move);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1)) {
            SM.change_state(character.s_move);

        }
    }

    public override void Exit()
    {
        base.Exit();
        character.ToggleAim();
    }
}
