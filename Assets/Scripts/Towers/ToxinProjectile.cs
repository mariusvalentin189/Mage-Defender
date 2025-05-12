using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinProjectile : Projectile
{
    [SerializeField] ToxinCloud cloud;
    int cloudChance;
    int cloudDamage;
    float cloudTime;
    float cloudDamageDelay;

    public int CloudChance
    {
        get { return cloudChance; }
        set { cloudChance = value; }
    }
    public int CloudDamage
    {
        get { return cloudDamage; }
        set { cloudDamage = value; }
    }
    public float CloudTime
    {
        get { return cloudTime; }
        set { cloudTime = value; }
    }
    public float CloudDamageDelay
    {
        get { return cloudDamageDelay; }
        set { cloudDamageDelay = value; }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            target.GetComponent<Monster>().TakeDamage(damage, ProjectileType.toxin);
            GameObject effect = Instantiate(hitEffect, other.transform);
            effect.transform.localPosition = Vector3.zero;
            ApplyDebuff();
            Destroy(gameObject);
        }
    }
    void ApplyDebuff()
    {
        int randomChance = Random.Range(0, 101);
        if(randomChance<=cloudChance)
        {
            ToxinCloud c = Instantiate(cloud, target.transform.position, Quaternion.identity);
            c.Initialize(cloudDamage,cloudDamageDelay,cloudTime);
        }
    }
}
