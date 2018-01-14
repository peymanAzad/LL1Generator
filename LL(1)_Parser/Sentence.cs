using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL_1__Parser
{
    class Sentence
    {
        public NonTerminalState Left { get; set; }
        public List<IState> Right { get; set; }
    }
}
