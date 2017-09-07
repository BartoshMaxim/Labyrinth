using Assets.Scripts.Logger;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts.Session
{
    /// <summary>
    /// Session List
    /// </summary>
    [XmlRoot("SessionList")]
    public class SessionList
    {
        [XmlIgnore]
        private static string _tag = "SessionList";

        [XmlIgnore]
        private UnityEngine.Logger _logger;

        /// <summary>
        /// SessionRessult List
        /// </summary>
        [XmlArray("Sessions")]
        [XmlArrayItem("Session")]
        public List<SessionResult> Sessions = new List<SessionResult>();

        public SessionList()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "SessionList Create.");
        }

        public static SessionList Load()
        {
            var serializer = new XmlSerializer(typeof(SessionList));
            using (var stream = new FileStream(Path.Combine(Application.dataPath, "session_list.xml"), FileMode.Open))
            {
                return serializer.Deserialize(stream) as SessionList;
            }
        }

        public void Save()
        {
            try
            {
                _logger.Log(_tag, "SessionList Save.");
                var serializer = new XmlSerializer(typeof(SessionList));
                using (var stream = new FileStream(Path.Combine(Application.dataPath, "session_list.xml"), FileMode.Create))
                {
                    serializer.Serialize(stream, this);
                }
            }
            catch(System.Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
}
