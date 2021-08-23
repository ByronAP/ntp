using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ByronAP.Net.Ntp
{
    public static class NtpClient
    {
        public static async Task<NtpResult> GetNetworkTimeAsync(string ntpServer = "time.nist.gov")
        {
            if (ntpServer == null)
            {
                throw new ArgumentNullException(nameof(ntpServer));
            }

            const int daysTo1900 = 1900 * 365 + 95; // 95 = offset for leap-years etc.
            const long ticksPerSecond = 10000000L;
            const long ticksPerDay = 24 * 60 * 60 * ticksPerSecond;
            const long ticksTo1900 = daysTo1900 * ticksPerDay;

            try
            {
                var ntpData = new byte[48];
                ntpData[0] = 0x23; //(Octal) LeapIndicator = 0 (no warning), VersionNum = 4 (IPV4 and 6), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;

                foreach (var address in addresses)
                {
                    try
                    {
                        var ipEndPoint = new IPEndPoint(address, 123);

                        var pingDuration = Stopwatch.GetTimestamp();

                        var startTime = DateTimeOffset.UtcNow;

                        using (var socket = new Socket(ipEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp))
                        {
                            await socket.ConnectAsync(ipEndPoint);
                            socket.ReceiveTimeout = 5000;
                            socket.Send(ntpData);
                            pingDuration = Stopwatch.GetTimestamp();

                            socket.Receive(ntpData);
                            pingDuration = Stopwatch.GetTimestamp() - pingDuration;
                        }

                        var pingTicks = pingDuration * ticksPerSecond / Stopwatch.Frequency;

                        var intPart = (long)ntpData[40] << 24 | (long)ntpData[41] << 16 | (long)ntpData[42] << 8 | ntpData[43];
                        var fractPart = (long)ntpData[44] << 24 | (long)ntpData[45] << 16 | (long)ntpData[46] << 8 | ntpData[47];
                        var netTicks = intPart * ticksPerSecond + (fractPart * ticksPerSecond >> 32);

                        var networkDateTime = new DateTimeOffset(ticksTo1900 + netTicks + pingTicks / 2, TimeSpan.FromSeconds(0));

                        return new NtpResult(startTime, networkDateTime, new TimeSpan(pingTicks), address);
                    }
                    catch
                    {
                        // ignore so we can try the next address
                    }
                }

                // FAIL if we are here we were not able to get a valid answer
                return null;
            }
            catch
            {
                // FAIL
                return null;
            }
        }
    }
}