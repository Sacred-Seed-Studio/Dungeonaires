using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IAttackable
{
    public int PlayerID { get; set; }
    public Color CurrentColor
    {
        get { return sr.color; }
        set { sr.color = value; }
    }
    public float attackCooldownTime = 1, defenseCooldownTime = 1;

    SpriteRenderer sr;

    bool attack, defend;
    float nextAttack, nextDefend;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

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
