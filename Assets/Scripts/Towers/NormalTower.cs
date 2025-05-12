using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTower : Tower
{
    public Upgrade[] upgrades;
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
        tooltip = "Normal Tower\nDamage: " + damage
                + "\nRange: " + range
                + "\nAttack Cooldown: " + attackCooldown + "s";
        return tooltip;
    }
    public override string SetTowerUpgradeInfo()
    {
        string tooltip = null;
        if (CurrentUpgrade < upgrades.Length)
        {
            tooltip = "Normal Tower\nDamage: " + damage + "+" + upgrades[CurrentUpgrade].damage
                + "\nRange: " + range + "+" + upgrades[CurrentUpgrade].range
                + "\nAttack Cooldown: " + attackCooldown + "-" + upgrades[CurrentUpgrade].attackCooldown + "s";
            return tooltip;
        }
        return "";
    }
    public override string SetTowerUnlockUpgradeInfo(int upradeId)
    {
        string tooltip = null;
        tooltip = "Normal Tower\nDamage +" + upgrades[upradeId].damage
                + "\nRange +" + upgrades[upradeId].range
                + "\nAttack Cooldown -" + upgrades[upradeId].attackCooldown + "s";
        return tooltip;
    }
}
