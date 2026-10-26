namespace DesktopCat;

public static class Constants
{
 public const string IdlePath =
        "avares://DesktopCat/Assets/Images/Bellota/idle.png";

    public const string Walk1Path =
        "avares://DesktopCat/Assets/Images/Bellota/walk1.png";

    public const string Walk2Path =
        "avares://DesktopCat/Assets/Images/Bellota/walk2.png";

    public const string SleepPath =
        "avares://DesktopCat/Assets/Images/Bellota/sleep.png";
    public const string GroomPath =
        "avares://DesktopCat/Assets/Images/Bellota/groom.png";

    // Frecuencia de actualización: 30 veces por segundo.
    public const int UpdateIntervalMs = 16;

    // Distancia mínima para considerar que Bellota debe caminar.
 //   public const double MovementThreshold = 12;

    // Velocidad máxima aproximada en píxeles por segundo.
    public const double FollowSpeed = 300;

    // Distancia mínima al objetivo para detenerse.
    public const double StopDistance = 12;

    // Separación respecto del puntero.
    public const int CursorOffsetX = 30;
    public const int CursorOffsetY = 30;

    // Tiempo sin movimiento para volver a Idle.
   // public const int IdleDelayMs = 500;

    // Duración de cada fotograma de caminata.
    public const int WalkFrameDurationMs = 140;

    // Distancia mínima para registrar un nuevo movimiento.
    public const double CursorMovementThreshold = 3;
}