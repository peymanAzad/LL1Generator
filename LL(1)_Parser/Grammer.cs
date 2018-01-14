using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL_1__Parser
{
    class Grammer
    {
        public Grammer(List<Sentence> sentences)
        {
            Sentences = sentences;

            var states = sentences.Select(t => t.Left).Distinct();
            foreach (var state in states)
            {
                state.FirstList = CalculateFirst(state);
            }
            foreach (var state in states)
            {
                state.FollowList = CalculateFollow(state).Distinct().ToList();
            }
        }

        public List<Sentence> Sentences { get; set; }

        public List<TerminalState> CalculateFirst(NonTerminalState state)
        {
            var result = new List<TerminalState>();

            var states = Sentences.Where(t => t.Left.Name == state.Name);
            foreach (var sentence in states)
            {
                for (int i = 0; i < sentence.Right.Count; ++i)
                {
                    var s = sentence.Right[i];
                    if (s is TerminalState)
                    {
                        result.Add(s as TerminalState);
                        break;
                    }
                    else if (s is NonTerminalState)
                    {
                        result.AddRange(CalculateFirst(s as NonTerminalState).Except(new List<TerminalState>() {TerminalState.Lambda}));

                        if (!Sentences.Any(a => a.Left.Name == s.Name && a.Right.Contains(TerminalState.Lambda)))
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public List<TerminalState> CalculateFollow(NonTerminalState state)
        {
            var result = new List<TerminalState>();

            if (state.IsStart)
            {
                result.Add(TerminalState.EoF);
            }

            foreach (var sentence in Sentences)
            {
                for (int i = 0; i < sentence.Right.Count; ++i)
                {
                    var s = sentence.Right[i];
                    if (s.Name == state.Name)
                    {
                        var j = i + 1;

                        if (i == sentence.Right.Count -1)
                        {
                            if (sentence.Left.Name != s.Name)
                            {
                                result.AddRange(CalculateFollow(sentence.Left));
                            }
                            
                            break;
                        }

                        while (j <= sentence.Right.Count)
                        {
                            if (j == sentence.Right.Count)
                            {
                                if (sentence.Left.Name != s.Name)
                                {
                                    result.AddRange(CalculateFollow(sentence.Left));
                                }

                                break;
                            }

                            var next = sentence.Right[j++];
                            if (next is TerminalState)
                            {
                                result.Add(next as TerminalState);
                                break;
                            }
                            else if (next is NonTerminalState)
                            {
                                result.AddRange((next as NonTerminalState).FirstList.Except(new List<TerminalState>() { TerminalState.Lambda }));

                                if (!Sentences.Any(a => a.Left.Name == next.Name && a.Right.Contains(TerminalState.Lambda)))
                                {
                                    break;
                                }
                            }
                        } 

                        break;
                    }
                }
            }

            return result;
        }   
    }
}
