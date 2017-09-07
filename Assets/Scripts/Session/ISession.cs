using Assets.Scripts.GamePlayer;

namespace Assets.Scripts.Session
{
    /// <summary>
    /// Session interface
    /// </summary>
    interface ISession
    {
        IGamePlayer Player { get; set; }

        bool SessionStart { get; set; }

        void RemoveCoin(int id);

        void ExitWithSaveCoins(string nameDeath);

        void ExitWithoutSaveCoins(string nameDeath);

        void SaveResults(string nameDeath);

        int GetCountEnemies();
    }
}
