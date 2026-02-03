using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class ModelUserService
    {
        private readonly QuizApi api;
        private readonly AppSession session;

        private string lastError;
        public string LastError { get { return lastError; } private set { lastError = value; } }

        public ModelUserService(AppSession session)
        {
            this.session = session;
            api = new QuizApi();
            LastError = "";
        }

        public async Task<bool> RegisterAsync(string email, string nickname, string fullname, string password)
        {
            LastError = "";

            try
            {
                var body = new SignupRequest();
                body.Nickname = nickname;
                body.Fullname = fullname;
                body.Password = password;

                var response = await api.SignupAsync(email, body);
                string txt = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return true;

                LastError = "Signup fehlgeschlagen: " + txt;
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Fehler: " + ex.Message;
                return false;
            }
        }

        public async Task<bool> LoginAsync(string user, string password)
        {
            LastError = "";

            try
            {
                var response = await api.SigninAsync(user, password);
                string txt = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode == false)
                {
                    LastError = "Login fehlgeschlagen: " + txt;
                    return false;
                }

                var signin = JsonConvert.DeserializeObject<SigninResponse>(txt);
                if (signin == null || string.IsNullOrWhiteSpace(signin.AuthToken))
                {
                    LastError = "Login fehlgeschlagen: Kein Token erhalten.";
                    return false;
                }

                // Token speichern
                session.AuthToken = signin.AuthToken;

                // Userdaten laden (ID, nickname, email ...)
                var meResp = await api.GetUserAsync(session.AuthToken, user);
                string meTxt = await meResp.Content.ReadAsStringAsync();

                if (meResp.IsSuccessStatusCode == false)
                {
                    LastError = "Userdaten konnten nicht geladen werden: " + meTxt;
                    return false;
                }

                var me = JsonConvert.DeserializeObject<ApiUser>(meTxt);
                if (me == null)
                {
                    LastError = "Userdaten konnten nicht gelesen werden.";
                    return false;
                }

                session.CurrentUserId = me.UserId;
                session.CurrentUsername = me.Nickname;
                session.CurrentEmail = me.Email;
                session.CurrentUser = me;

                return true;
            }
            catch (Exception ex)
            {
                LastError = "Fehler: " + ex.Message;
                return false;
            }
        }

        public async Task<bool> ChangeNicknameAsync(string currentPassword, string newNickname)
        {
            LastError = "";

            try
            {
                var req = new UpdateUserRequest();
                req.CurrentPassword = currentPassword;
                req.Nickname = newNickname;

                string userKey = session.CurrentUserId.ToString();

                var resp = await api.UpdateUserAsync(session.AuthToken, userKey, req);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                {
                    session.CurrentUsername = newNickname;
                    if (session.CurrentUser != null) session.CurrentUser.Nickname = newNickname;
                    return true;
                }

                LastError = "Ändern fehlgeschlagen: " + txt;
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Fehler: " + ex.Message;
                return false;
            }
        }

        public async Task<bool> ChangeEmailAsync(string currentPassword, string newEmail)
        {
            LastError = "";

            try
            {
                var req = new UpdateUserRequest();
                req.CurrentPassword = currentPassword;
                req.Email = newEmail;

                string userKey = session.CurrentUserId.ToString();

                var resp = await api.UpdateUserAsync(session.AuthToken, userKey, req);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                {
                    session.CurrentEmail = newEmail;
                    if (session.CurrentUser != null) session.CurrentUser.Email = newEmail;
                    return true;
                }

                LastError = "Ändern fehlgeschlagen: " + txt;
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Fehler: " + ex.Message;
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            LastError = "";

            try
            {
                var req = new UpdateUserRequest();
                req.CurrentPassword = currentPassword;
                req.NewPassword = newPassword;

                string userKey = session.CurrentUserId.ToString();

                var resp = await api.UpdateUserAsync(session.AuthToken, userKey, req);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                    return true;

                LastError = "Ändern fehlgeschlagen: " + txt;
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Fehler: " + ex.Message;
                return false;
            }
        }
    }
}
