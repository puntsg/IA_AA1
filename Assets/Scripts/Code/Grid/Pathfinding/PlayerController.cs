using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridMap gridMap; // referencia al GridMap
    public float moveSpeed = 2f;

    private Queue<Node> pathQueue = new Queue<Node>();
    private bool isMoving = false;
    private Node currentTargetNode;

    void Update()
    {
        HandleInput();
        MoveAlongPath();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(1)) // click derecho
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node targetNode = gridMap.GetNodeFromWorldPos(mousePos);

            Node startNode = gridMap.GetNodeFromWorldPos(transform.position);

            List<Node> path = BFS(startNode, targetNode);
            if (path != null)
            {
                pathQueue = new Queue<Node>(path);
                isMoving = true;
            }
        }
    }

    void RecalculatePath()
    {
        Node startNode = gridMap.GetNodeFromWorldPos(transform.position);

        List<Node> newPath = BFS(startNode, currentTargetNode);
        if (newPath != null)
        {
            pathQueue = new Queue<Node>(newPath);
            isMoving = true;
        }
        else
        {
            pathQueue.Clear();
            isMoving = false;
        }
    }

    void MoveAlongPath()
    {
        if (isMoving && pathQueue.Count > 0)
        {
            Node target = pathQueue.Peek();

            if (!target.walkable)
            {
                RecalculatePath();
                return;
            }

            Vector3 targetPos = target.worldPos;
            targetPos.z = transform.position.z;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                pathQueue.Dequeue();
            }
        }
        else
        {
            isMoving = false;
        }
    }

    List<Node> BFS(Node startNode, Node targetNode)
    {
        if (!targetNode.walkable)
        {
            RecalculatePath();
        }

        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parentMap = new Dictionary<Node, Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current == targetNode)
            {
                // Reconstruir camino
                List<Node> path = new List<Node>();
                Node temp = targetNode;
                while (temp != startNode)
                {
                    path.Add(temp);
                    temp = parentMap[temp];
                }
                path.Reverse();
                return path;
            }

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (neighbor.walkable && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parentMap[neighbor] = current;
                }
            }
        }

        return null; // no hay camino
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int[,] directions = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } }; // solo 4 direcciones

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int nx = node.x + directions[i, 0];
            int ny = node.y + directions[i, 1];

            if (nx >= 0 && nx < gridMap.width && ny >= 0 && ny < gridMap.height)
            {
                neighbors.Add(gridMap.grid[nx, ny]);
            }
        }

        return neighbors;
    }
}
