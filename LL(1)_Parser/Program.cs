using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL_1__Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            string path;
            if (args.Length != 0)
            {
                path = args[0];
            }
            else
            {
                Console.WriteLine("Enter Input File Path:");
                path = Console.ReadLine();
            }

            string input;
            Console.WriteLine("Enter String To Parse");
            input = Console.ReadLine();

            var Alphabet = new List<TerminalState>();
            var States = new List<NonTerminalState>();
            var Vectors = new List<Sentence>();

            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var command = parts[0];
                switch (command)
                {
                    case "state":
                        var state = new NonTerminalState() { Name = parts[1], IsStart = line.Contains("-start")};
                        States.Add(state);
                        break;
                    case "vector":
                        var name1 = parts[1];
                        var name2 = parts[2];
                        var state1 = States.SingleOrDefault(u => u.Name == name1);
                        var state2 = new List<IState>();
                        foreach (var a in name2)
                        {
                            IState s = Alphabet.FirstOrDefault(t => t.Name == a.ToString() );
                            if (s != null)
                            {
                                state2.Add(s);
                            }
                            else
                            {
                                s = States.FirstOrDefault(t => t.Name == a.ToString());
                                if (s != null)
                                {
                                    state2.Add(s);
                                }
                            }
                        }
                        if (state1 != null && state2.Count > 0)
                        {
                            Vectors.Add(new Sentence() {Left = state1, Right = state2});
                        }
                        break;
                    case "alphabet":
                        var alphabet = parts[1].Split(',');
                        Alphabet.AddRange(alphabet.Select(s => new TerminalState() {Name = s}));
                        Alphabet.Add(TerminalState.Lambda);
                        Alphabet.Add(TerminalState.EoF);
                        break;
                }
            }
            
            var parser = new Parser(Alphabet.Except(new List<TerminalState>() { TerminalState.Lambda }).Select(t => t.Name).ToList(), States.Select(t => t.Name).ToList(), Vectors);

            parser.GenerateLL1Table();

            Console.WriteLine(parser.PirntLL1Table());

            Console.ReadKey();
        }
    }
}
