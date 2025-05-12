using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWaves : Singleton<GameWaves>
{
    public Waves[] waves;
    [Header("Enemies")]
    public Waypoint startWaypoint;
    public Transform spawnPosition;
}
