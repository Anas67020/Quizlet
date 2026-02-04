using Newtonsoft.Json;

namespace Quizlet.Model
{
    public class UpdateUserRequest
    {
        [JsonProperty("current_password")]
        public string CurrentPassword { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("fullname")]
        public string Fullname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}
