using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class AppSession
    {
        private int currentUserId;
        public int CurrentUserId
        {
            get { return currentUserId; }
            set { currentUserId = value; }
        }

        private string currentUsername;
        public string CurrentUsername
        {
            get { return currentUsername; }
            set { currentUsername = value; }
        }

        private string currentEmail;
        public string CurrentEmail
        {
            get { return currentEmail; }
            set { currentEmail = value; }
        }

        private string authToken;
        public string AuthToken
        {
            get { return authToken; }
            set { authToken = value; }
        }

        // NEU: API-Key (kommt laut Lehrer als Response-Header beim Login)
        private string apiKey;
        public string ApiKey
        {
            get { return apiKey; }
            set { apiKey = value; }
        }

        private ApiUser currentUser;
        public ApiUser CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        public AppSession()
        {
            Clear();
        }

        public void Clear()
        {
            CurrentUserId = -1;
            CurrentUsername = "";
            CurrentEmail = "";
            AuthToken = "";
            ApiKey = "";     // NEU
            CurrentUser = null;
        }
    }
}
