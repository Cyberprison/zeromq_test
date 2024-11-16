using NetMQ;
using NetMQ.Sockets;
using System;
namespace zeromq_test_server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new ResponseSocket("@tcp://localhost:5556"))
            {
                try
                {
                    string msg = server.ReceiveFrameString();
                    server.SendFrame("Сервер запущен.");
                    while (true)
                    {
                        msg = server.ReceiveFrameString();
                        if (msg == "exit")
                        {
                            server.SendFrame("Сервер остановлен.");
                            break;
                        }
                        else
                        {
                            //IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "," };
                            if (double.TryParse(msg, out double number))
                            {
                                double result = number * number;
                                server.SendFrame(result.ToString());
                            }
                            else
                            {
                                server.SendFrame("Вы ввели не число. Попробуйте снова...");
                            }
                        }
                    }
                }
                catch (Exception ex) { }
                finally { }
            }
        }
    }
}
