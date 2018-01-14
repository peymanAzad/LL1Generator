using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL_1__Parser
{
    class TerminalState : IState
    {
        private static TerminalState _lambda;

        public static TerminalState Lambda
        {
            get
            {
                if (_lambda == null)
                {
                    _lambda = new TerminalState()
                    {
                        Name = "#"
                    };
                }
                return _lambda;
            }
        }

        private static TerminalState _eof;

        public static TerminalState EoF
        {
            get
            {
                if (_eof == null)
                {
                    _eof = new TerminalState()
                    {
                        Name = "$"
                    };
                }
                return _eof;
            }
        }

        public string Name { get; set; }
    }
}
