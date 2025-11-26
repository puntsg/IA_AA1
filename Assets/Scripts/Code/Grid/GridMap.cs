using UnityEngine;


public class Node
{
    public int x, y;
    public Vector3 worldPos;
    public bool walkable;

    public int gCost, hCost;
    public Node parent;
    public float heuristic = 0.0f;
    public float cost = 1.0f;

    public int fCost => gCost + hCost;

    public Node(int x, int y, Vector3 worldPos, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.worldPos = worldPos;
        this.walkable = walkable;
    }
}



public class GridMap : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;
    public LayerMask obstacleMask;


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

            n.walkable = !n.walkable; // alternar estado
        }
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node n = GetNodeFromWorldPos(mousePos);
            if ( n.heuristic < 1.0f)
            n.heuristic += 0.1f;

            else { n.heuristic = 0.0f; }
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

                grid[x, y] = new Node(x, y, worldPos, walkable);
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

    void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (!node.walkable)
                {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * (cellSize * 0.9f));
                }
                if (node.heuristic > 0)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(node.worldPos, Vector3.one * (cellSize * 0.9f));
                }

            }
        }
    }

    public void SetWalkable(int x, int y, bool walkable)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y].walkable = walkable;
        }
    }
}

