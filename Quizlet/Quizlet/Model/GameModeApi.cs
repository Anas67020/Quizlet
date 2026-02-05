using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class GameModeApi
    {
        [JsonProperty("game_mode_id")]
        public int Id { get; set; }

        [JsonProperty("game_mode")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
