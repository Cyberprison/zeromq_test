﻿using NetMQ;
using NetMQ.Sockets;
using System;
using System.Diagnostics;
using System.IO;
namespace zeromq_test_client
{
    class Program
    {
        private static Process _childProcess;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true; 
                Console.WriteLine("Завершение основного приложения...");
                StopChildProcess();
            };
            StartChildProcess();
            using (var client = new RequestSocket(">tcp://127.0.0.1:5556"))
            {
                try
                {
                    client.SendFrame("Hello from client");
                    var msg = client.ReceiveFrameString();
                    Console.WriteLine(msg);
                    Console.WriteLine("Связь с сервером установлена.");
                    Console.WriteLine("Для завершения работы введите exit.");
                    while (true)
                    {
                        Console.WriteLine("Введите число");
                        var input = Console.ReadLine();
                        client.SendFrame(input);
                        msg = client.ReceiveFrameString();
                        Console.WriteLine(msg);
                        if (input == "exit")
                        {
                            break;
                        }
                    }
                }
                catch(Exception ex) { }
                finally
                {
                    StopChildProcess();
                }
            }
            Console.WriteLine("Нажмите любую клавишу для завершения работы консольного приложения...");
            Console.ReadKey();
        }

        private static void StartChildProcess()
        {
            _childProcess = new Process();
            _childProcess.StartInfo.FileName = Path
                .Combine(AppDomain.CurrentDomain.BaseDirectory + "\\win-x64\\zeromq_test_server");
            _childProcess.StartInfo.UseShellExecute = false;
            _childProcess.StartInfo.CreateNoWindow = true; 
            _childProcess.Start();
            Console.WriteLine("Дочерний процесс запущен.");
        }

        private static void StopChildProcess()
        {
            if (_childProcess != null && !_childProcess.HasExited)
            {
                _childProcess.Kill(); 
                _childProcess.Dispose();
                Console.WriteLine("Дочерний процесс завершен.");
            }
        }
    }
}
