using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile
{
    int tickDamage;
    int tickChance;
    float tickTime;
    float tickDelay;

    public int TickDamage
    {
        get { return tickDamage; }
        set { tickDamage = value; }
    }
    public int TickChance
    {
        get { return tickChance; }
        set { tickChance = value; }
    }
    public float TickTime
    {
        get { return tickTime; }
        set { tickTime = value; }
    }
    public float TickDelay
    {
        get { return tickDelay; }
        set { tickDelay = value; }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            target.GetComponent<Monster>().TakeDamage(damage, ProjectileType.fire);
            GameObject effect = Instantiate(hitEffect, other.transform);
            effect.transform.localPosition = Vector3.zero;
            ApplyDebuff();
            Destroy(gameObject);
        }
    }
    void ApplyDebuff()
    {
        int randomChance = Random.Range(0, 101);
        if(randomChance<=tickChance)
        {
            target.GetComponent<Monster>().ApplyDebuff(tickTime, tickDelay, tickDamage, ProjectileType.fire);
        }
    }
}
