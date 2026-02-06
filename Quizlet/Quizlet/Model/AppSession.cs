using Prism.Mvvm;

namespace Quizlet.Model
{
    public class AppSession : BindableBase
    {
        private int currentUserId;
        public int CurrentUserId
        {
            get { return currentUserId; }
            set
            {
                if (SetProperty(ref currentUserId, value))
                {
                    RaisePropertyChanged(nameof(IsLoggedIn));
                }
            }
        }

        private string currentUsername;
        public string CurrentUsername
        {
            get { return currentUsername; }
            set { SetProperty(ref currentUsername, value); }
        }

        private string currentEmail;
        public string CurrentEmail
        {
            get { return currentEmail; }
            set { SetProperty(ref currentEmail, value); }
        }

        private string authToken;
        public string AuthToken
        {
            get { return authToken; }
            set { SetProperty(ref authToken, value); }
        }

        private string apiKey;
        public string ApiKey
        {
            get { return apiKey; }
            set { SetProperty(ref apiKey, value); }
        }

        private ApiUser currentUser;
        public ApiUser CurrentUser
        {
            get { return currentUser; }
            set { SetProperty(ref currentUser, value); }
        }

        public bool IsLoggedIn
        {
            get { return CurrentUserId >= 0; }
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
            ApiKey = "";
            CurrentUser = null;
        }
    }
}
