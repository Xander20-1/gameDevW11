
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker;
    public Transform target;

    GridManager grid;

    void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    public void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        if (grid == null || grid.grid == null)
            return;

        // Reset data pencarian sebelumnya
        foreach (Node node in grid.grid)
        {
            node.gCost = 0;
            node.hCost = 0;
            node.parent = null;
        }

        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        if (!startNode.walkable || !targetNode.walkable)
        {
            grid.path = null;
            return;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost &&
                     openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newCost =
                    currentNode.gCost +
                    GetDistance(currentNode, neighbour);

                if (newCost < neighbour.gCost ||
                    !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        grid.path = null;
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> newPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;

            if (currentNode == null)
            {
                grid.path = null;
                return;
            }
        }

        newPath.Reverse();
        grid.path = newPath;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
