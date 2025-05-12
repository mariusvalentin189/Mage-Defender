using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    [SerializeField] int tickDamage;
    [SerializeField] int tickChance;
    [SerializeField] float tickTime;
    [SerializeField] float tickDelay;
    public FireUpgrade[] upgrades;
    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        UpdateTarget();
        if (target != null)
        {
            FireProjectile p = Instantiate((FireProjectile)projectile, attackPoint.position, Quaternion.identity);
            am.PlaySound(hitSound);
            p.TickDamage = tickDamage;
            p.TickChance = tickChance;
            p.TickTime = tickTime;
            p.TickDelay = tickDelay;
            p.Damage = damage;
            p.target = target.transform;
        }
        StartCoroutine(Attack());
    }
    public override void UpgradeTower()
    {
        if (CurrentUpgrade < upgrades.Length)
        {
            gm.Currency-= upgrades[CurrentUpgrade].upgardeCost;
            range += upgrades[CurrentUpgrade].range;
            damage += upgrades[CurrentUpgrade].damage;
            attackCooldown -= upgrades[CurrentUpgrade].attackCooldown;
            tickDamage += upgrades[CurrentUpgrade].tickDamage;
            tickChance += upgrades[CurrentUpgrade].tickChance;
            cost += upgrades[CurrentUpgrade].upgardeCost;
            UpdateRange();
            CurrentUpgrade += 1;
        }
    }
    public override bool CheckNextUpgrade()
    {
        if (CurrentUpgrade < upgrades.Length)
            return true;
        return false;
    }
    public override int GetNextUpgrade()
    {
        if (CurrentUpgrade < upgrades.Length)
            return upgrades[CurrentUpgrade].upgardeCost;
        return 0;
    }
    public override string SetTowerInfo()
    {
        string tooltip = null;
        tooltip = "Fire Tower\nDamage: " + damage 
                + "\nRange: " + range 
                + "\nAttack Cooldown: " + attackCooldown + "s" 
                + "\nBurn Chance: " + tickChance + "%"
                + "\nBurn Damage: " + tickDamage;
        return tooltip;
    }
    public override string SetTowerUpgradeInfo()
    {
        string tooltip = null;
        if (CurrentUpgrade < upgrades.Length)
        {
            tooltip = "Fire Tower\nDamage: " + damage + "+" + upgrades[CurrentUpgrade].damage
                + "\nRange: " + range + "+" + upgrades[CurrentUpgrade].range
                + "\nAttack Cooldown: " + attackCooldown + "-" + upgrades[CurrentUpgrade].attackCooldown + "s"
                + "\nBurn Chance: " + tickChance + "+" + upgrades[CurrentUpgrade].tickChance + "%"
                + "\nBurn Damage: " + tickDamage + "+" + upgrades[CurrentUpgrade].tickDamage;
            return tooltip;
        }
        return "";
    }
    public override string SetTowerUnlockUpgradeInfo(int upgradeId)
    {
        string tooltip = null;
        tooltip = "Fire Tower\nDamage +" + upgrades[upgradeId].damage
            + "\nRange +" + upgrades[upgradeId].range
            + "\nAttack Cooldown -" + upgrades[upgradeId].attackCooldown + "s"
            + "\nBurn Chance +" + upgrades[upgradeId].tickChance + "%"
            + "\nBurn Damage +" + upgrades[upgradeId].tickDamage;
        return tooltip;
    }
}
