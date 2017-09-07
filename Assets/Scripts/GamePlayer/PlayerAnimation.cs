using Assets.Scripts.Enemy;
using Assets.Scripts.Logger;
using Assets.Scripts.Session;
using UnityEngine;

/// <summary>
/// Control player actions
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private static string _tag = "PlayerAnimation";

    private Animator _anim;

    private Camera _followCamera;

    private Logger _logger;

    /// <summary>
    /// Distance to main camera from player
    /// </summary>
    private readonly Vector3 offset = new Vector3(0, 9, -10);

    /// <summary>
    /// Current session
    /// </summary>
    private ISession _session;

    void Start()
    {
        _logger = new Logger(new LabyrinthLogHandler());
        _logger.Log(_tag, "PlayerAnimation Start.");

        _anim = GetComponent<Animator>();
        _followCamera = Camera.main;
        _session = GameObject.Find(Session.sessionGameObjectName).GetComponent<Session>();
    }


    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            _anim.SetFloat("Speed", 1);
        else
            _anim.SetFloat("Speed", 0);
        _followCamera.transform.position = transform.position + offset;
        Movement();
    }

    public void Movement()
    {
        try
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * _session.Player.Speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.right * _session.Player.Speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * _session.Player.Speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.back * _session.Player.Speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
        }
        catch(System.Exception e)
        {
            _logger.LogException(e);
        }
    }

    void OnCollisionEnter(Collision col)
    {
            if (col.gameObject.name == Zombie.ZombyObjectName)
            {
                _session.ExitWithSaveCoins(Zombie.ZombyClassName);
                Destroy(this);
            }
            else if (col.gameObject.name == Mummy.MummyObjectName)
            {
                _session.ExitWithoutSaveCoins(Mummy.MummyClassName);
                Destroy(this);
            } 
    }
}
