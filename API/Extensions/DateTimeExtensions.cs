namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age;
    }

    public static int GetAgeInMonths(this DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return (today.Year - dateOfBirth.Year) * 12 + today.Month - dateOfBirth.Month;
    }

    public static int GetAgeInDays(this DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return (today.ToDateTime(new TimeOnly()) - dateOfBirth.ToDateTime(new TimeOnly())).Days;
    }
}
