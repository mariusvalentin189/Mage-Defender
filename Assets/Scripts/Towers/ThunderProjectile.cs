using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderProjectile : Projectile
{
    int stunChance;
    float stunTime;

    public int StunChance
    {
        get { return stunChance; }
        set { stunChance = value; }
    }
    public float StunTime
    {
        get { return stunTime; }
        set { stunTime = value; }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            target.GetComponent<Monster>().TakeDamage(damage, projectileType);
            GameObject effect=Instantiate(hitEffect, other.transform);
            effect.transform.localPosition = Vector3.zero;
            ApplyDebuff();
            Destroy(gameObject);
        }
    }

    void ApplyDebuff()
    {
        int randomChance = Random.Range(0, 101);
        if (randomChance <= stunChance)
        {
            target.GetComponent<Monster>().ApplyThunderDebuff(stunTime);
        }
    }
}
