using NetMQ;
using NetMQ.Sockets;
using System;

namespace zeromq_test_server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new ResponseSocket())
            {
                server.Bind("tcp://*:5556");
                string msg = server.ReceiveFrameString();
                Console.WriteLine("From Client: {0}", msg);
                server.SendFrame("World");
            }

            Console.ReadKey();
        }
    }
}
