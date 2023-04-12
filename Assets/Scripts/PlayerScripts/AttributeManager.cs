using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    public static AttributeManager instance;

    public int health;
    public int attack;

    public void Start()
    {
        instance = this;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
    }
    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributeManager>();
        if (atm != null)
        {
            atm.TakeDamage(attack);
        }
    }
}
