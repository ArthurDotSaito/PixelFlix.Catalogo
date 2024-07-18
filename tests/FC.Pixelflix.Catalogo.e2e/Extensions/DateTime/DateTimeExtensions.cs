namespace FC.Pixelflix.Catalogo.e2e.Extensions.DateTime;
using SystemDateTime = System.DateTime;

internal static class DateTimeExtensions
{
    public static SystemDateTime TrimMilliseconds(this SystemDateTime dateTime)
    {
        return new SystemDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second,0, dateTime.Kind);
    }
}