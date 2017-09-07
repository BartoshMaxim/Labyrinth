using Assets.Scripts.Logger;
using Assets.Scripts.Session;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Score
{
    class Score : MonoBehaviour
    {
        private static string _tag = "Score";

        private UnityEngine.Logger _logger;

        private ISession _session;

        private Text _text;

        private void Start()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "Score Start.");

            _session = GameObject.Find(Session.Session.sessionGameObjectName).GetComponent<Session.Session>();
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            if (_session.SessionStart)
                _text.text = "Coins: " + _session.Player.Coins + "\n" + "\nEnemies: " + _session.GetCountEnemies();
        }
    }
}
