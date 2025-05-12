using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinCloud : MonoBehaviour
{
    int damage;
    float delay;
    float time;
    List<Monster> insideTargets = new List<Monster>();
    public void Initialize(int damage, float delay, float time)
    {
        this.damage = damage;
        this.delay = delay;
        this.time = time;
        Destroy(gameObject, this.time);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            insideTargets.Add(other.GetComponent<Monster>());
            other.GetComponent<Monster>().ApplyDebuff(time, delay, damage, ProjectileType.toxin);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            insideTargets.Remove(other.GetComponent<Monster>());
            other.GetComponent<Monster>().CancelToxinDamage();
        }
    }
    private void OnDestroy()
    {
        foreach (Monster m in insideTargets)
            m.CancelToxinDamage();
        insideTargets.Clear();
    }
}
