using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour, IAttackable
{
    public int PlayerID { get; set; }
    public int Health { get; set; }
    public int Gold { get; set; }
    public int AttackPower { get; set; }
    public int DefensePower { get; set; }

    int currentColor;
    public int CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; UpdateSprite(); }
    }

    public float attackCooldownTime = 1, defenseCooldownTime = 1;

    SpriteRenderer sr;

    bool attack, defend;
    float nextAttack, nextDefend;

    public PlayerClass pClass;
    public PlayerClass PClass
    {
        get { return pClass; }
        set { pClass = value; UpdateSprite(); }
    }

    Text nameText;
    public string NameText
    {
        get { return name; }
        set { name = value; nameText.text = name; }
    }

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        nameText = GetComponentInChildren<Text>();
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

    void UpdateSprite()
    {
        //set the color and class
        sr.sprite = SpriteDictionary.controller.GetSprite(pClass, CurrentColor);
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
