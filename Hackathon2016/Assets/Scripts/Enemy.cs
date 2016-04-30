using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float attackCooldownTime = 1, defenseCooldownTime = 1;

    public int Health { get; set; }
    public int Gold { get; set; }
    public int AttackPower { get; set; }
    public int DefensePower { get; set; }

    bool attack, defend;
    float nextAttack, nextDefend; //enemies will have a more complication attack and defense system --coming soon :)

    void Update()
    {
        if (attack && Time.time > nextAttack)
        {
            Attack();
        }

        if (defend && Time.time > nextDefend)
        {
            Defend();
        }
    }

    public void Setup(Information info)
    {
        Health = info.health;
        AttackPower = info.attack;
        DefensePower = info.defense;
        attackCooldownTime = info.attackCooldownTime;
        defenseCooldownTime = info.defenseCooldownTime;
    }

    public void Attack()
    {
        attack = false;

        nextAttack = Time.time + attackCooldownTime;
    }

    public void Defend()
    {
        defend = false;

        nextDefend = Time.time + defenseCooldownTime;
    }
}
