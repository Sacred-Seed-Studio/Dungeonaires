using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour, IAttackable
{
    public int DeviceID { get; set; }
    public int MaxHealth { get; set; }
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

    public bool attack, defend;
    float nextAttack, nextDefend;

    public PlayerClass pClass;
    public PlayerClass PClass
    {
        get { return pClass; }
        set { pClass = value; UpdateSprite(); }
    }
    Slider healthSlider;

    Text nameText;
    public string NameText
    {
        get { return name; }
        set { name = value; nameText.text = name; }
    }

    public float timeAtDefend;
    public float defenseTime = 1f;

    public Image attackImage, defendImage;

    public bool canAttack, canDefend;
    public bool readyToAdventure; //when all are ready, the player will enter the dungeon

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        nameText = GetComponentInChildren<Text>();
        healthSlider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        healthSlider.value = Health;
        canAttack = Time.time > nextAttack;
        canDefend = Time.time > nextDefend;

        if (canAttack)
        {
            attackImage.enabled = true;
        }
        else
        {
            attackImage.enabled = false;
        }
        if (canDefend)
        {
            defendImage.enabled = true;
        }
        else
        {
            defendImage.enabled = false;
        }
        if (attack && canAttack && canDefend)
        {
            Attack();
        }

        if (defend && canDefend)
        {
            Defend();
        }

        if (attack && !canAttack)
        {
            attack = false;
        }
        if (defend && !canDefend)
        {
            defend = false;
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
        MaxHealth = info.health;
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
        Debug.Log("Hitting enemy!");
        GameController.controller.AttackEnemy(AttackPower);
        nextAttack = Time.time + attackCooldownTime;
    }

    IEnumerator ShowAttack()
    {
        sr.sprite = SpriteDictionary.controller.GetAttackSprite(pClass, currentColor);
        AudioController.controller.PlaySound(PClass, true);
        yield return new WaitForSeconds(attackDelay);
        sr.sprite = SpriteDictionary.controller.GetSprite(pClass, currentColor);

        yield return null;
    }

    public void Defend()
    {
        timeAtDefend = Time.time;
        StartCoroutine(ShowDefend());
        //if successfully defending - take no/less damage 
        //Gain some experience/points?
        defend = false;
        Debug.Log("Defending from enemy! ");
        nextDefend = Time.time + defenseCooldownTime;
    }

    IEnumerator ShowDefend()
    {
        sr.sprite = SpriteDictionary.controller.GetDefendSprite(pClass, currentColor);
        AudioController.controller.PlaySound(PClass, false);
        yield return new WaitForSeconds(defendDelay);
        sr.sprite = SpriteDictionary.controller.GetSprite(pClass, currentColor);

        yield return null;
    }
    public void TakeDamage(int amount = 1)
    {
        if (Time.time < defenseTime + timeAtDefend)
        {
            //Defend against the amount;
            amount -= DefensePower;
            if (amount < 0) amount = 0;
        }
        else
        {
            //Debug.Log("Missed:  "+Time.time + " " + defenseTime + timeAtDefend);
        }
        Health -= amount;
        //if (Health < 0)
        //{
        //    Health = 0;
        //    GameController.controller.KillPlayer(this);
        //}
    }

    public bool HasDiedEh()
    {
        return Health <= 0;
    }
}
