// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using UnityEditor;
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// MagicaSphereColliderのギズモ表示
    /// </summary>
    public class MagicaSphereColliderGizmoDrawer
    {
        //[DrawGizmo(GizmoType.Selected | GizmoType.InSelectionHierarchy | GizmoType.Active)]
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Active)]
        static void DrawGizmo(MagicaSphereCollider scr, GizmoType gizmoType)
        {
            bool selected = (gizmoType & GizmoType.Selected) != 0 || (ClothMonitorMenu.Monitor != null && ClothMonitorMenu.Monitor.UI.AlwaysClothShow);

            if (selected == false)
                return;

            DrawGizmo(scr, selected);
        }

        public static void DrawGizmo(MagicaSphereCollider scr, bool selected)
        {
            Gizmos.color = selected ? GizmoUtility.ColorCollider : GizmoUtility.ColorNonSelectedCollider;
            GizmoUtility.DrawWireSphere(
                scr.transform.position,
                scr.transform.rotation,
                Vector3.one * scr.GetScale(),
                scr.Radius,
                true,
                true
                );
        }
    }
}
