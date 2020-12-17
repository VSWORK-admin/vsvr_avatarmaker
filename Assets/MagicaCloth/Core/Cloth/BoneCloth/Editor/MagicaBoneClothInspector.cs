// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// ボーンクロスのエディタ拡張
    /// </summary>
    [CustomEditor(typeof(MagicaBoneCloth))]
    public class MagicaBoneClothInspector : ClothEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            MagicaBoneCloth scr = target as MagicaBoneCloth;

            // データ状態
            EditorInspectorUtility.DispVersionStatus(scr);
            EditorInspectorUtility.DispDataStatus(scr);

            serializedObject.Update();
            Undo.RecordObject(scr, "CreateBoneCloth");

            // データ検証
            if (EditorApplication.isPlaying == false)
                VerifyData();

            // モニターボタン
            EditorInspectorUtility.MonitorButtonInspector();

            // メイン
            MainInspector();

            // コライダー
            ColliderInspector();

            // パラメータ
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorPresetUtility.DrawPresetButton(scr, scr.Params);
            {
                var cparam = serializedObject.FindProperty("clothParams");
                if (EditorInspectorUtility.RadiusInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.Radius);
                if (EditorInspectorUtility.MassInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.Mass);
                if (EditorInspectorUtility.GravityInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.Gravity);
                if (EditorInspectorUtility.ExternalForceInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.ExternalForce);
                if (EditorInspectorUtility.DragInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.Drag);
                if (EditorInspectorUtility.MaxVelocityInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.MaxVelocity);
                if (EditorInspectorUtility.WorldInfluenceInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.WorldInfluence);
                if (EditorInspectorUtility.DistanceDisableInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.DistanceDisable);

                if (EditorInspectorUtility.ClampDistanceInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.ClampDistance);
                if (EditorInspectorUtility.ClampPositionInspector(cparam, true))
                    scr.Params.SetChangeParam(ClothParams.ParamType.ClampPosition);
                if (EditorInspectorUtility.ClampRotationInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.ClampRotation);

                if (EditorInspectorUtility.RestoreDistanceInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.RestoreDistance);
                if (EditorInspectorUtility.RestoreRotationInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.RestoreRotation);
                if (EditorInspectorUtility.CollisionInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.ColliderCollision);
                if (EditorInspectorUtility.RotationInterpolationInspector(cparam))
                    scr.Params.SetChangeParam(ClothParams.ParamType.RotationInterpolation);
            }
            serializedObject.ApplyModifiedProperties();

            // データ作成
            if (EditorApplication.isPlaying == false)
            {
                EditorGUI.BeginDisabledGroup(CheckCreate() == false);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Create"))
                {
                    Undo.RecordObject(scr, "CreateBoneCloth");
                    CreateData();
                }
                GUI.backgroundColor = Color.white;

                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUI.backgroundColor = Color.blue;
                if (GUILayout.Button("Reset Position"))
                {
                    scr.ResetCloth();
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.Space();
        }

        //=========================================================================================
        /// <summary>
        /// 作成を実行できるか判定する
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCreate()
        {
            if (PointSelector.EditEnable)
                return false;

            return true;
        }

        /// <summary>
        /// データ検証
        /// </summary>
        private void VerifyData()
        {
            MagicaBoneCloth scr = target as MagicaBoneCloth;
            if (scr.VerifyData() != Define.Error.None)
            {
                // 検証エラー
                serializedObject.ApplyModifiedProperties();
            }
        }

        //=========================================================================================
        void MainInspector()
        {
            MagicaBoneCloth scr = target as MagicaBoneCloth;

            EditorGUILayout.LabelField("Main Setup", EditorStyles.boldLabel);

            // ルートリスト
            EditorInspectorUtility.DrawObjectList<Transform>(
                serializedObject.FindProperty("clothTarget.rootList"),
                scr.gameObject,
                false, true
                );
            //EditorGUILayout.Space();

            // ブレンド率
            UserBlendInspector();

            // アニメーション連動
            //scr.ClothTarget.IsAnimationBone = EditorGUILayout.Toggle("Is Animation Bones", scr.ClothTarget.IsAnimationBone);
            //scr.ClothTarget.IsAnimationPosition = EditorGUILayout.Toggle("Is Animation Position", scr.ClothTarget.IsAnimationPosition);
            //scr.ClothTarget.IsAnimationRotation = EditorGUILayout.Toggle("Is Animation Rotation", scr.ClothTarget.IsAnimationRotation);

            // ポイント選択
            DrawInspectorGUI(scr);
            EditorGUILayout.Space();
        }

        void ColliderInspector()
        {
            MagicaBoneCloth scr = target as MagicaBoneCloth;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Collider List", EditorStyles.boldLabel);
            EditorInspectorUtility.DrawObjectList<ColliderComponent>(
                serializedObject.FindProperty("teamData.colliderList"),
                scr.gameObject,
                true, true
                );
        }

        //=========================================================================================
        /// <summary>
        /// 選択データの初期化
        /// 配列はすでに頂点数分が確保されゼロクリアされています。
        /// </summary>
        /// <param name="selectorData"></param>
        protected override void OnResetSelector(List<int> selectorData)
        {
            // ルートトランスフォームのみ固定で初期化する
            MagicaBoneCloth scr = target as MagicaBoneCloth;

            // 現在の頂点選択データをコピー
            // また新規データの場合はボーン階層のルートを固定化する
            if (scr.ClothSelection != null)
            {
                // 既存
                var sel = scr.ClothSelection.GetSelectionData(scr.MeshData, null);
                for (int i = 0; i < selectorData.Count; i++)
                {
                    if (i < sel.Count)
                        selectorData[i] = sel[i];
                }
            }
            else
            {
                // 新規
                var tlist = scr.GetTransformList();
                for (int i = 0; i < tlist.Count; i++)
                {
                    var t = tlist[i];
                    int data = 0;
                    if (scr.ClothTarget.GetRootIndex(t) >= 0)
                    {
                        // 固定
                        data = SelectionData.Fixed;
                    }
                    else
                    {
                        // 移動
                        data = SelectionData.Move;
                    }
                    selectorData[i] = data;
                }
            }
        }

        /// <summary>
        /// 選択データの決定
        /// </summary>
        /// <param name="selectorData"></param>
        protected override void OnFinishSelector(List<int> selectorData)
        {
            MagicaBoneCloth scr = target as MagicaBoneCloth;

            // 必ず新規データを作成する（ヒエラルキーでのコピー対策）
            var sel = CreateSelection(scr, "clothSelection");

            // 選択データコピー
            sel.SetSelectionData(scr.MeshData, selectorData, null);

            // 現在のデータと比較し差異がない場合は抜ける
            if (scr.ClothSelection != null && scr.ClothSelection.Compare(sel))
                return;

            //if (scr.ClothSelection != null)
            //    Undo.RecordObject(scr.ClothSelection, "Set Selector");

            // 保存
            var cdata = serializedObject.FindProperty("clothSelection");
            cdata.objectReferenceValue = sel;
            serializedObject.ApplyModifiedProperties();
        }

        //=========================================================================================
        void CreateData()
        {
            MagicaBoneCloth scr = target as MagicaBoneCloth;

            Debug.Log("Started creating. [" + scr.name + "]");

            // 共有選択データが存在しない場合は作成する
            if (scr.ClothSelection == null)
                InitSelectorData();

            // チームハッシュ設定
            scr.TeamData.ValidateColliderList();

            // メッシュデータ作成
            CreateMeshData(scr);

            // クロスデータ作成
            CreateClothdata(scr);

            // 検証
            scr.CreateVerifyData();
            serializedObject.ApplyModifiedProperties();

            if (scr.VerifyData() == Define.Error.None)
                Debug.Log("Creation completed. [" + scr.name + "]");
            else
                Debug.LogError("Creation failed.");
        }

        /// <summary>
        /// メッシュデータ作成
        /// </summary>
        void CreateMeshData(MagicaBoneCloth scr)
        {
            // 共有データオブジェクト作成
            string dataname = "BoneClothMeshData_" + scr.name;
            MeshData mdata = ShareDataObject.CreateShareData<MeshData>(dataname);

            // トランスフォームリスト作成
            var transformList = scr.GetTransformList();
            if (transformList.Count == 0)
                return;

            // 頂点作成
            List<Vector3> wposList = new List<Vector3>();
            List<Vector3> lposList = new List<Vector3>();
            List<Vector3> lnorList = new List<Vector3>();
            List<Vector3> ltanList = new List<Vector3>();
            Transform myt = scr.transform;
            for (int i = 0; i < transformList.Count; i++)
            {
                var t = transformList[i];

                // 頂点追加
                var pos = t.position;
                var lpos = myt.InverseTransformDirection(pos - myt.position);
                var lnor = myt.InverseTransformDirection(t.forward);
                var ltan = myt.InverseTransformDirection(t.up);
                wposList.Add(pos);
                lposList.Add(lpos);
                lnorList.Add(lnor);
                ltanList.Add(ltan);
            }
            var vertexInfoList = new List<uint>();
            var vertexWeightList = new List<MeshData.VertexWeight>();
            for (int i = 0; i < lposList.Count; i++)
            {
                // １ウエイトで追加
                uint vinfo = DataUtility.Pack4_28(1, i);
                vertexInfoList.Add(vinfo);
                var vw = new MeshData.VertexWeight();
                vw.parentIndex = i;
                vw.weight = 1.0f;
                vw.localPos = lposList[i];
                vw.localNor = lnorList[i];
                vw.localTan = ltanList[i];
            }
            mdata.vertexInfoList = vertexInfoList.ToArray();
            mdata.vertexWeightList = vertexWeightList.ToArray();
            mdata.vertexCount = lposList.Count;

            // ライン作成
            HashSet<uint> lineSet = new HashSet<uint>();

            // 構造ライン
            for (int i = 0; i < transformList.Count; i++)
            {
                var t = transformList[i];
                var pt = t.parent;
                if (pt != null && transformList.Contains(pt))
                {
                    int v0 = i;
                    int v1 = transformList.IndexOf(pt);
                    uint pair = DataUtility.PackPair(v0, v1);
                    lineSet.Add(pair);
                }
            }

            // 近接ライン接続
            //if (scr.ClothTarget.LineConnection)
            //{
            //    CreateNearLine(scr, lineSet, wposList, mdata);
            //}

            // ライン格納
            List<int> lineList = new List<int>();
            foreach (var pair in lineSet)
            {
                int v0, v1;
                DataUtility.UnpackPair(pair, out v0, out v1);
                lineList.Add(v0);
                lineList.Add(v1);
            }
            mdata.lineList = lineList.ToArray();
            mdata.lineCount = lineList.Count / 2;

            serializedObject.FindProperty("meshData").objectReferenceValue = mdata;
            serializedObject.ApplyModifiedProperties();

            // 使用トランスフォームシリアライズ
            var property = serializedObject.FindProperty("useTransformList");
            var propertyPos = serializedObject.FindProperty("useTransformPositionList");
            var propertyRot = serializedObject.FindProperty("useTransformRotationList");
            var propertyScl = serializedObject.FindProperty("useTransformScaleList");
            property.arraySize = transformList.Count;
            propertyPos.arraySize = transformList.Count;
            propertyRot.arraySize = transformList.Count;
            propertyScl.arraySize = transformList.Count;
            for (int i = 0; i < transformList.Count; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue = transformList[i];
                propertyPos.GetArrayElementAtIndex(i).vector3Value = transformList[i].localPosition;
                propertyRot.GetArrayElementAtIndex(i).quaternionValue = transformList[i].localRotation;
                propertyScl.GetArrayElementAtIndex(i).vector3Value = transformList[i].localScale;
            }
            serializedObject.ApplyModifiedProperties();

            // データ検証とハッシュ
            mdata.CreateVerifyData();
            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(mdata);
        }

        /// <summary>
        /// 近接ラインの接続
        /// </summary>
        /// <param name="lineSet"></param>
        /// <param name="wposList"></param>
        /// <param name="mdata"></param>
        //void CreateNearLine(BoneCloth scr, HashSet<uint> lineSet, List<Vector3> wposList, MeshData mdata)
        //{
        //    for (int i = 0; i < (mdata.VertexCount - 1); i++)
        //    {
        //        for (int j = i + 1; j < mdata.VertexCount; j++)
        //        {
        //            float dist = Vector3.Distance(wposList[i], wposList[j]);
        //            if (dist <= scr.ClothTarget.ConnectionDistance)
        //            {
        //                // 接続
        //                uint pair = DataUtility.PackPair(i, j);
        //                lineSet.Add(pair);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// クロスデータ作成
        /// </summary>
        void CreateClothdata(MagicaBoneCloth scr)
        {
            if (scr.MeshData == null)
                return;

            // クロスデータ共有データ作成（既存の場合は選択状態のみコピーする）
            string dataname = "BoneClothData_" + scr.name;
            var cloth = ShareDataObject.CreateShareData<ClothData>(dataname);

            // クロスデータ作成
            cloth.CreateData(
                scr,
                scr.Params,
                scr.TeamData,
                scr.MeshData,
                scr,
                scr.ClothSelection.GetSelectionData(scr.MeshData, null)
                );
            serializedObject.FindProperty("clothData").objectReferenceValue = cloth;

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(cloth);
        }
    }
}