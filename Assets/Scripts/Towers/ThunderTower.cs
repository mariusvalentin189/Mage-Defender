using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderTower : Tower
{
    [SerializeField] int stunChance;
    [SerializeField] float stunTime;
    public ThunderUpgrade[] upgrades;
    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        UpdateTarget();
        if (target != null)
        {
            ThunderProjectile p = Instantiate((ThunderProjectile)projectile, attackPoint.position, Quaternion.identity);
            am.PlaySound(hitSound);
            p.StunChance = stunChance;
            p.StunTime = stunTime;
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
            stunChance += upgrades[CurrentUpgrade].stunChance;
            stunTime += upgrades[CurrentUpgrade].stunTime;
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
        tooltip = "Thunder Tower\nDamage: " + damage
                + "\nRange: " + range
                + "\nAttack Cooldown: " + attackCooldown + "s"
                + "\nStun Chance: " + stunChance + "%"
                + "\nStun Time: " + stunTime + "s";
        return tooltip;
    }
    public override string SetTowerUpgradeInfo()
    {
        if (CurrentUpgrade < upgrades.Length)
        {
            string tooltip = null;
            tooltip = "Thunder Tower\nDamage: " + damage + "+" + upgrades[CurrentUpgrade].damage
                    + "\nRange: " + range + "+" + upgrades[CurrentUpgrade].range
                    + "\nAttack Cooldown: " + attackCooldown + "-" + upgrades[CurrentUpgrade].attackCooldown + "s"
                    + "\nStun Chance: " + stunChance + "+" + upgrades[CurrentUpgrade].stunChance + "%"
                    + "\nStun Time: " + stunTime + "+" + upgrades[CurrentUpgrade].stunTime + "%";
            return tooltip;
        }
        return "";
    }
    public override string SetTowerUnlockUpgradeInfo(int upgradeId)
    {
        string tooltip = null;
        tooltip = "Thunder Tower\nDamage +" + upgrades[upgradeId].damage
                + "\nRange +" + upgrades[upgradeId].range
                + "\nAttack Cooldown -" + upgrades[upgradeId].attackCooldown + "s"
                + "\nStun Chance +" + upgrades[upgradeId].stunChance + "%"
                + "\nStun Time +" + upgrades[upgradeId].stunTime + "%";
        return tooltip;
    }
}
