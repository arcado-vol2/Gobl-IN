using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase_state_AI : State_AI
{
    private float start_interest_time;
    public chase_state_AI(controller_AI _character, state_machine_AI _SM) : base(_character, _SM)
    {
    }
    public override void Enter()
    {
        base.Enter();
        character.StartCoroutine(character.Chase_closest_target());
        
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.FOV.visible_targets.Count == 0)
        {
            SM.change_state(character.s_search);
        }
        else
        {
            character.target = character.FOV.visible_targets[character.get_closest_target_id()];
        }
        if (character.target != null)
        {
            if (Vector3.Distance(character.transform.position, character.target.transform.position) < character.attack_range)
            {
                character.Attack();
                SM.change_state(character.s_patrol);
            }
        }

    }
    public override void Exit()
    {
        base.Exit();
        character.StopAllCoroutines();   

    }
}

