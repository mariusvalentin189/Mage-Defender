using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTile : MonoBehaviour
{
    bool hasTower;
    GameManager gm;
    Tower myTower;
    private void Start()
    {
        gm = GameManager.Instance;
    }
    private void OnMouseOver()
    {
        if (!gm.IsPaused)
        {
            if (gm.MouseObject)
            {
                GameObject mouseObject = gm.MouseObject;
                mouseObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            }
            if (!myTower)
                hasTower = false;
            if (!gm.CanBuyTower())
                gm.DeselectTower();
            if (Input.GetMouseButtonDown(0))
                if (!hasTower)
                    StartCoroutine(PlaceTower());
        }
    }
    IEnumerator PlaceTower()
    {
        if (!gm.IsOverButton)
        {
            if (gm.SelectedTower != null)
            {
                Tower tower = Instantiate(gm.SelectedTower, transform);
                GameObject towerShadow = Instantiate(gm.SelectedTower.selectTowerPrefab, transform);
                myTower = tower;
                gm.BuyTower(tower);
                tower.transform.localPosition = new Vector3(0, 0.5f, 0);
                towerShadow.transform.localPosition = new Vector3(0, 0.5f, 0);
                towerShadow.transform.GetChild(0).gameObject.SetActive(false);
                hasTower = true;
                tower.gameObject.SetActive(false);
                yield return new WaitForSeconds(gm.TowerBuildTime);
                Destroy(towerShadow);
                tower.gameObject.SetActive(true);
            }
            else
            {
                gm.HideTowerMenu();
            }
        }
    }
}
