using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gird : MonoBehaviour
{
    Node[,] grid;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;

    float nodeD;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeD = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeRadius);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeRadius);
        print("1");
        CreateGrid();
        print("2");
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2-Vector3.forward*gridWorldSize.y/2;



        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeD + nodeRadius) + Vector3.forward * (y * nodeD + nodeD);
                bool wanlable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                grid[x, y] = new Node(wanlable, worldPoint);
                
            }
        }
        print("3");
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.green : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeD - .1f));

            }
        }
    }
}
