using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class State_player
{
    
    protected contoller_player character;
    protected state_machine_player SM;

    protected State_player(contoller_player _character, state_machine_player _SM)
    {
        this.character = _character;
        this.SM = _SM;
    }
    public virtual void Enter()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}