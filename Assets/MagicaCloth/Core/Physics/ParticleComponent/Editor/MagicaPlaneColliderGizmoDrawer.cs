// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using UnityEditor;
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// MagicaPlaneColliderのギズモ表示
    /// </summary>
    public class MagicaPlaneColliderGizmoDrawer
    {
        //[DrawGizmo(GizmoType.Selected | GizmoType.InSelectionHierarchy | GizmoType.Active)]
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Active)]
        static void DrawGizmo(MagicaPlaneCollider scr, GizmoType gizmoType)
        {
            bool selected = (gizmoType & GizmoType.Selected) != 0 || (ClothMonitorMenu.Monitor != null && ClothMonitorMenu.Monitor.UI.AlwaysClothShow);

            if (selected == false)
                return;

            DrawGizmo(scr, selected);
        }

        public static void DrawGizmo(MagicaPlaneCollider scr, bool selected)
        {
            Gizmos.matrix = scr.transform.localToWorldMatrix;

            Gizmos.color = selected ? GizmoUtility.ColorCollider : GizmoUtility.ColorNonSelectedCollider;
            Vector3 size = new Vector3(1.0f, 0.0f, 1.0f);
            Gizmos.DrawWireCube(Vector3.zero, size);
            Gizmos.DrawLine(Vector3.zero, Vector3.up * 0.1f);

            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
