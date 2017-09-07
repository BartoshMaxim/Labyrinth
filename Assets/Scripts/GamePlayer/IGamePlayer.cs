using System;
using UnityEngine;

namespace Assets.Scripts.GamePlayer
{
    /// <summary>
    /// GamePlayer interface
    /// </summary>
    public interface IGamePlayer
    {
        string Name { get; set; }

        GameObject Player { get; }

        DateTime TimeCreate { get; set; }

        float Speed { get; set; }

        int Coins { get; }

        void AddCoin(int income);

        void ResetCoins();
    }
}
