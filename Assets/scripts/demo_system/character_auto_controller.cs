using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.IO;

public class character_auto_controller : MonoBehaviour
{
    public Queue<action> action_queue;
    public contoller_player player;
    public bool is_playing = false;
    public unit_manager unit_manager_ref;

    float last_vertical_input = 0;
    float last_horizontal_input = 0;

    action current_action;
    public void Start()
    {
        is_playing = !player.user_control;
        action_queue = new Queue<action>();
        //SaveAction(DemoActionType.none, false);
        //player.on_input += SaveAction;
        if (is_playing)
        {
            LoadQueue();
            if (action_queue != null)
            {
                current_action = NextAction();
                StartCoroutine(PlayReplay());
            }
        }

    }



    public void SaveAction(DemoActionType type, bool long_action, float v_in = 0 , float h_in = 0, DemoDeviceType device = DemoDeviceType.none, float angle = 0)
    {
        action new_action = new action(type, v_in, h_in, Time.timeSinceLevelLoad, device, angle, long_action);
        action_queue.Enqueue(new_action);

    }
    public void SaveQueue()
    {
        if (!is_playing)
        {
            unit_manager_ref.replay_manager.AddQueue(action_queue, gameObject.name);
        }
    }
    public void LoadQueue()
    {
        Queue<action> in_queue = unit_manager_ref.replay_manager.GetQueue(gameObject.name);
        if (action_queue != null)
        {
            action_queue = in_queue;
        }
    }


    public action NextAction() {
        ShortAction();
        if (action_queue.Count > 0){
            return action_queue.Dequeue(); }
        return null;

    }
    /*
     Короче, раз я иду спать то напишу завтрашенму себе здесь полную инф-у
    Идея такая у нас все действия окромя движения отлично работают при смене действия, следовательено мы спокойно можем
    из карутина убрать обработку коротких действий
    в свою очередь там должны проигрываться только передвижение и прицеливание (через лерпы от угла к углу, надо подумать как их записывать)
    Передвижение обрабатывается через мувы, но надо записывать их инчаче, тобишь надо писать прошлое передвижение когда оно меняется на новое, потому что иначе будет фигня
    так же надо писать разную обработку дейтвий исходя из их типа
    тобишь мы должны прекращать двтигаться как только приходит команда на то что игрок решил прекратить движение
    имею в виду что мы не должны стопаться если приходить команда, например, заложить бомбу

    селяви

     */
    IEnumerator PlayReplay()
    {
        while (action_queue.Count > 0)
        {
            if (current_action.long_action)
            {
                if (current_action.type == DemoActionType.move)
                {
                    last_horizontal_input = current_action.hor_input;
                    last_vertical_input = current_action.vert_input;
                }

            }
            if (last_vertical_input != 0 || last_horizontal_input != 0)
            {
                player.Move(last_horizontal_input, last_vertical_input);
            }
            yield return null;
        }
    }

    public void Update()
    {
       
      
        if (is_playing)
        {
            if (current_action != null)
            {
                if (Time.timeSinceLevelLoad >= current_action.time)
                {
                    current_action = NextAction();
                }
            }
        }
        else
        {
            
            float vertical_input = Input.GetAxis("Vertical");
            float horizontal_input = Input.GetAxis("Horizontal");

            if ((vertical_input != 0 || horizontal_input != 0) && (vertical_input != last_vertical_input || horizontal_input != last_horizontal_input ) )
            {
                SaveAction(DemoActionType.move, true, last_vertical_input, last_horizontal_input);
            }

            last_vertical_input = vertical_input;
            last_horizontal_input = horizontal_input;
        }
    }

    private void ShortAction()
    {
        if (current_action == null)
        {
            return;
        }
        switch (current_action.type)
        {
            case DemoActionType.plant:
                switch (current_action.device)
                {
                    case DemoDeviceType.mine:
                        player.SpawDevice(player.mine);
                        break;
                    case DemoDeviceType.C4:
                        player.SpawDevice(player.C4);
                        break;
                }
                break;
            case DemoActionType.blow_up:
                player.Detonate();
                break;
            case DemoActionType.shoot:
                player.Aim(false, current_action.angle);
                player.Shoot();
                break;
            case DemoActionType.use:
                player.use_key();
                break;
        }
    }
}
    