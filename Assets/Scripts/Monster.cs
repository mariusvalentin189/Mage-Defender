using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class Monster : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Animator anim;
    [SerializeField] Animator healthAnim;
    [SerializeField] int maxHealth;
    [SerializeField] TMP_Text damageText;
    [SerializeField] int currencyReward;
    [SerializeField] int livesDamage;
    [Header("Effects")]
    [SerializeField] GameObject iceEffect;
    [SerializeField] GameObject fireEffect;
    [SerializeField] GameObject toxinEffect;
    [SerializeField] GameObject thunderEffect;
    GameManager gm;
    int currentHealth;
    Waypoint nextWaypoint;
    NavMeshAgent agent;
    bool isTakingFireDamage;
    bool isTakingToxinDamage;
    bool isSlowed;
    bool isStunned;
    float timeFire, timeIce, timeToxin,timeThunder;
    float speed;
    GameObject ice, fire, toxin,thunder;
    GameWaves gmw;
    void Start()
    {
        gm = GameManager.Instance;
        gmw = GameWaves.Instance;
        agent = GetComponent<NavMeshAgent>();
        nextWaypoint = gmw.startWaypoint;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        currentHealth = maxHealth;
        speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        healthBar.transform.parent.LookAt(Camera.main.transform);

        if (isSlowed)
        {
            if (timeIce > 0)
                timeIce -= Time.deltaTime;
            else
            {
                timeIce = 0;
                if (ice)
                    Destroy(ice);
                agent.speed = speed;
                isSlowed = false;
            }
        }
        if (isStunned)
        {
            if (timeThunder > 0)
                timeThunder -= Time.deltaTime;
            else
            {
                timeThunder = 0;
                if (thunder)
                    Destroy(thunder);
                agent.speed = speed;
                isStunned = false;
            }
        }
    }
    void Move()
    {
        if (agent != null && nextWaypoint!=null && agent.enabled && !isStunned)
        {
            agent.SetDestination(nextWaypoint.transform.position);
            if (!agent.pathPending && agent.stoppingDistance >= agent.remainingDistance)
                nextWaypoint = nextWaypoint.NextWaypoint();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EndPortal"))
        {
            gm.Lives -= livesDamage;
            gm.RemoveMonster(this, 0);
        }
    }
    public void TakeDamage(int damage, ProjectileType projectileType)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        switch (projectileType)
        {
            case ProjectileType.fire:
                {
                    damageText.color = Color.red;
                    break;
                }
            case ProjectileType.ice:
                {
                    damageText.color = Color.blue;
                    break;
                }
            case ProjectileType.toxin:
                {
                    damageText.color = Color.green;
                    break;
                }
            case ProjectileType.thunder:
                {
                    damageText.color = Color.magenta;
                    break;
                }
            case ProjectileType.normal:
                {
                    damageText.color = Color.white;
                    break;
                }
        }
        damageText.text = damage.ToString();
        healthAnim.SetTrigger("ShowDamage");
        anim.SetTrigger("TakeDamage");
        if (currentHealth <= 0 && agent.enabled)
        {
            gm.Currency += currencyReward;
            agent.isStopped = true;
            agent.enabled = false;
            anim.SetTrigger("Die");
            gm.RemoveMonster(this, 2f);
        }
    }
    public void ApplyDebuff(float time, float delay, int damage, ProjectileType projectile)
    {
        if (projectile == ProjectileType.fire)
        {
            if (isTakingFireDamage == false)
                StartCoroutine(TakeOvertimeFireDamage(time, delay, damage));
        }
        else if (projectile == ProjectileType.toxin)
        {
            if (isTakingToxinDamage == false)
                StartCoroutine(TakeOvertimeToxinDamage(time, delay, damage));
        }
    }
    IEnumerator TakeOvertimeFireDamage(float time, float delay, int damage)
    {
        isTakingFireDamage = true;
        if (fire == null)
        {
            fire = Instantiate(fireEffect, transform.position, Quaternion.identity);
            fire.transform.SetParent(transform);
        }
        timeFire = time;
        yield return new WaitForSeconds(delay);
        TakeDamage(damage, ProjectileType.fire);
        while (timeFire > 0 && isTakingFireDamage)
        {
            yield return new WaitForSeconds(delay);
            timeFire -= delay;
            if (currentHealth > 0 && agent.enabled)
            {
                TakeDamage(damage, ProjectileType.fire);
            }
            else break;
        }
        timeFire = 0;
        if (fire)
            Destroy(fire);
        isTakingFireDamage = false;
    }
    IEnumerator TakeOvertimeToxinDamage(float time, float delay, int damage)
    {
        isTakingToxinDamage = true;
        if (toxin == null)
        {
            toxin = Instantiate(toxinEffect, transform.position, Quaternion.identity);
            toxin.transform.SetParent(transform);
        }
        timeToxin = time;
        yield return new WaitForSeconds(delay);
        TakeDamage(damage, ProjectileType.toxin);
        while (timeToxin > 0 && isTakingToxinDamage)
        {
            yield return new WaitForSeconds(delay);
            if (currentHealth > 0 && agent.enabled)
            {
                TakeDamage(damage, ProjectileType.toxin);
            }
            else break;
        }
        isTakingToxinDamage = false;
    }
    public void CancelToxinDamage()
    {
        isTakingToxinDamage = false;
        if (toxin)
            Destroy(toxin);
    }
    public void ApplyIceDebuff(float time, float percentage)
    {
        if(!isSlowed)
        {
            ice=Instantiate(iceEffect, transform.position, Quaternion.identity);
            ice.transform.SetParent(transform);
            isSlowed = true;
            agent.speed = (agent.speed * percentage)/100;
            timeIce = time;
        }
    }
    public void ApplyThunderDebuff(float time)
    {
        if(!isStunned)
        {
            thunder = Instantiate(thunderEffect, transform.position, Quaternion.identity);
            thunder.transform.SetParent(transform);
            isStunned = true;
            agent.speed = 0;
            timeThunder = time;
        }
    }
    public void HideDamageText()
    {
        damageText.text = "";
    }
}
