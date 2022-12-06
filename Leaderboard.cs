using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardFunction
{
    public class Leaderboard
    {
        public List<LeaderboardSingle> leaderboardSingleList;
    }

    public class LeaderboardSingle
    {
        public string name;
        public int score;
    }
}
