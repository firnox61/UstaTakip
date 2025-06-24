namespace UstaTakip.Application.Interfaces.Logging
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
    }
}
