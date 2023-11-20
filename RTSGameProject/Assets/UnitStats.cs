using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public int health;
    public int attack;
    public int defense;
    public int maxHealth;
    public float attackRange;
    public float attackSpeed;
    public float moveSpeed;
    public float visionRange;
    public bool isDead;

    public void TakeDamage(int enemyAttack)
    {
        int damage = enemyAttack - defense;
        if (damage < 0)
        {
            damage = 0;
        }
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetAttack()
    {
        return attack;
    }
}
