using Assets.Scripts.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Logger;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Manage Statistics scene
    /// </summary>
    class Statistics : MonoBehaviour
    {
        private static string _tag = "Settings";

        private UnityEngine.Logger _logger;

        /// <summary>
        /// Resuslt statistic
        /// </summary>
        private Text _textResult;
        private void Start()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "Statistics Start.");

            try
            {
                _textResult = GetComponent<Text>();

                if (File.Exists(Path.Combine(Application.dataPath, "session_list.xml")))
                    _textResult.text = getTable(SessionList.Load().Sessions);
                else
                    _textResult.text = "No data";
            }
            catch(System.Exception e)
            {
                _logger.LogException(e);
            }
        }

        /// <summary>
        /// Conver data from sessionList to string data
        /// </summary>
        /// <param name="sessionResultList">sessionList</param>
        /// <returns>string data</returns>
        private string getTable(List<SessionResult> sessionResultList)
        {
            var sessionList = sessionResultList.OrderByDescending(d => d.TimeCreated);

            string sessionListString = "Time Created\tName\t\tCoins\tDeath\tSpentTime\t\n";
            foreach (SessionResult sessionResult in sessionList)
            {
                string tab = "\t";

                if (sessionResult.PlayerName.Length >= 5 && sessionResult.PlayerName.Length < 8)
                    tab += "\t";
                else if (sessionResult.PlayerName.Length <= 4)
                    tab += "\t\t";

                sessionListString += String.Format("{0}\t{1}" + tab + "{2}\t\t\t{3}\t\t{4}\n",
                    sessionResult.TimeCreated.ToString("MM/dd hh:mm:ss"),
                    sessionResult.PlayerName,
                    sessionResult.Coins,
                    sessionResult.DeathCause,
                    sessionResult.SpentTime.ToString());
            }
            return sessionListString;
        }

    }
}
