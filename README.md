# ByronAP.Net.WebSockets

ByronAP.Net.NTP is a .NET library that simplifies the usage of NTP (network time) servers.

## Installation

Package available [![NuGet version (SoftCircuits.Silk)](https://img.shields.io/nuget/v/ByronAP.Net.Ntp.svg?style=flat-square)](https://www.nuget.org/packages/ByronAP.Net.Ntp/)

## Usage

```c#
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
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.


## License
[MIT](https://choosealicense.com/licenses/mit/)
