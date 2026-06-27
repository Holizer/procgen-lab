using Godot;
using Godot.Collections;

namespace ProcGenLab.Shared.UI;

public partial class ConfigRow
{
    public override void _ValidateProperty(Dictionary property)
    {
        base._ValidateProperty(property);

        var name = property["name"].AsString();

        if (_type == ConfigRowVariants.LineEdit)
        {
            if (name == "IsChecked") property["usage"] = (int)PropertyUsageFlags.NoEditor;
        }
        else if (_type == ConfigRowVariants.CheckBox)
        {
            if (name == "Value" || name == "MinValue" || name == "MaxValue" || name == "Suffix")
                property["usage"] = (int)PropertyUsageFlags.NoEditor;
        }
    }
}