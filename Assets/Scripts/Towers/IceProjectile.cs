using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : Projectile
{
    int slowPercentage;
    int slowChance;
    float slowTime;

    public int SlowPercentage
    {
        get { return slowPercentage; }
        set { slowPercentage = value; }
    }
    public int SlowChance
    {
        get { return slowChance; }
        set { slowChance = value; }
    }
    public float SlowTime
    {
        get { return slowTime; }
        set { slowTime = value; }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            target.GetComponent<Monster>().TakeDamage(damage, ProjectileType.ice);
            GameObject effect=Instantiate(hitEffect, other.transform);
            effect.transform.localPosition = Vector3.zero;
            ApplyDebuff();
            Destroy(gameObject);
        }
    }

    void ApplyDebuff()
    {
        int randomChance = Random.Range(0, 101);
        if (randomChance <= slowChance)
        {
            target.GetComponent<Monster>().ApplyIceDebuff(slowTime,slowPercentage);
        }
    }
}
