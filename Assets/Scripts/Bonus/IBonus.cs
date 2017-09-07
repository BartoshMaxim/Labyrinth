using UnityEngine;

namespace Assets.Scripts.Bonus
{
    /// <summary>
    /// Bonus interface
    /// </summary>
    interface IBonus
    {
        int Id { get; set;}

        GameObject _coin { get; set; }
    }
}
