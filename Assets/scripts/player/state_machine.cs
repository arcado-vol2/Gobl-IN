using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_machine
{
    public State CurrentState { get; private set; }

    public void initialize(State starting_state)
    {
        CurrentState = starting_state;
        CurrentState.Enter();
    }

    public void change_state(State new_state)
    {
        CurrentState.Exit();
        CurrentState = new_state;
        CurrentState.Enter();
    }
}   
