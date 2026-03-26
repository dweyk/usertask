namespace UserTaskManagement.Domain.SeedWorks;

/// <summary>
/// Системное время
/// </summary>
public class SysTime
{
    /// <summary>
    /// Текущий провайдер времени
    /// </summary>
    public static Func<DateTime> CurrentTimeProvider { get; set; } = () => DateTime.UtcNow;
    
    /// <summary>
    /// Возвращает текущее время в Utc
    /// </summary>
    public static DateTime UtcNow() => CurrentTimeProvider();
}
