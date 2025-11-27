using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] enum PathfindingAlgorithm { GBFS, A, BFS, Dijkstra }
public class PlayerController : MonoBehaviour
{
    public GridMap gridMap; // referencia al GridMap
    public float moveSpeed = 2f;

    private Queue<Node> pathQueue = new Queue<Node>();
    private bool isMoving = false;
    private Node currentTargetNode;

    [SerializeField] PathfindingAlgorithm algorithm = PathfindingAlgorithm.BFS;
    void Update()
    {
        HandleInput();
        MoveAlongPath();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node targetNode = gridMap.GetNodeFromWorldPos(mousePos);
            Node startNode = gridMap.GetNodeFromWorldPos(transform.position);


            switch (algorithm)
            {
                case PathfindingAlgorithm.BFS:
                    pathQueue = new Queue<Node>(BFS(startNode, targetNode));
                    break;
                case PathfindingAlgorithm.Dijkstra:
                    pathQueue = new Queue<Node>(Dijkstra(startNode, targetNode));
                    break;
                case PathfindingAlgorithm.GBFS:
                    pathQueue = new Queue<Node>(GBFS(startNode, targetNode));
                    break;
                case PathfindingAlgorithm.A:
                    pathQueue = new Queue<Node>(A(startNode, targetNode));
                    break;
            }

            if (pathQueue.Count > 0)
                isMoving = true;
            currentTargetNode = targetNode;
        }
    }

    void RecalculatePath()
    {
        Node startNode = gridMap.GetNodeFromWorldPos(transform.position);

        switch (algorithm)
        {
            case PathfindingAlgorithm.BFS:
                pathQueue = new Queue<Node>(BFS(startNode, currentTargetNode));
                break;
            case PathfindingAlgorithm.Dijkstra:
                pathQueue = new Queue<Node>(Dijkstra(startNode, currentTargetNode));
                break;
            case PathfindingAlgorithm.GBFS:
                pathQueue = new Queue<Node>(GBFS(startNode, currentTargetNode));
                break;
            case PathfindingAlgorithm.A:
                pathQueue = new Queue<Node>(A(startNode, currentTargetNode));
                break;
        }

        if (pathQueue.Count > 0)
            isMoving = true;
        else
            isMoving = false;
    }

    void MoveAlongPath()
    {
        if (isMoving && pathQueue.Count > 0)
        {
            // Si algún nodo del camino ya no es caminable, recalcular
            if (pathQueue.Count > 0 && !pathQueue.Peek().walkable)
            {
                RecalculatePath();
                if (pathQueue.Count == 0)
                    return;
            }

            Node target = pathQueue.Peek();
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

    List<Node> GBFS(Node startNode, Node targetNode) {
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        HashSet<Node> visited = new HashSet<Node>();
        return null;
    }
    List<Node> A(Node startNode, Node targetNode)
    {
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        HashSet<Node> visited = new HashSet<Node>();
        return null;
    }
    List<Node> Dijkstra(Node startNode, Node targetNode)
        {
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        HashSet<Node> visited = new HashSet<Node>();

        // Inicializar distancias
        foreach (Node n in gridMap.grid)
            dist[n] = Mathf.Infinity;

        dist[startNode] = 0f;

        List<Node> pq = new List<Node> { startNode };

        while (pq.Count > 0)
        {
            // Obtener el nodo con menor distancia
            pq.Sort((a, b) => dist[a].CompareTo(dist[b]));
            Node current = pq[0];
            pq.RemoveAt(0);

            if (current == targetNode)
                return ReconstructPath(parent, startNode, targetNode);

            visited.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.walkable || visited.Contains(neighbor))
                    continue;

                float newDist = dist[current] + neighbor.cost;

                if (newDist < dist[neighbor])
                {
                    dist[neighbor] = newDist;
                    parent[neighbor] = current;

                    if (!pq.Contains(neighbor))
                        pq.Add(neighbor);
                }
            }
        }

        return null; // Sin camino
    }

    // Función para reconstruir el camino
    List<Node> ReconstructPath(Dictionary<Node, Node> parent, Node start, Node target)
    {
        List<Node> path = new List<Node>();
        Node current = target;

        while (current != start)
        {
            path.Add(current);
            current = parent[current];
        }

        path.Reverse();
        return path;
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
