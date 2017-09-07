using Assets.Scripts.Logger;
using System;
using UnityEngine;

namespace Assets.Scripts.Bonus
{
    /// <summary>
    /// Is responsible for the coin object
    /// </summary>
    class Coin : IBonus
    {
        private static string tag = "Coin";

        private static int id = 0;

        private int _id;

        private UnityEngine.Logger _logger;

        /// <summary>
        /// Coin GameObject
        /// </summary>
        public GameObject _coin { get; set; }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _coin.GetComponent<CoinUpdate>().Id = _id;
            }
        }

        /// <summary>
        /// income from one coin
        /// </summary>
        public readonly int Income = 1;

        public Coin(GameObject coin, Func<GameObject, GameObject> create)
        {
            try
            {
                _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
                _logger.Log(tag, "Coin " + _id + " Create.");
                _coin = coin;
                Id = id;
                id++;
                _coin.GetComponent<CoinUpdate>().Income = Income;
                create(_coin);
            }
            catch (System.Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
}
