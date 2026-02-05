using System;
using System.Collections.ObjectModel;

namespace Quizlet.Model
{
    public class ModelGameHub
    {
        public ObservableCollection<GameSession> Sessions { get; private set; }

        public ModelGameHub()
        {
            Sessions = new ObservableCollection<GameSession>();

            // DUMMIES optional:
            // GameSession s1 = new GameSession(1000, "AJ", "Allgemeinwissen", GameState.Running, 1, "Allgemeinwissen", 1);
            // s1.OpponentName = "Max";
            // Sessions.Add(s1);
        }

        // Für spätere Duelle (WaitingForPlayer)
        public GameSession StartNewGame(int hostUserId, string hostName, string title)
        {
            GameSession s = new GameSession(hostUserId, hostName, title, GameState.WaitingForPlayer);
            Sessions.Add(s);
            return s;
        }

        // NEU: Einzelspiel (sofort Running)
        public GameSession StartSingleplayerGame(int hostUserId, string hostName, string title, int categoryId, string categoryName)
        {
            // gameModeId = 1 (laut deiner API: "Einzelspiel")
            GameSession s = new GameSession(hostUserId, hostName, title, GameState.Running, categoryId, categoryName, 1);
            Sessions.Add(s);
            return s;
        }

        public void JoinGame(GameSession session, string opponentName)
        {
            if (session.State != GameState.WaitingForPlayer)
                throw new InvalidOperationException("Dieses Spiel ist nicht mehr offen.");

            if (string.IsNullOrWhiteSpace(session.OpponentName) == false)
                throw new InvalidOperationException("Dieses Spiel hat schon einen Gegner.");

            session.OpponentName = opponentName;
            session.State = GameState.Running;
        }
    }
}
