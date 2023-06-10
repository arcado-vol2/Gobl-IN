using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class move_state_player : State_player
{
    protected float horizontal_input;
    protected float vertical_input;

    private bool crouch;
    private bool run;

    public move_state_player(contoller_player _character, state_machine_player _SM, character_auto_controller _CAC) : base(_character, _SM, _CAC)
    {

    }
    public override void Enter()
    {
        base.Enter();
        character.speed = character.walk_speed;
        character.rotation_speed = character.walk_rotation_speed;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        vertical_input = Input.GetAxis("Vertical");
        horizontal_input = Input.GetAxis("Horizontal");
        crouch = Input.GetKeyDown(KeyCode.C);
        run = Input.GetKeyDown(KeyCode.V);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Item item = character.invertiry_manager.GetSelectedItem(false);
            if (item != null)
            {
                switch (item.type)
                {
                    case (ItemType.weapon):
                        if (item.action_type == ActionType.Range)
                        {
                            SM.change_state(character.s_shoot);
                        }
                        break;
                    case (ItemType.detonator):
                        character.Detonate();
                        character.invertiry_manager.GetSelectedItem(true);
                        CAC.SaveAction(DemoActionType.blow_up, false);
                        break;
                    case (ItemType.bomb):
                        character.invertiry_manager.GetSelectedItem(true);
                        character.SpawDevice(character.C4);
                        CAC.SaveAction(DemoActionType.plant, false, 0, 0, DemoDeviceType.C4);
                        break;
                    case (ItemType.mine):
                        character.invertiry_manager.GetSelectedItem(true);
                        character.SpawDevice(character.mine);
                        CAC.SaveAction(DemoActionType.plant, false, 0,0, DemoDeviceType.mine);
                        break;
                    case (ItemType.key):
                        if (character.use_key())
                        {
                            character.invertiry_manager.GetSelectedItem(true);
                            CAC.SaveAction(DemoActionType.use, false);
                        }
                        break;

                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            character.invertiry_manager.NextSlot();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            character.invertiry_manager.PrevSlot();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            character.invertiry_manager.ChangeSelectedSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            character.invertiry_manager.ChangeSelectedSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            character.invertiry_manager.ChangeSelectedSlot(2);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontal_input, vertical_input);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (crouch)
        {
            SM.change_state(character.s_sneak);
        }
        else if (run)
        {
            SM.change_state(character.s_run);
        }
    }
}
