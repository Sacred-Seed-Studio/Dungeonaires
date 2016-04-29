using UnityEngine;
using System.Collections;

public class Information
{
    public Information(float act, float dct, float h, float a, float d)
    {
        attackCooldownTime = act;
        defenseCooldownTime = dct;
        health = h;
        attack = a;
        defense = d;
    }
    public float attackCooldownTime;
    public float defenseCooldownTime;

    //contains any other stats
    public float health;
    public float attack;
    public float defense;
}

public enum PlayerClass
{
    Fighter, //has stronger attacks, weaker defense
    Mage, //has stronger defense, weaker attacks, slowest attacker
    Rogue, //similar to fighter, not as strong but quicker at attcking
    Archer //has strong defense, weaker, but ??? do we need a 4th class?
}

public enum EnemyClass
{
    Enemy1,
    Enemy2,
    Enemy3
}
public static class Helper
{
    public static Information GetInformation(PlayerClass player)
    {
        switch (player)
        {
            default:
            case PlayerClass.Fighter: return new Information(1, 1, 100, 10, 10);
            case PlayerClass.Mage: return new Information(1f, 1, 125, 6f, 13f);
            case PlayerClass.Rogue: return new Information(0.5f, 1, 80, 8, 12);
            case PlayerClass.Archer: return new Information(1, 1, 90, 13, 7);
        }
    }
    public static Information GetInformation(EnemyClass enemy)
    {
        switch (enemy)
        {
            default:
            case EnemyClass.Enemy1: return new Information(1, 1, 100, 10, 10);
            case EnemyClass.Enemy2: return new Information(1f, 1, 125, 6f, 13f);
            case EnemyClass.Enemy3: return new Information(5f, 1, 80, 8, 12);
        }
    }
}
