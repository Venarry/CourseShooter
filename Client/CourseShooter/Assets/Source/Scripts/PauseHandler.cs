public class PauseHandler
{
    private static int _pauseLevelCounter;

    public static bool IsPaused => _pauseLevelCounter > 0;

    public static void AddPauseLevel()
    {
        _pauseLevelCounter++;
    }

    public static void RemovePauseLevel()
    {
        if (_pauseLevelCounter <= 0)
            return;

        _pauseLevelCounter--;
    }
}
