using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class unit_manager : MonoBehaviour
{
    public List<GameObject> units;
    public camera_conrol camera_ref;
    public InvertiryManager inv_manager_ref;

    public Replay_manager replay_manager;

    private void Awake()
    {
        replay_manager = FindObjectOfType<Replay_manager>();
        if (replay_manager.unit_id >= units.Count)
        {
            replay_manager.unit_id = 0;
        }
        inv_manager_ref.InitInvertiry(units[replay_manager.unit_id].GetComponent<Player_invertory>().items);
        camera_ref.follow_target = units[replay_manager.unit_id].transform;
        units[replay_manager.unit_id].GetComponent<contoller_player>().user_control = true;
        units[replay_manager.unit_id].GetComponent<character_auto_controller>().is_playing = false;
    }

    public void NextUnit()
    {
        if (replay_manager.unit_id < units.Count)
        {
            replay_manager.unit_id ++;
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        foreach (GameObject unit in units)
        {
            if (unit != null)
            {
                unit.GetComponent<character_auto_controller>().SaveQueue();
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
