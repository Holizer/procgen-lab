using Godot;

namespace ProcGenLab.Lobby.Core;

public partial class Camera : Camera2D
{
    private static readonly float[] ZoomLevels = [0.25f, 0.5f, 1.0f, 2.0f, 3.0f, 4.0f];

    private int _currentZoomIndex = 2;

    private bool _isPanning;

    private float _targetZoom = 1.0f;

    [Export] public float ZoomLerpSpeed = 10.0f;

    public override void _Ready()
    {
        _targetZoom = Zoom.X;
        _currentZoomIndex = FindNearestZoomIndex(_targetZoom);
        _targetZoom = ZoomLevels[_currentZoomIndex];
    }

    public override void _Process(double delta)
    {
        var nextZoom = Mathf.Lerp(Zoom.X, _targetZoom, (float)delta * ZoomLerpSpeed);
        Zoom = Vector2.One * nextZoom;

        Position = Position.Round();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Middle)
            {
                _isPanning = mouseEvent.Pressed;

                return;
            }

            HandleMouseButton(mouseEvent);
        }

        if (@event is InputEventMouseMotion motionEvent && _isPanning)
            Pan(motionEvent);
    }

    private void CenterOn(Vector2 worldPos)
    {
        Position = worldPos.Round();
        _targetZoom = 1.0f;
        _currentZoomIndex = FindNearestZoomIndex(1.0f);
    }

    private void HandleMouseButton(InputEventMouseButton mouseEvent)
    {
        if (!mouseEvent.Pressed)
            return;

        switch (mouseEvent.ButtonIndex)
        {
            case MouseButton.WheelUp:
                ApplyZoomStep(true);

                break;

            case MouseButton.WheelDown:
                ApplyZoomStep(false);

                break;
        }
    }

    private void ApplyZoomStep(bool zoomIn)
    {
        _currentZoomIndex = zoomIn
            ? Mathf.Clamp(_currentZoomIndex + 1, 0, ZoomLevels.Length - 1)
            : Mathf.Clamp(_currentZoomIndex - 1, 0, ZoomLevels.Length - 1);

        _targetZoom = ZoomLevels[_currentZoomIndex];
    }

    private void Pan(InputEventMouseMotion motionEvent)
    {
        Position -= motionEvent.Relative / Zoom;
        Position = Position.Round();
    }

    private int FindNearestZoomIndex(float currentZoom)
    {
        var nearestIndex = 2;
        var minDiff = float.MaxValue;

        for (var i = 0; i < ZoomLevels.Length; i++)
        {
            var diff = Mathf.Abs(ZoomLevels[i] - currentZoom);

            if (diff < minDiff)
            {
                minDiff = diff;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }
}