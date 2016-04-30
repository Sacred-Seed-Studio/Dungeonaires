using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSpriteInfo
{
    public PlayerSpriteInfo(PlayerClass pClass, int c)
    {
        playerClass = pClass;
        color = c;
    }
    public PlayerClass playerClass;
    public int color;
}

public class SpriteDictionary : MonoBehaviour
{
    public static SpriteDictionary controller;

    public List<Sprite> fighterSprites, mageSprites, rogueSprites, archerSprites;
    public List<Sprite> fighterAttackSprites, mageAttackSprites, rogueAttackSprites, archerAttackSprites;
    public List<Sprite> fighterDefendSprites, mageDefendSprites, rogueDefendSprites, archerDefendSprites;

    List<PlayerSpriteInfo> spriteInfo;
    public Dictionary<PlayerSpriteInfo, Sprite> playerSprites;
    public Dictionary<PlayerSpriteInfo, Sprite> playerAttackSprites;
    public Dictionary<PlayerSpriteInfo, Sprite> playerDefendSprites;

    public void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else if (controller != null)
        {
            Destroy(gameObject);
        }

        LoadPlayerSprite();
    }

    void LoadPlayerSprite()
    {
        playerSprites = new Dictionary<PlayerSpriteInfo, Sprite>();
        playerAttackSprites = new Dictionary<PlayerSpriteInfo, Sprite>();
        playerDefendSprites = new Dictionary<PlayerSpriteInfo, Sprite>();
        spriteInfo = new List<PlayerSpriteInfo>();
        int i = 0;

        foreach (Sprite s in fighterSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Fighter, i);
            playerSprites[psi] = s;
            spriteInfo.Add(psi);
            i++;
        }

        i = 0;
        foreach (Sprite s in mageSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Mage, i);
            playerSprites[psi] = s;
            spriteInfo.Add(psi);
            i++;
        }

        i = 0;
        foreach (Sprite s in rogueSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Rogue, i);
            playerSprites[psi] = s;
            spriteInfo.Add(psi);
            i++;
        }

        i = 0;
        foreach (Sprite s in archerSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Archer, i);
            playerSprites[psi] = s;
            spriteInfo.Add(psi);
            i++;
        }

        i = 0;
        foreach (Sprite s in fighterAttackSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Fighter, i);
            playerAttackSprites[psi] = s;
            i++;
        }

        i = 0;
        foreach (Sprite s in mageAttackSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Mage, i);
            playerAttackSprites[psi] = s;
            i++;
        }

        i = 0;
        foreach (Sprite s in rogueAttackSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Rogue, i);
            playerAttackSprites[psi] = s;
            i++;
        }

        i = 0;
        foreach (Sprite s in archerAttackSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Archer, i);
            playerAttackSprites[psi] = s;
            i++;
        }

        i = 0;
        foreach (Sprite s in fighterDefendSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Fighter, i);
            playerDefendSprites[psi] = s;
            i++;
        }

        i = 0;
        foreach (Sprite s in mageDefendSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Mage, i);
            playerDefendSprites[psi] = s;
            i++;
        }

        i = 0;
        foreach (Sprite s in rogueDefendSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Rogue, i);
            playerDefendSprites[psi] = s;
            i++;
        }

        i = 0;
        foreach (Sprite s in archerDefendSprites)
        {
            PlayerSpriteInfo psi = new PlayerSpriteInfo(PlayerClass.Archer, i);
            playerDefendSprites[psi] = s;
            i++;
        }


    }

    public Sprite GetSprite(PlayerClass pClass, int color)
    {
        foreach (KeyValuePair<PlayerSpriteInfo, Sprite> pair in playerSprites)
        {
            if (pair.Key.playerClass == pClass && pair.Key.color == color) return pair.Value;
        }
        return null;
    }

    public Sprite GetAttackSprite(PlayerClass pClass, int color)
    {
        foreach (KeyValuePair<PlayerSpriteInfo, Sprite> pair in playerAttackSprites)
        {
            if (pair.Key.playerClass == pClass && pair.Key.color == color) return pair.Value;
        }
        return null;
    }

    public Sprite GetDefendSprite(PlayerClass pClass, int color)
    {
        foreach (KeyValuePair<PlayerSpriteInfo, Sprite> pair in playerDefendSprites)
        {
            if (pair.Key.playerClass == pClass && pair.Key.color == color) return pair.Value;
        }
        return null;
    }
}
