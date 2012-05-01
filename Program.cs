using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
namespace ConsoleApplication1
{

    class AClass
    {
        public void print(int a)
        {
            Console.WriteLine(a * a);
            System.Threading.Thread.Sleep(100);

        }
        public void printb(int a)
        {
            Console.WriteLine(a - 1);

        }
        public static void staticPrint(int a)
        {
            Console.WriteLine(a);

        }
    }

    class Program
    {
        public delegate void del(int a); 
        public static event del myEvent;


        public static void Callback(IAsyncResult itfAR)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.GetHashCode());

            AsyncResult res = (AsyncResult) itfAR;
            del d = (del)res.AsyncDelegate;
            d.EndInvoke(itfAR);
        }
        static void Main(string[] args)
        {
            AClass a = new AClass();
            AClass b = new AClass();

            del d;
            d = new del(AClass.staticPrint);

            // event 사용
            myEvent += new del(a.print);
            myEvent.BeginInvoke(3, new AsyncCallback(Callback), null);

            
            d += new del(a.print);
            // 비동기는 1개의 delegate만 가능함.
            //d += new del(b.printb);
            //d -= new del(b.printb);

            foreach (del dd in d.GetInvocationList())
            {
                Console.WriteLine(dd.Method);
            }


            IAsyncResult itfAR = d.BeginInvoke(3, new AsyncCallback(Callback), null);
            System.Threading.Thread.Sleep(10000);

            
        }
    }
}
