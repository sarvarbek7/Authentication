using System;

namespace Authentication.Web.Api.Brokers.DateTimeBroker
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTimeOffset GetCurrentDateTime() => DateTimeOffset.UtcNow;
    }
}
