using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_machine_AI
{
    public State_AI current_state { get; private set; }
    private controller_AI bot;

    public void initialize(State_AI starting_state, controller_AI AI)
    {
        current_state = starting_state;
        current_state.Enter();
        bot = AI;
    }

    public void change_state(State_AI new_state)
    {
        current_state.Exit();
        current_state = new_state;
        bot.RefreshState();
        current_state.Enter();
    }
}
