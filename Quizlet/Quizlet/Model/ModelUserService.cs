using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class ModelUserService
    {
        private readonly QuizApi api;
        private readonly AppSession session;

        private string lastError;
        public string LastError
        {
            get { return lastError; }
            private set { lastError = value; }
        }

        public ModelUserService(AppSession session)
        {
            this.session = session;
            api = new QuizApi();
            LastError = "";
        }

        // Registrieren -> Account erstellen (E-Mail kommt vom Server, wenn Request ok ist)
        public async Task<bool> RegisterAsync(string email, string nickname, string fullname, string password)
        {
            LastError = "";

            try
            {
                var body = new SignupRequest();
                body.Nickname = nickname;
                body.Fullname = fullname;
                body.Password = password;

                HttpResponseMessage resp = await api.SignupAsync(email, body);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }

                LastError = BuildHttpError("Signup", resp, txt);
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Signup Fehler: " + ex.Message;
                return false;
            }
        }

        // Login -> Token holen -> Userdaten laden -> Session füllen
        public async Task<bool> LoginAsync(string userOrEmailOrId, string password)
        {
            LastError = "";

            try
            {
                // Login Request (muss POST sein, sonst knallt es in WPF/.NET Framework)
                HttpResponseMessage resp = await api.SigninAsync(userOrEmailOrId, password);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode == false)
                {
                    LastError = BuildHttpError("Login", resp, txt);
                    return false;
                }

                // Token aus JSON lesen
                var signin = SafeDeserialize<SigninResponse>(txt);
                if (signin == null || string.IsNullOrWhiteSpace(signin.AuthToken))
                {
                    LastError = "Login fehlgeschlagen: Kein Token erhalten.";
                    return false;
                }

                // Token in Session speichern
                session.AuthToken = signin.AuthToken;

                // Userdaten laden (Token muss im Header mit)
                HttpResponseMessage meResp = await api.GetUserAsync(session.AuthToken, userOrEmailOrId);
                string meTxt = await meResp.Content.ReadAsStringAsync();

                if (meResp.IsSuccessStatusCode == false)
                {
                    LastError = BuildHttpError("User laden", meResp, meTxt);
                    session.AuthToken = "";
                    return false;
                }

                // User-Objekt lesen
                var me = SafeDeserialize<ApiUser>(meTxt);
                if (me == null)
                {
                    LastError = "Userdaten konnten nicht gelesen werden.";
                    session.AuthToken = "";
                    return false;
                }

                // Session füllen
                session.CurrentUserId = me.UserId;
                session.CurrentUsername = me.Nickname;
                session.CurrentEmail = me.Email;
                session.CurrentUser = me;

                return true;
            }
            catch (Exception ex)
            {
                LastError = "Login Fehler: " + ex.Message;
                return false;
            }
        }

        // Nickname ändern
        public async Task<bool> ChangeNicknameAsync(string currentPassword, string newNickname)
        {
            LastError = "";

            try
            {
                var req = new UpdateUserRequest();
                req.CurrentPassword = currentPassword;
                req.Nickname = newNickname;

                // Stabil: mit UserId arbeiten
                string userKey = session.CurrentUserId.ToString();

                HttpResponseMessage resp = await api.UpdateUserAsync(session.AuthToken, userKey, req);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                {
                    session.CurrentUsername = newNickname;
                    if (session.CurrentUser != null) session.CurrentUser.Nickname = newNickname;
                    return true;
                }

                LastError = BuildHttpError("Nickname ändern", resp, txt);
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Nickname ändern Fehler: " + ex.Message;
                return false;
            }
        }

        // E-Mail ändern
        public async Task<bool> ChangeEmailAsync(string currentPassword, string newEmail)
        {
            LastError = "";

            try
            {
                var req = new UpdateUserRequest();
                req.CurrentPassword = currentPassword;
                req.Email = newEmail;

                string userKey = session.CurrentUserId.ToString();

                HttpResponseMessage resp = await api.UpdateUserAsync(session.AuthToken, userKey, req);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                {
                    session.CurrentEmail = newEmail;
                    if (session.CurrentUser != null) session.CurrentUser.Email = newEmail;
                    return true;
                }

                LastError = BuildHttpError("E-Mail ändern", resp, txt);
                return false;
            }
            catch (Exception ex)
            {
                LastError = "E-Mail ändern Fehler: " + ex.Message;
                return false;
            }
        }

        // Passwort ändern
        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            LastError = "";

            try
            {
                var req = new UpdateUserRequest();
                req.CurrentPassword = currentPassword;
                req.NewPassword = newPassword;

                string userKey = session.CurrentUserId.ToString();

                HttpResponseMessage resp = await api.UpdateUserAsync(session.AuthToken, userKey, req);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }

                LastError = BuildHttpError("Passwort ändern", resp, txt);
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Passwort ändern Fehler: " + ex.Message;
                return false;
            }
        }

        // JSON sicher lesen (kein Crash bei komischem JSON)
        private static T SafeDeserialize<T>(string json) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return null;
            }
        }

        // Fehlermeldung zusammenbauen
        private static string BuildHttpError(string action, HttpResponseMessage resp, string bodyText)
        {
            string msg = action + " fehlgeschlagen: " + ((int)resp.StatusCode).ToString() + " " + resp.ReasonPhrase;

            if (string.IsNullOrWhiteSpace(bodyText) == false)
            {
                msg += " | " + bodyText;
            }

            return msg;
        }
    }
}
