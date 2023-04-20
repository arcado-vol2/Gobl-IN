using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_machine_AI
{
    public State_AI current_state { get; private set; }

    public void initialize(State_AI starting_state)
    {
        current_state = starting_state;
        current_state.Enter();
    }

    public void change_state(State_AI new_state)
    {
        current_state.Exit();
        current_state = new_state;
        current_state.Enter();
    }
}
