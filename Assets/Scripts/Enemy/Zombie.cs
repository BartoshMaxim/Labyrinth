using Assets.Scripts.Logger;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Zombie class
    /// </summary>
    public class Zombie : IEnemy
    {
        public static readonly string ZombyObjectName = "Zombie(Clone)";

        public static readonly string ZombyClassName = "Zombie";

        private UnityEngine.Logger _logger;

        private GameObject _zombie;
        
        public int Id { get; set; }

        public Zombie(int id, GameObject zombie, Func<GameObject, GameObject> create)
        {
            _logger = new UnityEngine.Logger(new LabyrinthLogHandler());
            _logger.Log(ZombyClassName, "Zombie Creates.");

            _zombie = zombie;
            Id = id;
            create(_zombie);
        }
    }
}
