using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float attackCooldownTime = 1, defenseCooldownTime = 1;

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
