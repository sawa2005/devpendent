namespace Devpendent.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (value == null) return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
