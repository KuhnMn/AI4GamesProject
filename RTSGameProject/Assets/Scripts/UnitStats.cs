using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    [SerializeField] private Image HealthBar;
    public GameObject InFormation;
    

    public int health;
    public int attack;
    public int defense;
    public int maxHealth;
    public float attackRange;
    public float attackSpeed;
    public float moveSpeed;
    public float visionRange;
    public bool isDead;
    public Team team;

    public enum Team
    {
        GoodBean,
        BadBean
    }

    void Start()
    {
        isDead = false;
    }

    void Update(){
        UpdateHeathBar();
    }

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

    public float GetVisionRange()
    {
        return visionRange;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetDefense()
    {
        return defense;
    }

    public string GetEnemyTag()
    {
        if (team == Team.GoodBean)
        {
            return Team.BadBean.ToString();
        } else
        {
            return Team.GoodBean.ToString();
        }
    }

    public string GetTeamTag()
    {
        return team.ToString();
    }

    public void UpdateHeathBar(){
        HealthBar.fillAmount = 1.0f - ((health * 1.0f) / (maxHealth * 1.0f));
    }
}