namespace LaunchpadReloaded.Networking;

public struct CustomColorData(byte color, byte gradient)
{
    public readonly byte ColorId = color;
    public readonly byte GradientId = gradient;
}