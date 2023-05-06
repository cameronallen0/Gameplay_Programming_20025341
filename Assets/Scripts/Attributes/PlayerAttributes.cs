using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : CharacterAttributes
{
    public HealthBar healthBar;
    private int damage;

    private void Start()
    {
        InitVariables();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        healthBar.SetHealth(health);
    }

    public void DoDamage(CharacterAttributes doDamage)
    {
        doDamage.TakeDamage(damage);
    }

    public override void InitVariables()
    {
        base.InitVariables();
        damage = 10;
    }
}
