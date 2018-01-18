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
                                    throw new NotLL1Exception();
                                }
                                row[follow.Name].AddRange(sentence.Right);
                            }
                        }
                        else
                        {
                            if (row[state.Name].Count > 0)
                            {
                                throw new NotLL1Exception();
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
                                throw new NotLL1Exception();
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

        public string PirntLL1Table()
        {
            var result = "<table style=\"border: 1px solid black;border-collapse: collapse;\"><thead><tr><th></th>";

            foreach (var col in Ll1Table.First().Value.Keys)
            {
                result += "<th style=\"border: 1px solid black;\">" + col + "</th>";
            }
            result += "</tr></thead><tbody>";

            foreach (var row in Ll1Table)
            {
                result += "<tr><th style=\"border: 1px solid black;\">" + row.Key + "</th>";
                foreach (var col in row.Value)
                {
                    result += "<td style=\"border: 1px solid black;\">" + string.Concat(col.Value.Select(s => s.Name)) + "</td>";
                }
                result += "</tr>";
            }

            result += "</tbody></table>";

            return result;
        }

        public bool CheckLL1()
        {
            var states = Grammer.Sentences.Select(s => s.Left).Distinct();
            if(states.GroupBy(g => g.Name).Any(t => t.Count() > 1))
            {
                return false;
            }
            return true;
        }
    }

    class NotLL1Exception : Exception
    {
    }
}
