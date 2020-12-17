﻿// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using UnityEditor;

namespace MagicaCloth
{
    /// <summary>
    /// アバターパーツのエディタ拡張
    /// </summary>
    [CustomEditor(typeof(MagicaAvatarParts))]
    public class MagicaAvatarPartsInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            MagicaAvatarParts scr = target as MagicaAvatarParts;

            // データ状態
            //EditorInspectorUtility.DispVersionStatus(scr);
            EditorInspectorUtility.DispDataStatus(scr);
            //DrawDefaultInspector();
        }

        //=========================================================================================
    }
}