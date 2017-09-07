using System;
using UnityEngine;
using Assets.Scripts.Logger;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Mummy class
    /// </summary>
    public class Mummy : IEnemy
    {
        public static readonly string MummyObjectName = "Mummy(Clone)";

        public static readonly string MummyClassName = "Mummy";

        private UnityEngine.Logger _logger;

        private GameObject _mummy;

        public int Id { get; set; }

        public Mummy(int id, GameObject mummy, Func<GameObject, GameObject> create)
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(MummyClassName, "Mummy Creates.");
            _mummy = mummy;
            Id = id;
            create(_mummy);
        }
    }
}
