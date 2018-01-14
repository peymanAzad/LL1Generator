using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL_1__Parser
{
    class NonTerminalState : IState
    {
        public string Name { get; set; }
        public bool IsStart { get; set; }
        public List<TerminalState> FirstList { get; set; }
        public List<TerminalState> FollowList { get; set; }
    }
}
