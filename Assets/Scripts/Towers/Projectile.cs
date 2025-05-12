using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target { get; set; }
    [SerializeField] protected ProjectileType projectileType;
    [SerializeField] protected float speed;
    [SerializeField] protected GameObject hitEffect;
    protected int damage;
    Vector3 targetPos;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    protected void Start()
    {
        if (target != null)
        {
            targetPos = target.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    protected void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            target.GetComponent<Monster>().TakeDamage(damage, ProjectileType.normal);
            GameObject effect = Instantiate(hitEffect, other.transform);
            effect.transform.localPosition = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
public enum ProjectileType
{
    normal,
    fire,
    ice,
    toxin,
    thunder
}
