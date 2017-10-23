using GBFrame.ThreadSupport;
using System;

namespace GBFrame.Test
{
    class ContinutyThreadTest
    {
        static void Main2(String[] arg)
        {
            var thread = new ContinutyThread();
            thread.Start();

            thread.TerminalHook += () => {
                Console.WriteLine("Finish 1");
            };

            thread.TerminalHook += () => {
                Console.WriteLine("Finish 2");
            };

            ConsoleKeyInfo key;
            while ((key = Console.ReadKey()) != null) {

                if (key.KeyChar == 'x') { 
                    thread.Finish = true;
                    break;
                }

                thread.Post(() =>
                {
                    Console.WriteLine("Test  {0}", key.KeyChar);
                });
            }
        }
    }
}
