using System;

namespace ProcGenLab.Shared.UI;

public static class TooltipBus
{
    private static string _activeFocusDesc;
    private static string _activeHoverDesc;
    public static event Action<string> OnRowHovered;
    public static event Action OnRowUnhovered;

    public static void SetFocus(string desc)
    {
        _activeFocusDesc = desc;
        UpdateDisplay();
    }

    public static void ClearFocus()
    {
        _activeFocusDesc = null;
        UpdateDisplay();
    }

    public static void SetHover(string desc)
    {
        _activeHoverDesc = desc;
        UpdateDisplay();
    }

    public static void ClearHover()
    {
        _activeHoverDesc = null;
        UpdateDisplay();
    }

    private static void UpdateDisplay()
    {
        if (_activeHoverDesc != null)
            OnRowHovered?.Invoke(_activeHoverDesc);
        else if (_activeFocusDesc != null)
            OnRowHovered?.Invoke(_activeFocusDesc);
        else
            OnRowUnhovered?.Invoke();
    }
}