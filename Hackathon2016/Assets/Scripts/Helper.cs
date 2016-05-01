using UnityEngine;
using System;
using System.Collections;

public class Information
{
    public Information(float act, float dct, int h, int a, int d)
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
    public int health;
    public int attack;
    public int defense;
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

public enum EncounterType
{
    Enemy,
    Store
}

public class BidInformation
{
    public BidInformation(int id,int b1, int b2, int b3)
    {
        deviceID = id;
        bid1 = b1;
        bid2 = b2;
        bid3 = b3;
    }
    public int deviceID;
    public int bid1, bid2, bid3;
}

public static class Helper
{
    public static Information[] GetRandomEnemies(int n)
    {
        //return an array with n enemies
        Information[] e = new Information[n];
        for (int i = 0; i < n; i++)
        {
            e[i] = GetInformation((EnemyClass)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EnemyClass)).Length - 1));
        }

        return e;
    }

    public static Information GetInformation(PlayerClass player)
    {
        switch (player)
        {
            default:
            case PlayerClass.Fighter: return new Information(1, 1.5f, 110, 9, 11);
            case PlayerClass.Mage: return new Information(2, 1, 90, 14, 7);
            case PlayerClass.Rogue: return new Information(0.5f, 2, 80, 6, 14);
            case PlayerClass.Archer: return new Information(1.5f, 0.5f, 120, 12, 9);
        }
    }
    public static Information GetInformation(EnemyClass enemy)
    {
        switch (enemy)
        {
            default:
            //case EnemyClass.Enemy1: return new Information(1, 1, 100, 25, 5);
            //case EnemyClass.Enemy2: return new Information(3f, 1, 125, 30, 4);
            //case EnemyClass.Enemy3: return new Information(5f, 1, 80, 15, 3);
            case EnemyClass.Enemy1: return new Information(1, 1, 100, 10, 5);
            case EnemyClass.Enemy2: return new Information(3f, 1, 125, 10, 4);
            case EnemyClass.Enemy3: return new Information(5f, 1, 80, 10, 3);
        }
    }
}
