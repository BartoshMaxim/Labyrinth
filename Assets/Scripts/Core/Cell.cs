using UnityEngine;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// Maze cell
    /// </summary>
    public class Cell
    {
        public bool visited;
        public GameObject north;//1
        public GameObject east;//2
        public GameObject west;//3
        public GameObject south;//4
    }
}
