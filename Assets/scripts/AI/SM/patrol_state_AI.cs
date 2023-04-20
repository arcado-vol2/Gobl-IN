using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class patrol_state_AI : State_AI {

    public patrol_state_AI(controller_AI _character, state_machine_AI _SM) : base(_character, _SM)
    {

    }

    public override void Enter()
    {
        Debug.Log("Started");
        base.Enter();
        character.player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3[] waypoints = new Vector3[character.path_holder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = new Vector3(character.path_holder.GetChild(i).position.x, character.transform.position.y, character.path_holder.GetChild(i).position.z);
        }
        character.StartCoroutine(character.FollowPath(waypoints));

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Input.GetKeyDown(KeyCode.U))
        {
            SM.change_state(character.s_chase);
        }
    }

    public override void Exit()
    {
        base.Exit();
        character.StopAllCoroutines();
    }
}
