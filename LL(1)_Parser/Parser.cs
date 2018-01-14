using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL_1__Parser
{
    class Parser
    {
        public Parser(List<string> alphabet, List<string> states, List<Sentence> sentences)
        {
            this.Grammer = new Grammer(sentences);

            Ll1Table = new Dictionary<string, Dictionary<string, List<IState>>>();
            foreach (var state in states)
            {
                var row = new Dictionary<string, List<IState>>();
                foreach (var alpha in alphabet)
                {
                    row.Add(alpha, new List<IState>());
                }
                Ll1Table.Add(state, row);
            }
        }
        public Grammer Grammer { get; set; }
        public Dictionary<string, Dictionary<string, List<IState>>> Ll1Table { get; set; }

        public bool GenerateLL1Table()
        {
            foreach (var sentence in Grammer.Sentences)
            {
                var row = Ll1Table[sentence.Left.Name];

                for (int i = 0; i < sentence.Right.Count; i++)
                {
                    var state = sentence.Right[i];

                    if (state is TerminalState)
                    {
                        if (state == TerminalState.Lambda)
                        {
                            foreach (var follow in sentence.Left.FollowList)
                            {
                                if (row[follow.Name].Count > 0)
                                {
                                    throw new Exception("the grammer is not LL(1)!");
                                }
                                row[follow.Name].AddRange(sentence.Right);
                            }
                        }
                        else
                        {
                            if (row[state.Name].Count > 0)
                            {
                                throw new Exception("the grammer is not LL(1)!");
                            }
                            row[state.Name].AddRange(sentence.Right);
                        }

                        break;
                    }
                    else if (state is NonTerminalState)
                    {
                        var firsts = (state as NonTerminalState).FirstList.Except(new List<TerminalState>() { TerminalState.Lambda });
                        foreach (var first in firsts)
                        {
                            if (row[first.Name].Count > 0)
                            {
                                throw new Exception("the grammer is not LL(1)!");
                            }
                            row[first.Name].AddRange(sentence.Right);
                        }

                        if (!(state as NonTerminalState).FirstList.Contains(TerminalState.Lambda))
                        {
                            break;
                        }
                    }

                }
                //var state = sentence.Right.First();


            }

            return true;
        }
    }
}
