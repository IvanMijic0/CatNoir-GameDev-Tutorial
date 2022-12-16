namespace Enums
{
    public enum Axis
    {
        X,
        Y
    }

    public static class AxisExtension
    {
        public static string ToUnityAxis(this Axis axis)
        {
            return axis == Axis.X ? "Horizontal" : "Vertical";
        }
    }
}