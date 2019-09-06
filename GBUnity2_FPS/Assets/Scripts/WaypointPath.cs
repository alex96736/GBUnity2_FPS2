using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public List<Transform> nodes = new List<Transform>();
    public Transform[] pathTransform;

    public Vector3 currNode;
    public Vector3 prevNode;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        pathTransform = GetComponentsInChildren<Transform>();
        if (pathTransform.Length > 0)
        {
            foreach (Transform t in pathTransform)
            {
                if (!nodes.Contains(t) && t != transform)
                {
                    nodes.Add(t);
                }
            }
        }

        if (nodes.Count >= 2)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                currNode = nodes[i].position;
                if (i > 0)
                {
                    prevNode = nodes[i - 1].position;
                }
                else if (i == 0 && nodes.Count > 1)
                {
                    prevNode = nodes[nodes.Count - 1].position;
                }

                Gizmos.DrawLine(prevNode, currNode);
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(currNode, Vector3.one);
            }
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
