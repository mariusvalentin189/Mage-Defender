using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] Waypoint nextWaypoint;
    [SerializeField] List<Waypoint> branches = new List<Waypoint>();
    [SerializeField] int branchChance=50;
    [SerializeField] bool takeBranch;

    public Waypoint NextWaypoint()
    {
        if (takeBranch)
        {
            int i = Random.Range(0, 101);
            if (i <= branchChance)
            {
                int b = Random.Range(0, branches.Count);
                return branches[b];
            }
        }
        return nextWaypoint;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (nextWaypoint)
        {
            Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);
            foreach (Waypoint branch in branches)
                Gizmos.DrawLine(transform.position, branch.transform.position);
        }
    }
}
