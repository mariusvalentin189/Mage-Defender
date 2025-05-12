using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealthUI : MonoBehaviour
{
    Monster monster;
    private void Awake()
    {
        monster = transform.parent.GetComponent<Monster>();
    }
    public void HideDamage()
    {
        monster.HideDamageText();
    }
}
