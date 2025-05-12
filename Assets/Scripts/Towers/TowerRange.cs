using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    [SerializeField] Tower tower;
    GameManager gm;
    private void Awake()
    {
        gm = GameManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && gm.ActiveMonsters.Contains(other.GetComponent<Monster>()))
            tower.AddTarget(other.gameObject.GetComponent<Monster>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
            tower.RemoveTarget(other.gameObject.GetComponent<Monster>());
    }
}
