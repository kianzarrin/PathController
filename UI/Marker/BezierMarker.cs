namespace PathController.UI.Marker;

using KianCommons;
using PathController.CustomData;
using PathController.Manager;
using PathController.Util;
using UnityEngine;

public class BezierMarker {
    internal bool Focused =>
        A.Hovered || A.Selected ||
        B.Hovered || B.Selected ||
        C.Hovered || C.Selected ||
        D.Hovered || D.Selected;

    public LaneIdAndIndex LaneIdAndIndex;
    public CustomLane CustomLane => PathControllerManager.Instance.GetOrCreateLane(LaneIdAndIndex);
    public ControlPointMarker[] controlMarkers = new ControlPointMarker[4];
    public ControlPointMarker A => controlMarkers[0];
    public ControlPointMarker B => controlMarkers[1];
    public ControlPointMarker C => controlMarkers[2];
    public ControlPointMarker D => controlMarkers[3];


    public BezierMarker(LaneIdAndIndex laneIdAndIndex) {
        LaneIdAndIndex = laneIdAndIndex;
        var bezier = laneIdAndIndex.Lane.m_bezier;
        for (int i = 0; i < 4; ++i)
            controlMarkers[i] = new ControlPointMarker(bezier.ControlPoint(i));
    }


    /// <param name="focus">number field contains mouses</param>
    public void RenderOverlay(RenderManager.CameraInfo cameraInfo, Color color, bool fieldHovered = false) {
        foreach (ControlPointMarker c in controlMarkers) 
            c.RenderOverlay(cameraInfo, color, fieldHovered);
    }

    public bool Drag(Vector3 hitPos) {
        for (int i = 0; i < 4; ++i) {
            controlMarkers[i].UpdatePosition(CustomLane.GetControlPoint(i));
        }

        for (int i = 0; i < 4; ++i) {
            var controlMarker = controlMarkers[i];
            if (controlMarker.Drag(hitPos)) {
                CustomLane.UpdateControlPoint(i,controlMarker.Position);
                return true;
            }
        }
        return false;
    }
}


