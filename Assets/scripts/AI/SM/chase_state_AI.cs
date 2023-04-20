using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase_state_AI : State_AI
{
    private float start_interest_time;
    public chase_state_AI(controller_AI _character, state_machine_AI _SM) : base(_character, _SM)
    {
        start_interest_time = character.chase_interest_time;
    }
    public override void Enter()
    {
        base.Enter();
        character.StartCoroutine(character.Chase_closest_target());
        Debug.Log("chase enter");
        
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.FOV.visible_targets.Count == 0)
        {
            character.chase_interest_time -= Time.deltaTime;
        }
        else
        {
            character.chase_interest_time = start_interest_time;
        }

        if (character.chase_interest_time < 0)
        {
            SM.change_state(character.s_search);
        }
    }
    public override void Exit()
    {
        base.Exit();
        character.chase_interest_time = start_interest_time;
    }
}

