using Assets.Scripts.Logger;
using System;
using UnityEngine;

namespace Assets.Scripts.GamePlayer
{
    /// <summary>
    /// Main player
    /// </summary>
    public class GamePlayer : IGamePlayer
    {
        private static string _tag = "GamePlayer";

        private UnityEngine.Logger _logger;

        /// <summary>
        /// Player name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Player speed
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// TimeCreate player
        /// </summary>
        public DateTime TimeCreate { get; set; }

        /// <summary>
        /// Count coins
        /// </summary>
        private int _coins;

        /// <summary>
        /// Player
        /// </summary>
        private GameObject _player;

        public GameObject Player { get { return _player; } }

        public int Coins { get { return _coins; } }

        public GamePlayer() { }

        public GamePlayer(string name, float speed, GameObject player)
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "GamePlayer Creates.");

            TimeCreate = DateTime.Now;
            _coins = 0;
            _player = player;
            Name = name;
            Speed = speed;
        }

        /// <summary>
        /// Add coin
        /// </summary>
        /// <param name="i">income from bonus</param>
        public void AddCoin(int i)
        {
            _coins += i;
        }

        public void ResetCoins()
        {
            _coins = 0;
        }
    }
}
