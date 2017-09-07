using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.Logger;
using Assets.Scripts.Exception;

/// <summary>
/// Grid on which the objects are placed
/// </summary>
public class Grid : MonoBehaviour
{
    private static string _tag = "Grid";

    private Logger _logger;

    /// <summary>
    /// Display grid
    /// </summary>
    public bool displayGridGizmos;

    public LayerMask unwalkableMask;

    public Vector2 gridWorldSize;

    public float nodeRadius;

    public static Node[,] grid;

    public static bool gridCreated = false;
    /// <summary>
    /// Static grid world size by X axis
    /// </summary>
    public static int GridWorldX { get; set; }
    /// <summary>
    /// Static grid world size by Y axis
    /// </summary>
    public static int GridWorldY { get; set; }

    float nodeDiameter;

    int gridSizeX, gridSizeY;


    private void Start()
    {
        _logger = new Logger(new LabyrinthLogHandler());
        _logger.Log(_tag, "Grid Start.");
        grid = null;
        gridCreated = false;
    }
    void Update()
    {
        if (!gridCreated && Maze.created)
        {
            try
            {
                nodeDiameter = nodeRadius * 2;
                gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
                gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
                CreateGrid();
                GridWorldX = Mathf.RoundToInt(gridWorldSize.x);
                GridWorldY = Mathf.RoundToInt(gridWorldSize.y);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }
    }


    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    /// <summary>
    /// Create main Grid
    /// </summary>
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                worldPoint = new Vector3(worldPoint.x, 0.3f, worldPoint.z);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
        gridCreated = true;
    }

    /// <summary>
    /// Get Neighbours around node
    /// </summary>
    /// <param name="node">node around which gets the neighbors</param>
    /// <returns>List with nodes</returns>
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        try
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if ((x == 0 && y == 0) || (x != 0 && y != 0))
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogException(e);
        }

        return neighbours;
    }

    /// <summary>
    /// Gets node from world position
    /// </summary>
    /// <param name="worldPosition">world position which gets the node</param>
    /// <returns>Node on which the position is located</returns>
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        if (x >= gridSizeX || y >= gridSizeY)
        {
            _logger.LogError(_tag, "Out of range exception");
            throw new LabyrinthException();
        }

        return grid[x, y];
    }

    /// <summary>
    /// Draw Grid Gizmos
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }


}