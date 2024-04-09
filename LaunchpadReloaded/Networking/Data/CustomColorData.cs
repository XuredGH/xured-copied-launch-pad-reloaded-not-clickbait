namespace LaunchpadReloaded.Networking.Data;

public struct CustomColorData(byte color, byte gradient)
{
    public readonly byte ColorId = color;
    public readonly byte GradientId = gradient;
}