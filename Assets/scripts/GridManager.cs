
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public LayerMask obstacleMask;

    public Vector2 gridWorldSize = new Vector2(20, 20);
    public float nodeRadius = 0.7f;

    public Node[,] grid;

    public List<Node> path;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft =
            transform.position
            - Vector3.right * gridWorldSize.x / 2
            - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint =
                    worldBottomLeft
                    + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !Physics.CheckSphere(
                    worldPoint,
                    nodeRadius,
                    obstacleMask
                );

                grid[x, y] = new Node(
                    walkable,
                    worldPoint,
                    x,
                    y
                );
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        Vector3 worldBottomLeft =
            transform.position
            - Vector3.right * gridWorldSize.x / 2
            - Vector3.forward * gridWorldSize.y / 2;

        int x = Mathf.FloorToInt(
            (worldPosition.x - worldBottomLeft.x) / nodeDiameter
        );

        int y = Mathf.FloorToInt(
            (worldPosition.z - worldBottomLeft.z) / nodeDiameter
        );

        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX &&
                    checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node GetNearestWalkableNode(Node node)
    {
        if (node.walkable)
            return node;

        for (int radius = 1; radius < Mathf.Max(gridSizeX, gridSizeY); radius++)
        {
            Node best = null;
            int bestDistance = int.MaxValue;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Mathf.Abs(x) != radius && Mathf.Abs(y) != radius)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX < 0 || checkX >= gridSizeX ||
                        checkY < 0 || checkY >= gridSizeY)
                        continue;

                    Node candidate = grid[checkX, checkY];

                    if (!candidate.walkable)
                        continue;

                    int distance = x * x + y * y;

                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        best = candidate;
                    }
                }
            }

            if (best != null)
                return best;
        }

        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(gridWorldSize.x, 0.1f, gridWorldSize.y)
        );

        if (grid == null)
            return;

        foreach (Node node in grid)
        {
            Gizmos.color = node.walkable ? Color.white : Color.red;

            Gizmos.DrawCube(
                node.worldPosition,
                Vector3.one * (nodeDiameter - 0.05f)
            );
        }

        if (path != null)
        {
            Gizmos.color = Color.black;

            foreach (Node node in path)
            {
                Gizmos.DrawCube(
                    node.worldPosition,
                    Vector3.one * (nodeDiameter - 0.05f)
                );
            }
        }
    }
}
