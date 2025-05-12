using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    fire,
    ice,
    toxin,
    thunder,
    normal
}
public class Tower : MonoBehaviour
{
    public TowerType towerType;
    public int cost;
    public int unlockCost;
    public GameObject selectTowerPrefab;
    [SerializeField] protected float range;
    [SerializeField] protected int damage;
    [SerializeField] protected MeshRenderer rangeRenderer;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected AudioClip hitSound;
    public int CurrentUpgrade { get; set; }
    protected Monster target;
    protected GameManager gm;
    protected AudioManager am;
    bool selected;
    bool mouseOverTower;
    public List<Monster> MonstersInRange { get; set; } = new List<Monster>();
    private void Start()
    {
        gm = GameManager.Instance;
        am = AudioManager.Instance;
        rangeRenderer.transform.localScale = new Vector3(range, 0.1f, range);
        StartCoroutine(Attack());
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mouseOverTower && !gm.IsOverButton)
        {
            rangeRenderer.enabled = false;
            if (selected)
                SelectTower();
        }
    }
    private void OnMouseOver()
    {
        mouseOverTower = true;
        if (Input.GetMouseButtonDown(0))
            SelectTower();
    }
    private void OnMouseExit()
    {
        mouseOverTower = false;
    }
    void SelectTower()
    {
        if (!gm.IsOverButton && !gm.IsPaused)
        {
            if (selected)
            {
                if (gm.HighlightedTower == null || gm.HighlightedTower == this)
                    gm.HideTowerMenu();
                rangeRenderer.enabled = false;
                selected = false;
                return;
            }
            else
            {
                rangeRenderer.enabled = true;
                gm.DeselectTower();
                gm.HighlightedTower = this;
                gm.ShowTowerMenu();
                selected = true;
                return;
            }
        }
    }
    public void DeselectTower()
    {
        gm.HideTowerMenu();
        rangeRenderer.enabled = false;
        selected = false;
    }
    public void UpdateTarget()
    {
        if (MonstersInRange.Count > 0)
        {
            target = MonstersInRange[0];
            float distance = Vector3.Distance(target.transform.position, transform.position);
            foreach (Monster monster in MonstersInRange)
            {
                if (Vector3.Distance(monster.transform.position, transform.position) < distance)
                target = monster;
            }
        }
        else target = null;
    }
    protected virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        UpdateTarget();
        if (target != null)
        {
            Projectile p = Instantiate(projectile, attackPoint.position, Quaternion.identity);
            am.PlaySound(hitSound);
            p.Damage = damage;
            p.target = target.transform;
        }
        StartCoroutine(Attack());
    }
    public void AddTarget(Monster monster)
    {
        MonstersInRange.Add(monster);
    }
    public void RemoveTarget(Monster monster)
    {
        MonstersInRange.Remove(monster);
    }
    public virtual string SetTowerInfo() { return null; }
    public virtual string SetTowerUpgradeInfo() { return null; }
    public virtual string SetTowerUnlockUpgradeInfo(int id) { return null; }
    public void UpdateRange()
    {
        rangeRenderer.transform.localScale = new Vector3(range, 0.1f, range);
    }
    public virtual void UpgradeTower() { }
    public virtual bool CheckNextUpgrade(){ return false; }
    public virtual int GetNextUpgrade() { return 0; }
}
