using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    // Status eines Spiels
    public enum GameState
    {
        WaitingForPlayer,
        Running,
        Finished
    }
}
