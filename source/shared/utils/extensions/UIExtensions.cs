using System;
using System.Text;
using Godot;

namespace ProcGenLab.Shared.Utils;

public static class UIExtensions
{
    public static void FilterNumericInput(
        this LineEdit lineEdit,
        string newText,
        bool allowDecimal = true,
        bool allowMinus = true
    )
    {
        if (lineEdit == null)
            return;

        if (string.IsNullOrEmpty(newText))
        {
            lineEdit.Text = "";

            return;
        }

        var filtered = new StringBuilder(newText.Length);
        var hasDecimal = false;
        var originalCaret = lineEdit.CaretColumn;
        var removedBeforeCaret = 0;

        for (var i = 0; i < newText.Length; i++)
        {
            var c = newText[i];
            var isValid = false;

            if (char.IsDigit(c))
            {
                filtered.Append(c);
                isValid = true;
            }
            else if (c == '-' && i == 0 && allowMinus)
            {
                filtered.Append(c);
                isValid = true;
            }
            else if ((c == '.' || c == ',') && !hasDecimal && allowDecimal)
            {
                filtered.Append('.');
                hasDecimal = true;
                isValid = true;
            }

            if (!isValid && i < originalCaret) removedBeforeCaret++;
        }

        var result = filtered.ToString();

        if (newText != result)
        {
            lineEdit.Text = result;
            lineEdit.CaretColumn = Math.Max(0, originalCaret - removedBeforeCaret);
        }
    }
}