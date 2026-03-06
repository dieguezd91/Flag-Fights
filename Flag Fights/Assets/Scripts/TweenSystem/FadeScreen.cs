using System.Threading.Tasks;

public static class FadeScreen
{
    public static bool IsReady => ScreenFadeController.Instance != null;

    public static async Task FadeOut(float? duration = null)
    {
        if (!IsReady) return;
        await ScreenFadeController.Instance.FadeOut(duration);
    }

    public static async Task FadeIn(float? duration = null)
    {
        if (!IsReady) return;
        await ScreenFadeController.Instance.FadeIn(duration);
    }
public static void FadeAndLoadScene(string sceneName, float? duration = null)
{
    if (ScreenFadeController.Instance == null) return;
    ScreenFadeController.Instance.FadeAndLoadScene(sceneName, duration);
}
}
