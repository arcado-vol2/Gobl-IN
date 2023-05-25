using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class defuse_state_AI : State_AI
{
    public defuse_state_AI(controller_AI _character, state_machine_AI _SM) : base(_character, _SM)
    {
    }
    public override void Enter()
    {
        base.Enter();
        character.StartCoroutine(character.GoToClosetDevise());

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.FOV.visible_devices_targets.Count <= 0 )
        {
            SM.change_state(character.s_patrol);
        }
    }
    public override void Exit()
    {
        base.Exit();
        character.StopAllCoroutines();
        character.AI_agent.stoppingDistance = 0;
    }
}