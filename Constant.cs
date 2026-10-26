namespace DesktopCat;

public static class Constants
{
 public const string IdlePath =
        "avares://DesktopCat/Assets/Images/Bellota/idle.png";

    public const string Walk1Path =
        "avares://DesktopCat/Assets/Images/Bellota/walk1.png";

    public const string Walk2Path =
        "avares://DesktopCat/Assets/Images/Bellota/walk2.png";

public const string WalkFront1Path =
        "avares://DesktopCat/Assets/Images/Bellota/walkfront1.png";

    public const string WalkFront2Path =
        "avares://DesktopCat/Assets/Images/Bellota/walkfront2.png";

    public const string WalkBack1Path =
        "avares://DesktopCat/Assets/Images/Bellota/walkback1.png";

    public const string WalkBack2Path =
        "avares://DesktopCat/Assets/Images/Bellota/walkback2.png";

    public const string SleepPath =
        "avares://DesktopCat/Assets/Images/Bellota/sleep.png";
    public const string GroomPath =
        "avares://DesktopCat/Assets/Images/Bellota/groom.png";


    public const int UpdateIntervalMs = 16;

    // Velocidad máxima aproximada en píxeles por segundo.
    public const double FollowSpeed = 300;

    // Distancia mínima al objetivo para detenerse.
    public const double StopDistance = 12;

    // Separación respecto del puntero.
    public const int CursorOffsetX = 30;
    public const int CursorOffsetY = 30;

    // Duración de cada fotograma de caminata.
    public const int WalkFrameDurationMs = 140;

    // Distancia mínima para registrar un nuevo movimiento.
    public const double CursorMovementThreshold = 3;

    public const int IdleBeforeRestMs = 2000;
}