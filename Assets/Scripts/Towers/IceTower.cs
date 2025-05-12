using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower
{
    [SerializeField] int slowPercentage;
    [SerializeField] int slowChance;
    [SerializeField] float slowTime;
    public IceUpgrade[] upgrades;
    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        UpdateTarget();
        if (target != null)
        {
            IceProjectile p = Instantiate((IceProjectile)projectile, attackPoint.position, Quaternion.identity);
            am.PlaySound(hitSound);
            p.SlowPercentage = slowPercentage;
            p.SlowChance = slowChance;
            p.SlowTime = slowTime;
            p.Damage = damage;
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
            slowPercentage += upgrades[CurrentUpgrade].slowPercentage;
            slowChance += upgrades[CurrentUpgrade].slowChance;
            slowTime += upgrades[CurrentUpgrade].slowTime;
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
        tooltip = "Ice Tower\nDamage: " + damage 
                + "\nRange: " + range 
                + "\nAttack Cooldown: " + attackCooldown + "s"
                + "\nSlow Chance: " + slowChance + "%"
                + "\nSlow Time: " + slowTime + "s"
                + "\nSlow Percentage: " + slowPercentage + "%";
        return tooltip;
    }
    public override string SetTowerUpgradeInfo()
    {
        string tooltip = null;
        if (CurrentUpgrade < upgrades.Length)
        {
            tooltip = "Ice Tower\nDamage: " + damage + "+" + upgrades[CurrentUpgrade].damage
                + "\nRange: " + range + "+" + upgrades[CurrentUpgrade].range
                + "\nAttack Cooldown: " + attackCooldown + "-" + upgrades[CurrentUpgrade].attackCooldown + "s"
                + "\nSlow Chance: " + slowChance + "+" + upgrades[CurrentUpgrade].slowChance + "%"
                + "\nSlow Time: " + slowTime + "+" + upgrades[CurrentUpgrade].slowTime + "s"
                + "\nSlow Percentage: " + slowPercentage + "+" + upgrades[CurrentUpgrade].slowPercentage + "%";
            return tooltip;
        }
        return "";
    }
    public override string SetTowerUnlockUpgradeInfo(int upgradeId)
    {
        string tooltip = null;
        tooltip = "Ice Tower\nDamage +" + upgrades[upgradeId].damage
            + "\nRange +" + upgrades[upgradeId].range
            + "\nAttack Cooldown -" + upgrades[upgradeId].attackCooldown + "s"
            + "\nSlow Chance +" + upgrades[upgradeId].slowChance + "%"
            + "\nSlow Time +" + upgrades[upgradeId].slowTime + "s"
            + "\nSlow Percentage +" + upgrades[upgradeId].slowPercentage + "%";
        return tooltip;
    }
}
