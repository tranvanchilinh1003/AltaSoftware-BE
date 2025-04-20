using System.Globalization;

public static class DateTimeUtils
{
    public static DateTime? ConvertToUnspecified(DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Unspecified);
        }
        return null;
    }

      public static string FormatSessionTime(DateTime dateTime)
    {
        // Lấy ngày trong tuần bằng tiếng Việt
        string[] weekdays = { "Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
        string dayOfWeek = weekdays[(int)dateTime.DayOfWeek];

        // Định dạng ngày giờ theo yêu cầu
        string formattedDate = dateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        string formattedTime = dateTime.ToString("hh:mm tt", CultureInfo.InvariantCulture); // 12h AM/PM format

        return $"{dayOfWeek}, {formattedDate} {formattedTime}";
    }
}
