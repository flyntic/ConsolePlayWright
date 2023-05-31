using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePW
{
    public class ActionThread
    {
        public static bool ReturnFalse(int N) { return false; }
        public delegate  void Action();
        public delegate bool Check(int N);
        public static async void  Run(Action action, int Number=1,
                                      Check? checkBreak= null,
                                      Check? checkEnd = null
                                         )
        {
            List<Thread> threads = new List<Thread>();
            
            do
            {
                threads.Add(new Thread(async ()=> { action(); }));
                threads.Last().IsBackground = true;
                threads.Last().Start();
                Thread.Sleep(1000);
                if (checkBreak!=null) 
                    if (checkBreak(0)) break;

            } while (threads.Count <= Number-1);

            if(checkEnd!=null)
              while (checkEnd(Number-1)) ;

        }

    }
}
