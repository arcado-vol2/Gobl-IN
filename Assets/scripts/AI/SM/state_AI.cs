using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class State_AI
{

    protected controller_AI character;
    protected state_machine_AI SM;

    protected State_AI(controller_AI _character, state_machine_AI _SM)
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