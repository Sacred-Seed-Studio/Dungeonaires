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

    List<PlayerSpriteInfo> spriteInfo;
    public Dictionary<PlayerSpriteInfo, Sprite> playerSprites;

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
    }

    public Sprite GetSprite(PlayerClass pClass, int color)
    {
        foreach (KeyValuePair<PlayerSpriteInfo, Sprite> pair in playerSprites)
        {
            if (pair.Key.playerClass == pClass && pair.Key.color == color) return pair.Value;
        }
        return null;
    }
}
