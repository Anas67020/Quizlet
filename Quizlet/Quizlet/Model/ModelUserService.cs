using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class ModelUserService
    {
        private readonly QuizApi api;
        private readonly AppSession session;

        public string LastError { get; private set; }

        public ModelUserService(AppSession session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
            api = new QuizApi();
            LastError = "";
        }

        // Leere Strings auf null setzen, damit sie NICHT gesendet werden
        private static void NormalizeUpdate(UpdateUserRequest req)
        {
            if (req == null) return;

            if (string.IsNullOrWhiteSpace(req.Nickname)) req.Nickname = null;
            if (string.IsNullOrWhiteSpace(req.Fullname)) req.Fullname = null;
            if (string.IsNullOrWhiteSpace(req.Email)) req.Email = null;
            if (string.IsNullOrWhiteSpace(req.NewPassword)) req.NewPassword = null;
        }

        public async Task<bool> RegisterAsync(string email, string nickname, string fullname, string password)
        {
            LastError = "";

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(nickname) ||
                string.IsNullOrWhiteSpace(fullname) ||
                string.IsNullOrWhiteSpace(password))
            {
                LastError = "Bitte alle Felder ausfüllen.";
                return false;
            }

            try
            {
                SignupRequest body = new SignupRequest
                {
                    Nickname = nickname,
                    Fullname = fullname,
                    Password = password
                };

                var resp = await api.SignupAsync(email, body);
                string txt = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                    return true;

                LastError = "Signup fehlgeschlagen: " + (string.IsNullOrWhiteSpace(txt)
                    ? ((int)resp.StatusCode + " " + resp.ReasonPhrase)
                    : txt);

                return false;
            }
            catch (Exception ex)
            {
                LastError = "Fehler: " + ex.Message;
                return false;
            }
        }

        public async Task<bool> LoginAsync(string userOrEmailOrId, string password)
        {
            LastError = "";

            if (string.IsNullOrWhiteSpace(userOrEmailOrId) || string.IsNullOrWhiteSpace(password))
            {
                LastError = "Bitte Username/E-Mail und Passwort eingeben.";
                return false;
            }

            try
            {
                var resp = await api.SigninAsync(userOrEmailOrId, password);
                string txt = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    LastError = "Login fehlgeschlagen: " + (string.IsNullOrWhiteSpace(txt)
                        ? ((int)resp.StatusCode + " " + resp.ReasonPhrase)
                        : txt);
                    return false;
                }

                var signin = JsonConvert.DeserializeObject<SigninResponse>(txt);
                if (signin == null || string.IsNullOrWhiteSpace(signin.AuthToken))
                {
                    LastError = "Login fehlgeschlagen: Kein Token erhalten.";
                    return false;
                }

                session.AuthToken = signin.AuthToken;

                // Userdaten laden (mit derselben Kennung wie beim Login)
                var meResp = await api.GetUserAsync(session.AuthToken, userOrEmailOrId);
                string meTxt = await meResp.Content.ReadAsStringAsync();

                if (!meResp.IsSuccessStatusCode)
                {
                    LastError = "Userdaten konnten nicht geladen werden: " + (string.IsNullOrWhiteSpace(meTxt)
                        ? ((int)meResp.StatusCode + " " + meResp.ReasonPhrase)
                        : meTxt);
                    return false;
                }

                var me = JsonConvert.DeserializeObject<ApiUser>(meTxt);
                if (me == null)
                {
                    LastError = "Userdaten konnten nicht gelesen werden.";
                    return false;
                }

                session.CurrentUserId = me.UserId;
                session.CurrentUsername = me.Nickname ?? "";
                session.CurrentEmail = me.Email ?? "";
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

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                LastError = "Bitte aktuelles Passwort eingeben.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(newNickname))
            {
                LastError = "Bitte neuen Username eingeben.";
                return false;
            }

            try
            {
                UpdateUserRequest req = new UpdateUserRequest
                {
                    CurrentPassword = currentPassword,
                    Nickname = newNickname
                };

                NormalizeUpdate(req);

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

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                LastError = "Bitte aktuelles Passwort eingeben.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                LastError = "Bitte neue E-Mail eingeben.";
                return false;
            }

            try
            {
                UpdateUserRequest req = new UpdateUserRequest
                {
                    CurrentPassword = currentPassword,
                    Email = newEmail
                };

                NormalizeUpdate(req);

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

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                LastError = "Bitte aktuelles Passwort eingeben.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                LastError = "Bitte neues Passwort eingeben.";
                return false;
            }

            try
            {
                UpdateUserRequest req = new UpdateUserRequest
                {
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                };

                NormalizeUpdate(req);

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
