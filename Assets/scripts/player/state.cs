using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class State
{
    
    protected contoller character;
    protected state_machine SM;

    protected State(contoller _character, state_machine _SM)
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