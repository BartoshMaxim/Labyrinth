using Assets.Scripts.Core;
using Assets.Scripts.Logger;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object of this class creates random Maze
/// </summary>
public class Maze : MonoBehaviour
{
    private static string _tag = "Maze";

    private Cell[] _cells;
    private int _currentNeighbour = 0;
    private int _totalCells = 0;
    private int _visitedCells = 0;
    private bool _startedBuilding = false;
    private List<int> _lastCells;
    private int _backingUp = 0;
    private int _wallToBreak = 0;
    private Vector3 _initialPos;
    private Logger _logger;

    public static bool created = false;
    public GameObject wall;
    public GameObject wallHolder;
    public float wallLenght = 2.0f;
    public int xSize = 15;
    public int ySize = 15;
    public int currentCell = 0;
    

    private void Start()
    {
        _logger = new Logger(new LabyrinthLogHandler());
        _logger.Log(_tag, "Maze Start.");

        try
        {
            CreateWalls();
        }
        catch(System.Exception e)
        {
            _logger.LogException(e);
        }
        created = true;
    }

    void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        _initialPos = new Vector3((-xSize) + wallLenght / 2, 0.0f, (-ySize) + wallLenght);
        Vector3 myPos = _initialPos;
        GameObject tempWall;

        //For x Axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(_initialPos.x + (j * wallLenght) - wallLenght / 2, 0.0f, _initialPos.z + (i * wallLenght) - wallLenght / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //For y Axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(_initialPos.x + (j * wallLenght), 0.0f, _initialPos.z + (i * wallLenght) - wallLenght);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }
        CreateCells();
    }

    void CreateCells()
    {
        _lastCells = new List<int>();
        _lastCells.Clear();
        _totalCells = xSize * ySize;

        GameObject[] allWalls;
        int children = wallHolder.transform.childCount;
        int eastWestProccess = 0;
        int childProccess = 0;
        int termCount = 0;

        allWalls = new GameObject[children];
        _cells = new Cell[xSize * ySize];

        //Gets All the Children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }
        //Assigns walls to the cells
        for (int cellprocess = 0; cellprocess < _cells.Length; cellprocess++)
        {
            _cells[cellprocess] = new Cell();
            _cells[cellprocess].east = allWalls[eastWestProccess];
            _cells[cellprocess].south = allWalls[childProccess + (xSize + 1) * ySize];
            if (termCount == xSize)
            {
                eastWestProccess += 2;
                termCount = 0;
            }
            else
                eastWestProccess++;
            termCount++;
            childProccess++;
            _cells[cellprocess].west = allWalls[eastWestProccess];
            _cells[cellprocess].north = allWalls[(childProccess + (xSize + 1) * ySize) + xSize - 1];
        }
        CreateMaze();
    }

    void CreateMaze()
    {

        while (_visitedCells < _totalCells)
        {
            if (_startedBuilding)
            {
                GiveMeNeighbour();
                if (_cells[_currentNeighbour].visited == false && _cells[currentCell].visited == true)
                {
                    BreakWall();
                    _cells[_currentNeighbour].visited = true;
                    _visitedCells++;
                    _lastCells.Add(currentCell);
                    currentCell = _currentNeighbour;
                    if (_lastCells.Count > 0)
                    {
                        _backingUp = _lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, _totalCells);
                _cells[currentCell].visited = true;
                _visitedCells++;
                _startedBuilding = true;
            }
        }
    }
    
    void BreakWall()
    {
        switch (_wallToBreak)
        {
            case 1:
                Destroy(_cells[currentCell].north);
                break;
            case 2:
                Destroy(_cells[currentCell].east);
                break;
            case 3:
                Destroy(_cells[currentCell].west);
                break;
            case 4:
                Destroy(_cells[currentCell].south);
                break;
        }
    }

    void GiveMeNeighbour()
    {

        int lenght = 0;
        int[] neighbours = new int[4];
        int check = 0;
        int[] connectingWall = new int[4];

        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //west
        if (currentCell + 1 < _totalCells && (currentCell + 1) != check)
        {
            if (_cells[currentCell + 1].visited == false)
            {
                neighbours[lenght] = currentCell + 1;
                connectingWall[lenght] = 3;
                lenght++;
            }
        }

        //east
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (_cells[currentCell - 1].visited == false)
            {
                neighbours[lenght] = currentCell - 1;
                connectingWall[lenght] = 2;
                lenght++;
            }
        }

        //north
        if (currentCell + xSize < _totalCells)
        {
            if (_cells[currentCell + xSize].visited == false)
            {
                neighbours[lenght] = currentCell + xSize;
                connectingWall[lenght] = 1;
                lenght++;
            }
        }

        //south
        if (currentCell - xSize >= 0)
        {
            if (_cells[currentCell - xSize].visited == false)
            {
                neighbours[lenght] = currentCell - xSize;
                connectingWall[lenght] = 4;
                lenght++;
            }
        }
        if (lenght != 0)
        {
            int theChosenOne = Random.Range(0, lenght);
            _currentNeighbour = neighbours[theChosenOne];
            _wallToBreak = connectingWall[theChosenOne];
        }
        else
        {
            if (_backingUp > 0)
            {
                currentCell = _lastCells[_backingUp];
                _backingUp--;
            }
        }
    }
}
