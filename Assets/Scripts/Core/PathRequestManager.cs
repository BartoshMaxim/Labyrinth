using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.Logger;

/// <summary>
/// Path Request Manager 
/// </summary>
public class PathRequestManager : MonoBehaviour
{

    private static string _tag = "PathRequestManager";

    private Logger _logger;

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        _logger = new Logger(new LabyrinthLogHandler());
        _logger.Log(_tag, "PathRequestManager Start.");
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);

            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();      
    }

    void TryProcessNext()
    {
        try
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }
        catch(Exception e)
        {
            _logger.LogException(e);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        try
        {
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }
        catch(Exception e)
        {
            _logger.LogException(e);
        }
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }

    }
}