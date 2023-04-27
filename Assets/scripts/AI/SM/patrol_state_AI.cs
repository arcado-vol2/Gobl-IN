using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class patrol_state_AI : State_AI {

    private float detected_time_start;
    public patrol_state_AI(controller_AI _character, state_machine_AI _SM) : base(_character, _SM)
    {
        detected_time_start = character.patrol_detect_time;
    }

    public override void Enter()
    {
        base.Enter();
        
        character.player = GameObject.FindGameObjectWithTag("Player").transform;
        Transform[] waypoints = new Transform[character.path_holder.childCount];
        waypoints = character.path_holder.GetComponentsInChildren<Transform>().Select(t => t.transform).ToArray(); ;
        character.StartCoroutine(character.FollowPath(waypoints.Skip(1).ToArray()));

    }

    public override void LogicUpdate()
    {
        if (character.FOV.visible_targets.Count > 0)
        {
            character.patrol_detect_time -= Time.deltaTime;
        }
        if (character.patrol_detect_time < 0)
        {
            SM.change_state(character.s_chase);
        }
    }

    public override void Exit()
    {
        base.Exit();
        character.patrol_detect_time = detected_time_start;
        character.StopAllCoroutines();
    }
}
