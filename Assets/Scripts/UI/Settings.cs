using Assets.Scripts.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Manage Settings scene
    /// </summary>
    class Settings : MonoBehaviour
    {
        private static string _tag = "Settings";

        private UnityEngine.Logger _logger;

        public Text setPlayerName;
        public Text errorText;
        private readonly int maxSize = 8;
        private readonly int minSize = 2;

        private void Start()
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(_tag, "Settings Start.");
        }

        /// <summary>
        /// Set Player Name with symbols check
        /// </summary>
        public void SetPlayerName()
        {
            string setName = setPlayerName.text;
            string error = string.Empty;

            if (setName == string.Empty)
                error += "Please, enter player name\n";

            if (setName.Length > maxSize)
                error += "The maximum number of characters is 8\n";

            if (setName.Length < minSize)
                error += "The minimum number of characters is 2\n";

            if (error == string.Empty)
            {
                Menu.PlayerName = setName;
                SceneManager.LoadScene("Menu");
            }
            else
            {
                errorText.text = error;
                setPlayerName.text = string.Empty;
            }
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
