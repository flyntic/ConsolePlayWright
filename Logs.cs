using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsolePW
{
    public class Logs
    {
        static StreamWriter sw = new StreamWriter("Logs.txt");
        static public void Write(string text)
        { 
            sw.WriteLine(text);
        }
        static public void Close()
        {
            sw.Close();
        }
    }
}
