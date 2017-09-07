using Assets.Scripts.Core;
using UnityEngine;

/// <summary>
/// The node from which the Grid is made up
/// </summary>
public class Node : IHeapItem<Node>
{
    private static string _tag = "Node";

    private int _heapIndex;

    public bool walkable;
    public Vector3 worldPosition;

    /// <summary>
    /// g(n) is the cost of the path from the start node to n
    /// </summary>
    public int gCost;

    /// <summary>
    /// h(n) is a heuristic that estimates the cost of the cheapest path from n to the goal
    /// </summary>
    public int hCost;

    public int gridX;
    public int gridY;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPosition,int _gridX,int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    /// <summary>
    /// Total cost
    /// </summary>
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return _heapIndex;
        }

        set
        {
            _heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
