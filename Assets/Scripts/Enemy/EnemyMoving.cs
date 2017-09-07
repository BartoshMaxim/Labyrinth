using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Session;
using Assets.Scripts.Enemy;
using System.Timers;
using Assets.Scripts.Logger;

/// <summary>
/// Control enemies actions
/// </summary>
public class EnemyMoving : MonoBehaviour
{
    private static string _tag = "EnemyMoving";

    private Logger _logger;

    /// <summary>
    /// Current session
    /// </summary>
    private ISession _session;
    private bool _findPath = true;
    private System.Random _rand;
    private Animator _anim;
    private Vector3[] _path;
    private int _targetIndex;
    private Timer _timer;

    public static float Speed;
    

    /// <summary>
    /// Manage object Rotate
    /// </summary>
    private float _xPosCurrent;
    private float _xPosLater;
    private bool _canCheckRoute = false;

    private void Start()
    {
        _logger = new Logger(new LabyrinthLogHandler());
        _logger.Log(_tag, "EnemyMoving Start.");

        _anim = GetComponent<Animator>();

        Speed = 0.01f;

        _rand = new System.Random();

        _session = GameObject.Find(Session.sessionGameObjectName).GetComponent<Session>();

        _timer = new Timer(100);
        _timer.Elapsed += Rotate;
        _timer.Enabled = true;

        _xPosCurrent = _xPosLater = transform.position.x;
    }

    private void Update()
    {
        RotateObject();
        if (_findPath)
        {
            _findPath = false;
            if (_session.Player.Coins >= 20)
                PathRequestManager.RequestPath(transform.position, _session.Player.Player.transform.position, OnPathFound);
            else
                PathRequestManager.RequestPath(transform.position, randomMove(), OnPathFound);
        }
        _xPosCurrent = transform.position.x;
        СhangeAnimationOrKillPlayer();
    }

    #region rotate_manage
    private void Rotate(object sender, ElapsedEventArgs e)
    {
        _canCheckRoute = true;
    }

    private void RotateObject()
    {
        if (_canCheckRoute)
        {
            _canCheckRoute = false;
            if (_xPosCurrent > _xPosLater)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            _xPosLater = _xPosCurrent;
        }
    }
    #endregion

    /// <summary>
    /// Control actions at a distance
    /// </summary>
    private void СhangeAnimationOrKillPlayer()
    {
        try
        {
            float distance = Vector3.Distance(_session.Player.Player.transform.position, transform.position);
            if (distance < 1)
            {
                if (name == Zombie.ZombyObjectName)
                    _session.ExitWithSaveCoins(Zombie.ZombyClassName);
                else if (name == Mummy.MummyObjectName)
                    _session.ExitWithoutSaveCoins(Mummy.MummyClassName);
            }

            if (distance < 4)
            {
                _anim.SetBool("PlayAttack", true);
            }
            else
                _anim.SetBool("PlayAttack", false);
        }
        catch(Exception e)
        {
            _logger.LogException(e);
        }
    }

    /// <summary>
    /// Create random walkable Vector3
    /// </summary>
    /// <returns>Random Vector3</returns>
    private Vector3 randomMove()
    {
        int x = 0;
        int y = 0;
        while (true)
        {
            x = _rand.Next(1, Mathf.RoundToInt(Grid.GridWorldX));
            y = _rand.Next(1, Mathf.RoundToInt(Grid.GridWorldY));
            if (Grid.grid[x, y].walkable)
                break;
        }
        return new Vector3(Grid.grid[x, y].worldPosition.x, 1f, Grid.grid[x, y].worldPosition.z);

    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;
            _targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = new Vector3();
        bool fail = false;
        try
        {
            currentWaypoint = _path[0];
        }
        catch (Exception e)
        {
            fail = true;
        }
        if (!fail)
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Length)
                    {
                        _findPath = true;
                        yield break;
                    }
                    currentWaypoint = _path[_targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, getSpeed());
                yield return null;
            }
        _findPath = true;
        yield return null;
    }

    /// <summary>
    /// Manage objects speed
    /// </summary>
    /// <returns>object speed</returns>
    private float getSpeed()
    {
        if (name == Zombie.ZombyObjectName)
        {
            return Speed;
        }
        else if (name == Mummy.MummyObjectName)
        {
            return 2 * Speed;
        }
        return Speed;
    }
}