using System;

namespace Authentication.Web.Api.Brokers.DateTimeBroker
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTime();
    }
}
