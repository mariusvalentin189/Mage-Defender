using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinTower : Tower
{
    [SerializeField] int cloudChance;
    [SerializeField] int cloudDamage;
    [SerializeField] float cloudDamageDelay;
    [SerializeField] float cloudTime;
    public ToxinUpgrade[] upgrades;
    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        UpdateTarget();
        if (target != null)
        {
            ToxinProjectile p = Instantiate((ToxinProjectile)projectile, attackPoint.position, Quaternion.identity);
            am.PlaySound(hitSound);
            p.Damage = damage;
            p.CloudChance = cloudChance;
            p.CloudDamage = cloudDamage;
            p.CloudTime = cloudTime;
            p.CloudDamageDelay = cloudDamageDelay;
            p.target = target.transform;
        }
        StartCoroutine(Attack());
    }
    public override void UpgradeTower()
    {
        if (CurrentUpgrade < upgrades.Length)
        {
            gm.Currency -= upgrades[CurrentUpgrade].upgardeCost;
            range += upgrades[CurrentUpgrade].range;
            damage += upgrades[CurrentUpgrade].damage;
            attackCooldown -= upgrades[CurrentUpgrade].attackCooldown;
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
        tooltip = "Toxin Tower\nDamage: " + damage
                + "\nRange: " + range
                + "\nAttack Cooldown: " + attackCooldown + "s"
                + "\nToxin Cloud Time: " + cloudTime + "s"
                + "\nToxin Cloud Damage Cooldown: " + cloudDamageDelay + "s"
                + "\nToxin Cloud Chance: " + cloudChance + "%"
                + "\nToxin Cloud Damage: " + cloudDamage;
        return tooltip;
    }
    public override string SetTowerUpgradeInfo()
    {
        string tooltip = null;
        if (CurrentUpgrade < upgrades.Length)
        {
            tooltip = "Toxin Tower\nDamage: " + damage + "+" + upgrades[CurrentUpgrade].damage
                + "\nRange: " + range + "+" + upgrades[CurrentUpgrade].range
                + "\nAttack Cooldown: " + attackCooldown + "-" + upgrades[CurrentUpgrade].attackCooldown + "s"
                + "\nToxin Cloud Time: " + cloudTime + "+" + upgrades[CurrentUpgrade].cloudTime + "s"
                + "\nToxin Cloud Damage Cooldown: " + cloudDamageDelay + "-" + upgrades[CurrentUpgrade].cloudDamageDelay + "s"
                + "\nToxin Cloud Chance: " + cloudChance + "+" + upgrades[CurrentUpgrade].cloudChance + "%"
                + "\nToxin Cloud Damage: " + cloudDamage + "+" + upgrades[CurrentUpgrade].cloudDamage;
            return tooltip;
        }
        return "";
    }
    public override string SetTowerUnlockUpgradeInfo(int upgradeId)
    {
        string tooltip = null;
        tooltip = "Toxin Tower\nDamage +" + upgrades[upgradeId].damage
                + "\nRange +" + upgrades[upgradeId].range
                + "\nAttack Cooldown -" + upgrades[upgradeId].attackCooldown + "s"
                + "\nToxin Cloud Time +" + upgrades[upgradeId].cloudTime + "s"
                + "\nToxin Cloud Damage Cooldown -" + upgrades[upgradeId].cloudDamageDelay + "s"
                + "\nToxin Cloud Chance +" + upgrades[upgradeId].cloudChance + "%"
                + "\nToxin Cloud Damage +" + upgrades[upgradeId].cloudDamage;
        return tooltip;
    }
}
