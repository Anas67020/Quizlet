using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class QuizApi
    {
        private readonly HttpClient client;

        public QuizApi()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://quizzapp.game-creators.de/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void SetToken(string token)
        {
            client.DefaultRequestHeaders.Remove("X-Auth-Token");
            if (string.IsNullOrWhiteSpace(token) == false)
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);
        }

        // Inhalt ohne "charset=utf-8" schicken, damit der Server nicht meckert
        private StringContent CreateJsonContent(string json)
        {
            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // ohne charset
            return content;
        }

        // PUT /api/v1/user/{email}/signup
        public async Task<HttpResponseMessage> SignupAsync(string email, SignupRequest body)
        {
            string safeEmail = Uri.EscapeDataString(email);

            string json = JsonConvert.SerializeObject(body);
            var content = CreateJsonContent(json); // Content-Type fix

            return await client.PutAsync($"api/v1/user/{safeEmail}/signup", content);
        }

        // GET /api/v1/user/{user}/signin  (Server will GET, aber .NET 4.8 kann kein GET-Body -> Raw Request)
        public async Task<HttpResponseMessage> SigninAsync(string user, string password)
        {
            string safeUser = Uri.EscapeDataString(user);

            var body = new SigninRequest();
            body.Password = password;

            string json = JsonConvert.SerializeObject(body);

            // Raw HTTP Request bauen, damit GET + Body funktioniert
            string path = "/api/v1/user/" + safeUser + "/signin";
            return await SendRawGetWithJsonBodyAsync("quizzapp.game-creators.de", 443, path, json);
        }

        // GET /api/v1/user/{user}
        public async Task<HttpResponseMessage> GetUserAsync(string token, string user)
        {
            SetToken(token);

            string safeUser = Uri.EscapeDataString(user);
            return await client.GetAsync($"api/v1/user/{safeUser}");
        }

        // PATCH /api/v1/user/{user}
        public async Task<HttpResponseMessage> UpdateUserAsync(string token, string user, UpdateUserRequest body)
        {
            SetToken(token);

            string safeUser = Uri.EscapeDataString(user);

            string json = JsonConvert.SerializeObject(body);
            var req = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/v1/user/{safeUser}");

            // Content-Type fix (ohne charset)
            req.Content = CreateJsonContent(json);

            return await client.SendAsync(req);
        }

        // Raw GET mit JSON Body (Workaround für .NET Framework 4.8)
        private async Task<HttpResponseMessage> SendRawGetWithJsonBodyAsync(string host, int port, string path, string jsonBody)
        {
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonBody);

            // HTTP Header bauen
            StringBuilder sb = new StringBuilder();
            sb.Append("GET ").Append(path).Append(" HTTP/1.1\r\n");
            sb.Append("Host: ").Append(host).Append("\r\n");
            sb.Append("Accept: application/json\r\n");
            sb.Append("Content-Type: application/json\r\n"); // ohne charset
            sb.Append("Content-Length: ").Append(bodyBytes.Length).Append("\r\n");
            sb.Append("Connection: close\r\n");
            sb.Append("\r\n");

            byte[] headerBytes = Encoding.ASCII.GetBytes(sb.ToString());

            using (TcpClient tcp = new TcpClient())
            {
                await tcp.ConnectAsync(host, port);

                using (SslStream ssl = new SslStream(tcp.GetStream(), false))
                {
                    // TLS Handshake
                    await ssl.AuthenticateAsClientAsync(host, null, SslProtocols.Tls12 | SslProtocols.Tls13, false);

                    // Request senden
                    await ssl.WriteAsync(headerBytes, 0, headerBytes.Length);
                    await ssl.WriteAsync(bodyBytes, 0, bodyBytes.Length);
                    await ssl.FlushAsync();

                    // Response lesen
                    byte[] raw = await ReadAllBytesAsync(ssl);

                    // Header/Body trennen
                    int split = IndexOf(raw, new byte[] { 13, 10, 13, 10 }); // \r\n\r\n
                    if (split < 0)
                    {
                        var fail = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        fail.Content = new StringContent("Ungültige Serverantwort (kein Header-Ende gefunden).");
                        return fail;
                    }

                    byte[] headerPart = SubArray(raw, 0, split);
                    byte[] bodyPart = SubArray(raw, split + 4, raw.Length - (split + 4));

                    string headerText = Encoding.ASCII.GetString(headerPart);

                    // Statuscode aus erster Zeile
                    // Beispiel: HTTP/1.1 200 OK
                    int statusCode = 0;
                    string reason = "Unknown";

                    string[] lines = headerText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length > 0)
                    {
                        string[] first = lines[0].Split(' ');
                        if (first.Length >= 3)
                        {
                            int.TryParse(first[1], out statusCode);
                            reason = string.Join(" ", first, 2, first.Length - 2);
                        }
                    }

                    // Chunked?
                    bool chunked = headerText.IndexOf("Transfer-Encoding: chunked", StringComparison.OrdinalIgnoreCase) >= 0;
                    if (chunked)
                    {
                        bodyPart = DecodeChunked(bodyPart);
                    }

                    var resp = new HttpResponseMessage((HttpStatusCode)statusCode);
                    resp.ReasonPhrase = reason;

                    // Body als JSON StringContent zurückgeben
                    string bodyString = Encoding.UTF8.GetString(bodyPart);
                    var content = new StringContent(bodyString, Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    resp.Content = content;

                    return resp;
                }
            }
        }

        private async Task<byte[]> ReadAllBytesAsync(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[8192];
                int read;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private int IndexOf(byte[] haystack, byte[] needle)
        {
            for (int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                bool ok = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j])
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok) return i;
            }
            return -1;
        }

        private byte[] SubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Buffer.BlockCopy(data, index, result, 0, length);
            return result;
        }

        private byte[] DecodeChunked(byte[] chunkedBody)
        {
            using (MemoryStream ms = new MemoryStream())
            using (MemoryStream outMs = new MemoryStream())
            {
                ms.Write(chunkedBody, 0, chunkedBody.Length);
                ms.Position = 0;

                while (true)
                {
                    string line = ReadLineAscii(ms);
                    if (line == null) break;

                    int semi = line.IndexOf(';');
                    if (semi >= 0) line = line.Substring(0, semi);

                    int chunkSize = int.Parse(line.Trim(), System.Globalization.NumberStyles.HexNumber);
                    if (chunkSize == 0) break;

                    byte[] chunk = new byte[chunkSize];
                    ms.Read(chunk, 0, chunkSize);
                    outMs.Write(chunk, 0, chunkSize);

                    // \r\n nach chunk
                    ms.ReadByte();
                    ms.ReadByte();
                }

                return outMs.ToArray();
            }
        }

        private string ReadLineAscii(Stream s)
        {
            using (MemoryStream line = new MemoryStream())
            {
                int b;
                while ((b = s.ReadByte()) != -1)
                {
                    if (b == 13)
                    {
                        int next = s.ReadByte(); // \n
                        break;
                    }
                    line.WriteByte((byte)b);
                }

                if (line.Length == 0 && b == -1) return null;
                return Encoding.ASCII.GetString(line.ToArray());
            }
        }
    }
}
