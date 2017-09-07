using UnityEngine;
using Assets.Scripts.Session;
using Assets.Scripts.Logger;

namespace Assets.Scripts.Bonus
{
    /// <summary>
    /// Is responsible for the coin object animation and collision
    /// </summary>
    class CoinUpdate : MonoBehaviour
    {
        private static string _tag = "CoinUpdate";

        private UnityEngine.Logger _logger;

        /// <summary>
        /// Current session
        /// </summary>
        private ISession _session;

        public int Id;

        public int Income;

        void Start()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "CoinUpdate Start.");
            _session = GameObject.Find(Session.Session.sessionGameObjectName).GetComponent<Session.Session>();
        }

        void OnCollisionEnter(Collision col)
        {
            try
            {
                if (col.gameObject.name == "Player(Clone)")
                {
                    _session.Player.AddCoin(Income);
                    _session.RemoveCoin(Id);
                    Destroy(this.gameObject);
                }
            }
            catch(System.Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
}
