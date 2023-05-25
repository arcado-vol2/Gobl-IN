using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class device_logic : MonoBehaviour
{
    [SerializeField]
    private int durability = 2;

    public void LoseDurability(int amount)
    {
        durability -= amount;
        if (durability <= 0)
        {
            Destroy(gameObject);
        }
    }

}
