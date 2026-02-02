using System;

namespace Quizlet
{
    // Anlegen des Delegates für Events
    public delegate void QuizletEventHandler<TEventArgs>(object source, TEventArgs e);

    // EventArgs Klasse für Session-Übergabe
    public class GameSessionEventArgs : EventArgs
    {
        // Speichern der übergebenen Session
        private Quizlet.Model.GameSession session;
        public Quizlet.Model.GameSession Session
        {
            get { return session; }
            set { session = value; }
        }

        public GameSessionEventArgs(Quizlet.Model.GameSession s)
        {
            // Übergeben der Values
            Session = s;
        }
    }
}
