using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class SignupRequest
    {
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("fullname")]
        public string Fullname { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
