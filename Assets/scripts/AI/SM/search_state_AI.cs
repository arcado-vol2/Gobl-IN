using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class search_state_AI : State_AI
{
    private float start_search_time;
    public search_state_AI(controller_AI _character, state_machine_AI _SM) : base(_character, _SM)
    {
        start_search_time = character.search_time;
    }

    public override void Enter()
    {
        Debug.Log("Search");
        base.Enter();
        character.StartCoroutine(character.Search());
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.FOV.visible_targets.Count == 0)
        {
            character.search_time -= Time.deltaTime;
        }
        else
        {
            SM.change_state(character.s_chase);
        }
        if (character.search_time <= 0)
        {
            SM.change_state(character.s_patrol);
        }
    }

    public override void Exit()
    {
        base.Exit();
        character.StopAllCoroutines();
        character.search_time = start_search_time;
    }
}
