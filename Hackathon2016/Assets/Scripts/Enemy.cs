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

    SpriteRenderer sr;

    public string NameText
    {
        get { return name; }
        set { name = value; nameText.text = name; }
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        healthSlider = GetComponentInChildren<Slider>();
        nameText = GetComponentInChildren<Text>();

    }

    void Update()
    {
        if (GameController.controller.players.Count > 0) attack = true;
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

    float attackDelay = 0.5f;
    float defendDelay = 0.5f;

    public void Attack()
    {
        StartCoroutine(ShowAttack());
        attack = false;
        //foreach (Player p in GameController.controller.players)
        //{
        //    p.TakeDamage(AttackPower);
        //}
        nextAttack = Time.time + attackCooldownTime;
    }

    IEnumerator ShowAttack()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(attackDelay);
        foreach (Player p in GameController.controller.players)
        {
            p.TakeDamage(AttackPower);
        }

        //check for player death here to avoid that error :)
        foreach (Player p in GameController.controller.players)
        {
            if (p.HasDiedEh())
            {
                GameController.controller.deadPlayers.Add(p);
            }
        }
        foreach (Player p in GameController.controller.deadPlayers)
        {
           if (!p.Dead) GameController.controller.KillPlayer(p);
            //GameController.controller.players.Remove(p);
            //p.gameObject.SetActive(false);
        }

        //hit all players
        sr.color = Color.white;
        nextAttack = Time.time + attackCooldownTime;

        GameController.controller.UpdateAllStats();
        yield return null;
    }
    public void Defend()
    {
        StartCoroutine(ShowDefend());
        defend = false;

        nextDefend = Time.time + defenseCooldownTime;
    }
    IEnumerator ShowDefend()
    {
        sr.color = Color.blue;
        yield return new WaitForSeconds(defendDelay);
        sr.color = Color.white;

        yield return null;
    }
    public void TakeDamage(int amount = 1)
    {
        // DO something based on the enemy defense
        Health -= amount;
        if (Health < 0) Health = 0;
    }

}
