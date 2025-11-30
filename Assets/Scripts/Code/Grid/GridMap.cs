using UnityEngine;


public class Node
{
    public int x, y;
    public Vector3 worldPos;

    public bool walkable;
    public bool walkableDijkstra;

    public int gCost, hCost;
    public Node parent;

    public float heuristic = 0.0f;
    public float cost = 1.0f;

    public int fCost => gCost + hCost;

    public GameObject visual;

    public Node(int x, int y, Vector3 worldPos, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.worldPos = worldPos;
        this.walkable = walkable;
    }

    public override bool Equals(object obj)
    {
        Node other = obj as Node;
        if (other == null) return false;
        return x == other.x && y == other.y;
    }
}



public class GridMap : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;
    public LayerMask obstacleMask;

    public GameObject cellPrefab; // prefab visual

    public Node[,] grid;

    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node n = GetNodeFromWorldPos(mousePos);

            n.walkable = !n.walkable;
            UpdateNodeVisual(n);
        }
        
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node n = GetNodeFromWorldPos(mousePos);

            if (n.heuristic < 1.0f)
                n.heuristic += 0.1f;
            else
                n.heuristic = 0.0f;

            UpdateNodeVisual(n);
        }
    }

    void CreateGrid()
    {
        grid = new Node[width, height];
        Vector3 origin = transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = origin + new Vector3(x * cellSize, y * cellSize, 0);
                bool walkable = !Physics2D.OverlapCircle(worldPos, cellSize * 0.4f, obstacleMask);

                Node node = new Node(x, y, worldPos, walkable);
                grid[x, y] = node;

                // Instanciar GameObject visual
                GameObject go = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                go.transform.localScale = Vector3.one * (cellSize * 0.9f);

                node.visual = go;

                UpdateNodeVisual(node);
            }
        }
    }

    public Node GetNodeFromWorldPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - transform.position.x) / cellSize);
        int y = Mathf.RoundToInt((worldPos.y - transform.position.y) / cellSize);

        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);

        return grid[x, y];
    }

    // Cambia color visual del nodo
    void UpdateNodeVisual(Node node)
    {
        SpriteRenderer sr = node.visual.GetComponent<SpriteRenderer>();

        if (!node.walkable)
        {
            sr.color = Color.red;
        }
        else if (node.heuristic > 0)
        {
            sr.color = Color.blue;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public void SetWalkable(int x, int y, bool walkable)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y].walkable = walkable;
            UpdateNodeVisual(grid[x, y]);
        }
    }
    public void PaintNode(Node n, Color c)
    {
        SpriteRenderer sr = n.visual.GetComponent<SpriteRenderer>();
        sr.color = c;
    }
    public void RefreshNode(Node n)
    {
        SpriteRenderer sr = n.visual.GetComponent<SpriteRenderer>();

        if (!n.walkable)
            sr.color = Color.red;
        else if (n.heuristic > 0)
            sr.color = Color.blue;
        else
            sr.color = Color.white;
    }

    public void PaintPathNode(Node node)
    {
        if (node.visual == null) return;
        node.visual.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void ResetAllNodeColors()
    {
        foreach (Node n in grid)
        {
            SpriteRenderer sr = n.visual.GetComponent<SpriteRenderer>();

            if (!n.walkable)
                sr.color = Color.red;
            else if (n.heuristic > 0)
                sr.color = Color.blue;
            else
                sr.color = Color.white;
        }
    }
}