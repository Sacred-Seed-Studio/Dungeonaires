﻿using UnityEngine;
using System.Collections;

public class TitleScreenDrop : MonoBehaviour
{
    public float speed = 1f;
    public bool drop;
    public bool dropping;
    public float dropLevel = -800f;

    void Update()
    {
        if (drop)
        {
            drop = false;
            DropScreen();
        }
    }
    public void DropScreen()
    {
        StartCoroutine(Drop());
    }

    IEnumerator Drop()
    {
        dropping = true;
        while (transform.position.y > dropLevel)
        {
            transform.position = (Vector2)transform.position + Vector2.down * speed;
            //yield return new WaitForSeconds(1f);
            yield return null;
        }
        gameObject.SetActive(true);
        dropping = false;
        yield return null;
    }
}
