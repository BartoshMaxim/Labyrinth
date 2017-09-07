using Assets.Scripts.Bonus;
using Assets.Scripts.Enemy;
using Assets.Scripts.GamePlayer;
using Assets.Scripts.Logger;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Session
{
    /// <summary>
    /// Current session
    /// </summary>
    public class Session : MonoBehaviour, ISession
    {
        public static readonly string sessionGameObjectName = "A*";

        private static string _tag = "Session";

        private UnityEngine.Logger _logger;

        /// <summary>
        /// Enemy list
        /// </summary>
        private List<IEnemy> _enemies;

        /// <summary>
        /// Bonus list
        /// </summary>
        private List<IBonus> _coins;

        private Grid _grid;

        private Timer _timer;

        private bool _coinControl = false;

        private System.Random _rand;

        /// <summary>
        /// Player object
        /// </summary>
        public GameObject playerGameObj;

        /// <summary>
        /// Zombie object
        /// </summary>
        public GameObject zombieGameObj;

        /// <summary>
        /// Mummy object
        /// </summary>
        public GameObject mummyGameObj;

        /// <summary>
        /// Coin object
        /// </summary>
        public GameObject coinGameObj;

        public bool SessionStart { get; set; }

        public IGamePlayer Player { get; set; }

        private void Start()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "Session Start.");

            _rand = new System.Random();
            _coins = new List<IBonus>();
            _enemies = new List<IEnemy>();
            _timer = new Timer(5000);
            _timer.Elapsed += coinGeneratorControl;
            SessionStart = false;
        }

        private void Update()
        {
            if (Maze.created && Player == null && Grid.gridCreated)
            {
                _grid = GetComponent<Grid>();
                _timer.Enabled = true;
                Player = new GamePlayer.GamePlayer(Menu.PlayerName, 3, createGameObject(playerGameObj));
                createZombie();
                SessionStart = true;
            }
            else
            {
                createCoin();
                createEnemies();
            }

            //exit
            if (Input.GetKey(KeyCode.Escape))
            {
                ExitWithSaveCoins("Escape");
            }
        }

        private void createEnemies()
        {
            if (Player.Coins == 5 && _enemies.Count == 1)
            {
                createZombie();
            }
            else if (Player.Coins == 10 && _enemies.Count == 2)
            {
                createMummy();
            }
        }

        private void coinGeneratorControl(object sender, ElapsedEventArgs e)
        {
            _coinControl = true;
        }

        private void createCoin()
        {
            if (_coins != null)
                if (_coins.Count < 10 && _coinControl)
                {
                    _coinControl = false;
                    _coins.Add(new Coin(coinGameObj, createGameObject));
                }
        }

        private void createZombie()
        {
            _enemies.Add(new Zombie(_enemies.Count, zombieGameObj, createGameObject));
        }

        private void createMummy()
        {
            _enemies.Add(new Mummy(_enemies.Count, mummyGameObj, createGameObject));
        }

        /// <summary>
        /// Func create gameObject in random position
        /// </summary>
        /// <returns>gameObject</returns>
        private GameObject createGameObject(GameObject gameObject)
        {
            try
            {
                GameObject gameObjectHolder = GameObject.Find(gameObject.name);
                if (gameObjectHolder == null)
                {
                    gameObjectHolder = new GameObject();
                    gameObjectHolder.name = gameObject.name;
                }
                Node coordinates = getRandomCoordinates();
                if (coordinates.walkable)
                {
                    GameObject player =
                Instantiate(gameObject,
                new Vector3(coordinates.worldPosition.x, 0.3f, coordinates.worldPosition.z),
                Quaternion.identity) as GameObject;
                    player.transform.parent = gameObjectHolder.transform;
                    return player;
                }
            }
            catch (System.Exception e)
            {
                _logger.LogException(e);
            }
            return null;
        }

        private Node getRandomCoordinates()
        {
            int[] coordinates = new int[2];
            try
            {
                while (true)
                {
                    coordinates[0] = _rand.Next(1, Mathf.RoundToInt(2 * _grid.gridWorldSize.x));
                    coordinates[1] = _rand.Next(1, Mathf.RoundToInt(2 * _grid.gridWorldSize.y));
                    if (Grid.grid[coordinates[0], coordinates[1]].walkable)
                        break;
                }
            }
            catch (System.Exception e)
            {
                _logger.LogException(e);
            }
            return Grid.grid[coordinates[0], coordinates[1]];
        }

        /// <summary>
        /// multiply enemy speed for 5%
        /// </summary>
        private void addEnemySpeed()
        {
            EnemyMoving.Speed = EnemyMoving.Speed + EnemyMoving.Speed * 0.05f;
        }

        public int GetCountEnemies()
        {
            return _enemies.Count;
        }

        /// <summary>
        /// Remove coin from Bonus list
        /// </summary>
        /// <param name="id">coin id</param>
        public void RemoveCoin(int id)
        {
            try
            {
                addEnemySpeed();
                _coins.Remove(_coins.First(coin => coin.Id == id));
            }
            catch (System.Exception e)
            {
                _logger.LogException(e);
            }
        }

        public void ExitWithSaveCoins(string nameDeath)
        {
            SaveResults(nameDeath);
        }

        public void ExitWithoutSaveCoins(string nameDeath)
        {
            Player.ResetCoins();
            ExitWithSaveCoins(nameDeath);
        }

        /// <summary>
        /// Save Results and start new scene
        /// </summary>
        public void SaveResults(string nameDeath)
        {
            try
            {
                Debug.Log("TimeCreate " + Player.TimeCreate);
                Debug.Log("Spend " + (DateTime.Now - Player.TimeCreate).ToString());
                Debug.Log("Name " + Player.Name);
                Debug.Log("Coins " + Player.Coins);
                Debug.Log("NameDeath " + nameDeath);
                SessionResult sessionResult = new SessionResult(Player.TimeCreate, (DateTime.Now - Player.TimeCreate), Player.Name, Player.Coins, nameDeath);
                Menu.SessionList.Sessions.Add(sessionResult);
                Menu.SessionList.Save();
            }
            catch (System.Exception e)
            {
                _logger.LogException(e);
            }
            SceneManager.LoadScene("Result");
        }
    }
}
