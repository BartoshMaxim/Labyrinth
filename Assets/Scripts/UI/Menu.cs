using Assets.Scripts.Logger;
using Assets.Scripts.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Start menu
    /// </summary>
    public class Menu : MonoBehaviour
    {
        private static string _tag = "Menu";

        private UnityEngine.Logger _logger;

        public static SessionList SessionList;

        public static string PlayerName = "Player";

        private void Start()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "SessionResultAnimation Start.");
            try
            {
                if (File.Exists(Path.Combine(Application.dataPath, "session_list.xml")))
                    SessionList = SessionList.Load();//read data from xml
                else
                    SessionList = new SessionList();
            }
            catch(System.Exception e)
            {
                _logger.LogException(e);
            }
        }

        /// <summary>
        /// Load Labyrinth
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadScene("Labyrinth");
        }

        /// <summary>
        /// Load Settings
        /// </summary>
        public void Settings()
        {
            SceneManager.LoadScene("Settings");
        }

        /// <summary>
        /// Load Statistics
        /// </summary>
        public void Statistics()
        {
            SceneManager.LoadScene("Statistics");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Load Menu
        /// </summary>
        public void BackToMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
