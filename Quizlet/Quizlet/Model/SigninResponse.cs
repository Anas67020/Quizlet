using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class SigninResponse
    {
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("X-Auth-Token")]
        public string AuthToken { get; set; }
    }
}
