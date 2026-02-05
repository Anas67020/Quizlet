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

            GameSession s1 = new GameSession(1000, "AJ", "Allgemeinwissen", GameState.Running);
            s1.OpponentName = "Max";
            Sessions.Add(s1);

            Sessions.Add(new GameSession(2002, "Lea", "Filme & Serien", GameState.WaitingForPlayer));
            Sessions.Add(new GameSession(3003, "Tom", "Gaming", GameState.WaitingForPlayer));
        }

        public GameSession StartNewGame(int hostUserId, string hostName, string title)
        {
            GameSession s = new GameSession(hostUserId, hostName, title, GameState.WaitingForPlayer);
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
