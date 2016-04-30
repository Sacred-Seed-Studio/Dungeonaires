using UnityEngine;
using System.Collections;

public interface IAttackable
{
    void Setup(Information info);
    void Attack();
    void Defend();
    void TakeDamage(int amount);
}
