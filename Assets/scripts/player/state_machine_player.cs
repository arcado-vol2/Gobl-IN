using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_machine_player
{
    public State_player CurrentState { get; private set; }

    public void initialize(State_player starting_state)
    {
        CurrentState = starting_state;
        CurrentState.Enter();
    }

    public void change_state(State_player new_state)
    {
        CurrentState.Exit();
        CurrentState = new_state;
        CurrentState.Enter();
    }
}   
