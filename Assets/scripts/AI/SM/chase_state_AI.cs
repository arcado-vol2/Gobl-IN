using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase_state_AI : State_AI
{

    public chase_state_AI(controller_AI _character, state_machine_AI _SM) : base(_character, _SM)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("chase enter");
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SM.change_state(character.s_patrol);
        }
    }
}
