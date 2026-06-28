using System.Text;

namespace ProcGenLab.CellularAutomata.Enums;

public enum PropType
{
    Grass = 0,

    Flower = 1,

    Mushroom = 2,

    Lily = 17,

    Branch = 3,

    GroundRock = 4,

    Bush = 6,

    Oak = 50,

    Spruce = 51,

    WaterRock = 71
}

public static class PropTypeExtensions
{
    public static string ToTag(this PropType type)
    {
        var name = type.ToString();

        if (string.IsNullOrEmpty(name))
            return string.Empty;

        var builder = new StringBuilder();
        builder.Append(char.ToLower(name[0]));

        for (var i = 1; i < name.Length; i++)
        {
            var c = name[i];

            if (char.IsUpper(c))
            {
                builder.Append('_');
                builder.Append(char.ToLower(c));
            }
            else
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}