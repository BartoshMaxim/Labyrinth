using Assets.Scripts.Logger;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Session
{
    /// <summary>
    /// Session Result Animation(Result scene)
    /// </summary>
    class SessionResultAnimation : MonoBehaviour
    {
        private static string _tag = "SessionResultAnimation";

        private UnityEngine.Logger _logger;

        Text text;
        private void Start()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "SessionResultAnimation Start.");
            try
            {
                text = GetComponent<Text>();
                if (Menu.SessionList == null)
                {
                    text.text = "No Data";
                    return;
                }
                else
                if (Menu.SessionList.Sessions.Count != 0)
                    text.text = getLastObjectInformation();
                else
                    text.text = "No Data";
            }
            catch(System.Exception e)
            {
                _logger.LogException(e);
            }
        }

        /// <summary>
        /// Get info about last session
        /// </summary>
        /// <returns>info about last session</returns>
        private string getLastObjectInformation()
        {
            SessionResult result = Menu.SessionList.Sessions[Menu.SessionList.Sessions.Count - 1];
            string stringResult = String.Format("Player name: {0}\nCoins: {1}\nTime int labyrinth: {2}\nGame Started: {3}\nDeath Cause: {4}",
                result.PlayerName,
                result.Coins,
                result.SpentTime,
                result.TimeCreated.ToString("G"),
                result.DeathCause);
            return stringResult;
        }
    }
}
