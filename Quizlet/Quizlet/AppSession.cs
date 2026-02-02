using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet
{
    public static class AppSession
    {
        // Speichern der Login-Session
        private static int currentUserId = -1;
        public static int CurrentUserId
        {
            get { return currentUserId; }
            set { currentUserId = value; }
        }

        private static string currentUsername = "";
        public static string CurrentUsername
        {
            get { return currentUsername; }
            set { currentUsername = value; }
        }
    }
}


