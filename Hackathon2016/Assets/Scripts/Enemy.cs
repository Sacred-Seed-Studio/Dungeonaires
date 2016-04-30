using UnityEngine;
using UnityEngine.UI;
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

    Slider healthSlider;

    Text nameText;
    public string NameText
    {
        get { return name; }
        set { name = value; nameText.text = name; }
    }

    void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
        nameText = GetComponentInChildren<Text>();

    }

    void Update()
    {
        attack = true;
        healthSlider.value = Health;
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

        healthSlider.maxValue = Health;
        healthSlider.value = Health;
    }

    public void Attack()
    {
        attack = false;

        //hit all players
        foreach (Player p in GameController.controller.players)
        {
            p.TakeDamage(AttackPower);
        }

        nextAttack = Time.time + attackCooldownTime;
    }

    public void Defend()
    {
        defend = false;

        nextDefend = Time.time + defenseCooldownTime;
    }

    public void TakeDamage(int amount = 1)
    {
        // DO something based on the enemy defense
        Health -= amount;
        if (Health < 0) Health = 0;
    }

}
