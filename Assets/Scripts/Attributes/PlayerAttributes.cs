using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : CharacterAttributes
{
    //private PlayerHUD hud;
    private int damage;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void GetReferences()
    {
        //hud = GetComponent<PlayerHUD>();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        //hud.UpdateHealth(health, maxHealth);
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
