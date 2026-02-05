using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class GameSettings
    {
        public int Rounds { get; set; }
        public int QuestionsPerRound { get; set; }

        //public CategoryDto SelectedCategory { get; set; }
        //public DifficultyDto SelectedDifficulty { get; set; }
    }
}
