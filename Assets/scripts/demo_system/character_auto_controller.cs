using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_auto_controller : MonoBehaviour
{
    public action[] action_list;
    int current_action_index = 0;
    float current_time = 0;
    public void Update()
    {
        current_time += Time.deltaTime;
        //bug.Log(current_time);
        
        if (current_time >= action_list[current_action_index].time)
        {
            Debug.Log(action_list[current_action_index].type);
            current_action_index++;
        }
    }

}
    