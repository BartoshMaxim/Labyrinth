using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.UI;
using Assets.Scripts.Logger;

namespace Assets.Scripts.Session
{
    /// <summary>
    /// Session Result
    /// </summary>
    public class SessionResult
    {
        [XmlIgnore]
        private static string _tag = "SessionResult";

        [XmlIgnore]
        private UnityEngine.Logger _logger;

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement(DataType = "dateTime")]
        public DateTime TimeCreated { get; set; }

        public string SpentTime { get; set; }

        public string PlayerName { get; set; }

        public string Coins { get; set; }

        public string DeathCause { get; set; }

        public SessionResult(DateTime timeCreated, TimeSpan spentTime, string playerName, int coins, string deathCause)
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "SessionResult Create.");
            TimeCreated = timeCreated;
            if (Menu.SessionList == null)
                Id = 0;
            else
                Id = Menu.SessionList.Sessions.Count;
            SpentTime = spentTime.Minutes + ":" + spentTime.Seconds;
            PlayerName = playerName;
            Coins = coins.ToString();
            DeathCause = deathCause;
        }

        public SessionResult()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "SessionResult Create.");
        }
    }
}
