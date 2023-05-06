using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : CharacterAttributes
{
    private int damage;

    enum slimeSizes
    {
        Large,
        Mid,
        Small
    }
    slimeSizes slimeSize;

    private void Start()
    {
        InitVariables();
    }
    public void DoDamage(CharacterAttributes doDamage)
    {
        doDamage.TakeDamage(damage);
    }
    public override void Die()
    {
        base.Die();
        Split();
    }
    void Split()
    {
        GameObject slimeSplit = gameObject;

        if (slimeSize == slimeSizes.Large)
        {
            MidSlime(slimeSplit);
            MidSlime(slimeSplit);
        }
        else if (slimeSize == slimeSizes.Mid)
        {
            SmallSlime(slimeSplit);
            SmallSlime(slimeSplit);
        }
        Destroy(gameObject);
    }
    void MidSlime(GameObject slime)
    {
        slime = Instantiate(slime);
        slime.GetComponent<EnemyAttributes>().slimeSize = slimeSizes.Mid;
        slime.GetComponent<EnemyAttributes>().maxHealth = 20;
        slime.GetComponent<EnemyAttributes>().damage = 2;
        slime.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    void SmallSlime(GameObject slime)
    {
        slime = Instantiate(slime);
        slime.GetComponent<EnemyAttributes>().slimeSize = slimeSizes.Small;
        slime.GetComponent<EnemyAttributes>().maxHealth = 10;
        slime.GetComponent<EnemyAttributes>().damage = 1;
        slime.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }
    public override void InitVariables()
    {
        SetHealthTo(maxHealth);
        isDead = false;

        damage = 5;
        attackSpeed = 1f;
    }
}
