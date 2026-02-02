using System;
using System.Collections.ObjectModel;

namespace Quizlet.Model
{
    internal class ModelGameHub
    {
        // Sammlung aller Spiele (Demo: InMemory)
        private ObservableCollection<GameSession> sessions;
        public ObservableCollection<GameSession> Sessions
        {
            get { return sessions; }
            set { sessions = value; }
        }

        public ModelGameHub()
        {
            // Startwerte anlegen
            Sessions = new ObservableCollection<GameSession>();

            GameSession s1 = new GameSession(1000, "AJ", "Allgemeinwissen", GameState.Running);
            s1.OpponentName = "Max";
            Sessions.Add(s1);

            Sessions.Add(new GameSession(2002, "Lea", "Filme & Serien", GameState.WaitingForPlayer));
            Sessions.Add(new GameSession(3003, "Tom", "Gaming", GameState.WaitingForPlayer));
        }

        public GameSession StartNewGame(int hostUserId, string hostName, string title)
        {
            // Neues Spiel anlegen
            GameSession s = new GameSession(hostUserId, hostName, title, GameState.WaitingForPlayer);
            Sessions.Add(s);
            return s;
        }

        public void JoinGame(GameSession session, string opponentName)
        {
            // Prüfen ob noch offen
            if (session.State != GameState.WaitingForPlayer)
                throw new InvalidOperationException("Dieses Spiel ist nicht mehr offen.");

            // Prüfen ob schon Gegner drin ist
            if (!string.IsNullOrWhiteSpace(session.OpponentName))
                throw new InvalidOperationException("Dieses Spiel hat schon einen Gegner.");

            // Gegner setzen und Spiel starten
            session.OpponentName = opponentName;
            session.State = GameState.Running;
        }
    }
}
