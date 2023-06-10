using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Replay_manager : MonoBehaviour
{
    public Dictionary<string, Queue<action>> queues;


    public int unit_id = 0; 
    public static Replay_manager instance { get; private set; }


    private void Awake()
    {
        queues = new Dictionary<string, Queue<action>>();
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

    }
    public void AddQueue(Queue<action> q, string unit_name)
    {
        if (queues.ContainsKey(unit_name))
        {
            queues[unit_name] = new Queue<action>(q);
        }
        else
        {
            queues.Add(unit_name, new Queue<action>(q));
        }
    }
    public Queue<action> GetQueue(string unit_name)
    {
        if (queues.ContainsKey(unit_name))
        {
            return new Queue<action>(queues[unit_name]);
        }
        return null;
    }
}
