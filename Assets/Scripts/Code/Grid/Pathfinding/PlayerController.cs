using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] enum PathfindingAlgorithm { GBFS, A, BFS, Dijkstra }

public class PlayerController : MonoBehaviour
{
    public GridMap gridMap; 
    public float moveSpeed = 2f;

    [SerializeField] private Queue<Node> pathQueue = new Queue<Node>();
    [SerializeField] private bool isMoving = false;
    [SerializeField] private Node currentTargetNode;

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

            Node startNode;

            if (pathQueue.Count > 0)
            {
                Node[] arr = pathQueue.ToArray();
                startNode = arr[arr.Length - 1];
            }
            else
            {
                startNode = gridMap.GetNodeFromWorldPos(transform.position);
            }

            List<Node> newPathList = ComputePath(startNode, targetNode);
            if (newPathList == null || newPathList.Count == 0)
                return;

            if (pathQueue.Count > 0)
            {
                Node[] arr = pathQueue.ToArray();
                Node lastExisting = arr[arr.Length - 1];

                for (int i = 0; i < newPathList.Count; i++)
                {
                    Node n = newPathList[i];

                    if (i == 0 && n == lastExisting)
                        continue;

                    pathQueue.Enqueue(n);
                }
            }
            else
            {
                pathQueue = new Queue<Node>(newPathList);
            }

            currentTargetNode = targetNode;
            isMoving = pathQueue.Count > 0;

            PaintCurrentPath();
        }
    }

    void RecalculatePath()
    {
        if (currentTargetNode == null)
        {
            isMoving = false;
            pathQueue.Clear();
            PaintCurrentPath();
            return;
        }

        Node startNode = gridMap.GetNodeFromWorldPos(transform.position);

        List<Node> newPathList = ComputePath(startNode, currentTargetNode);

        if (newPathList == null || newPathList.Count == 0)
        {
            pathQueue.Clear();
            isMoving = false;
            PaintCurrentPath();
            return;
        }

        pathQueue = new Queue<Node>(newPathList);
        isMoving = true;

    }

    void MoveAlongPath()
    {
        if (isMoving && pathQueue.Count > 0)
        {
            if (!pathQueue.Peek().walkable)
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
                PaintCurrentPath();
            }
        }
        else
        {
            gridMap.ResetAllNodeColors();
            isMoving = false;
        }
    }
    void PaintCurrentPath()
    {

        if (pathQueue.Count == 0)
            return;

        Node[] arr = pathQueue.ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            gridMap.PaintNode(arr[i], new Color(1f, 0.7f, 0f));
        }

        gridMap.PaintNode(arr[arr.Length - 1], Color.green);
    }
    List<Node> ComputePath(Node startNode, Node targetNode)
    {
        switch (algorithm)
        {
            case PathfindingAlgorithm.BFS:
                return BFS(startNode, targetNode);
            case PathfindingAlgorithm.Dijkstra:
                return Dijkstra(startNode, targetNode);
            case PathfindingAlgorithm.GBFS:
                return GBFS(startNode, targetNode);
            case PathfindingAlgorithm.A:
                return A(startNode, targetNode);
        }
        return null;
    }


    int CompareByHeuristic(Node a, Node b)
    {
        return a.heuristic.CompareTo(b.heuristic);
    }
    List<Node> GBFS(Node startNode, Node targetNode)
    {
        List<Node> open = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

        open.Add(startNode);

        while (open.Count > 0)
        {
            open.Sort(CompareByHeuristic);

            Node current = open[0];
            open.RemoveAt(0);


            if (current.walkable)
                current.visual.GetComponent<SpriteRenderer>().color = Color.cyan;

            if (current == targetNode)
                return ReconstructPath(parent, startNode, targetNode);

            visited.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.walkable || visited.Contains(neighbor))
                    continue;

                if (!open.Contains(neighbor))
                {
                    parent[neighbor] = current;
                    open.Add(neighbor);
                }
            }
        }
        return null;
    }

    List<Node> Dijkstra(Node startNode, Node targetNode)
    {
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        HashSet<Node> visited = new HashSet<Node>();

        foreach (Node n in gridMap.grid)
            dist[n] = Mathf.Infinity;

        dist[startNode] = 0f;

        List<Node> pq = new List<Node> { startNode };

        while (pq.Count > 0)
        {
            pq.Sort((a, b) => dist[a].CompareTo(dist[b]));
            Node current = pq[0];
            pq.RemoveAt(0);


            if (current.walkable)
                current.visual.GetComponent<SpriteRenderer>().color = Color.cyan;

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

    List<Node> BFS(Node startNode, Node targetNode)
    {
        if (!targetNode.walkable)
            return null;

        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parentMap = new Dictionary<Node, Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if(current.walkable)
                current.visual.GetComponent<SpriteRenderer>().color = Color.cyan;

            if (current == targetNode)
            {
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

        return null; 
    }
    List<Node> A(Node startNode, Node targetNode)
    {
        if (!targetNode.walkable)
            return null;

        Dictionary<Node, float> gScore = new Dictionary<Node, float>();
        Dictionary<Node, float> fScore = new Dictionary<Node, float>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        
        foreach (Node n in gridMap.grid)
        {
            gScore[n] = Mathf.Infinity;
            fScore[n] = Mathf.Infinity;
        }
        
        gScore[startNode] = 0f;
        fScore[startNode] = Heuristic(startNode, targetNode);
        
        List<Node> openList = new List<Node> { startNode };

        while (openList.Count > 0)
        {
            openList.Sort((a, b) => fScore[a].CompareTo(fScore[b]));
            Node current = openList[0];
            openList.RemoveAt(0);
            
            if (current == targetNode)
                return ReconstructPath(parent, startNode, targetNode);

            closedSet.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;
                
                float tentativeGScore = gScore[current] + neighbor.cost;
                
                if (tentativeGScore < gScore[neighbor])
                {
                    parent[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, targetNode);

                    
                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }
        return null;
    }

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

    float Heuristic(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
