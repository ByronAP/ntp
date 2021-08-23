using System;
using System.Net;

namespace ByronAP.Net.Ntp
{
    public class NtpResult
    {
        public DateTimeOffset QueryTime { get; }
        public DateTimeOffset NtpTime { get; }
        public TimeSpan TimeDifference { get; }
        public TimeSpan RoundTripTime { get; }
        public IPAddress ServerAddress { get; }

        internal NtpResult(DateTimeOffset queryTime, DateTimeOffset ntpTime, TimeSpan roundTripTime, IPAddress serverAddress)
        {
            QueryTime = queryTime;
            NtpTime = ntpTime;
            TimeDifference = queryTime - ntpTime;
            RoundTripTime = roundTripTime;
            ServerAddress = serverAddress;
        }
    }
}
