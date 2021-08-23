using ByronAP.Net.Ntp;
using System;
using System.Threading.Tasks;

namespace NtpDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Fetching current time from network time server.{Environment.NewLine}");
            try
            {
                // try and query the NTP server (supports IPV4 and V6)
                var ntpResult = await NtpClient.GetNetworkTimeAsync();

                if (ntpResult == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FAILED to get NTP time.");
                    Console.ResetColor();

                    return;
                }

                Console.WriteLine($"Server: {ntpResult.ServerAddress}");
                Console.WriteLine($"Local: {ntpResult.QueryTime:MM/dd/yy H:mm:ss.FFFF zzz}");
                Console.WriteLine($"NTP:   {ntpResult.NtpTime:MM/dd/yy H:mm:ss.FFFF zzz}");

                Console.ForegroundColor = Math.Abs(ntpResult.TimeDifference.TotalMilliseconds) > 5000 ? ConsoleColor.Red : ConsoleColor.Green;
                Console.WriteLine($"Diff: {ntpResult.TimeDifference}"); // a normal diff should be within less than half a second +-

                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine($"{Environment.NewLine}DONE");
            }
        }
    }
}
