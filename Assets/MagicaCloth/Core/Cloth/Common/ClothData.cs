// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// クロスデータ
    /// データは使用する頂点についてのみ保持する
    /// </summary>
    [System.Serializable]
    public class ClothData : ShareDataObject
    {
        /// <summary>
        /// データバージョン
        /// </summary>
        private const int DATA_VERSION = 4;

        // 頂点フラグ
        public const uint VertexFlag_End = 0x00010000; // 末端の頂点

        /// <summary>
        /// メッシュの利用する頂点インデックスのリスト
        /// これがそのままパーティクルとして作成される
        /// クロスデータはこのリストのインデックスをデータとして指すようにする
        /// </summary>
        public List<int> useVertexList = new List<int>();

        /// <summary>
        /// 頂点選択データ
        /// SelectionDataクラスのInvalid/Move/Fixed/Extend値
        /// </summary>
        public List<int> selectionData = new List<int>();

        /// <summary>
        /// 頂点の最大レベル
        /// </summary>
        public uint maxLevel;

        /// <summary>
        /// 頂点フラグ(上位16bit)とレベル(下位16bit)リスト
        /// </summary>
        public List<uint> vertexFlagLevelList = new List<uint>();

        /// <summary>
        /// 頂点深さデータ(0.0-1.0)
        /// </summary>
        public List<float> vertexDepthList = new List<float>();

        /// <summary>
        /// ルート頂点リスト(-1=なし)
        /// </summary>
        public List<int> rootList = new List<int>();

        /// <summary>
        /// 親頂点リスト(-1=なし)
        /// </summary>
        public List<int> parentList = new List<int>();

        /// <summary>
        /// 重力方向減衰のターゲットボーンの上方向（ローカル）
        /// </summary>
        public Vector3 directionalDampingUpDir = Vector3.up;

        /// <summary>
        /// 距離復元拘束（構造接続）
        /// </summary>
        public RestoreDistanceConstraint.RestoreDistanceData[] structDistanceDataList;
        public ReferenceDataIndex[] structDistanceReferenceList;

        /// <summary>
        /// 距離復元拘束（ベンド接続）
        /// </summary>
        public RestoreDistanceConstraint.RestoreDistanceData[] bendDistanceDataList;
        public ReferenceDataIndex[] bendDistanceReferenceList;

        /// <summary>
        /// 距離復元拘束（近接続）
        /// </summary>
        public RestoreDistanceConstraint.RestoreDistanceData[] nearDistanceDataList;
        public ReferenceDataIndex[] nearDistanceReferenceList;

        /// <summary>
        /// ルート距離拘束
        /// 対象距離リスト
        /// </summary>
        public ClampDistanceConstraint.ClampDistanceData[] rootDistanceDataList;
        public ReferenceDataIndex[] rootDistanceReferenceList;

        /// <summary>
        /// 回転復元拘束
        /// 復元ローカルベクトルリスト
        /// </summary>
        public RestoreRotationConstraint.RotationData[] restoreRotationDataList;
        public ReferenceDataIndex[] restoreRotationReferenceList;

        /// <summary>
        /// 最大回転拘束
        /// </summary>
        public ClampRotationConstraint.ClampRotationData[] clampRotationDataList;
        public ClampRotationConstraint.ClampRotationRootInfo[] clampRotationRootInfoList;

        /// <summary>
        /// 回転調整拘束
        /// このデータはモードがRotationLineの場合のみ必要
        /// またライン結合に対してのみ生成する
        /// </summary>
        public AdjustRotationWorker.AdjustRotationData[] adjustRotationDataList;

        /// <summary>
        /// トライアングルベンド拘束
        ///   v2 +
        ///     /|\
        /// v0 + | + v1
        ///     \|/
        ///   v3 +
        /// </summary>
        public TriangleBendConstraint.TriangleBendData[] triangleBendDataList;
        public ReferenceDataIndex[] triangleBendReferenceList;
        public int triangleBendWriteBufferCount;

        /// <summary>
        /// ボリューム拘束（テトラを形成する4頂点の情報）
        /// </summary>
        public VolumeConstraint.VolumeData[] volumeDataList;
        public ReferenceDataIndex[] volumeReferenceList;
        public int volumeWriteBufferCount;

        /// <summary>
        /// ライン回転調整
        /// </summary>
        public LineWorker.LineRotationData[] lineRotationDataList;
        public LineWorker.LineRotationRootInfo[] lineRotationRootInfoList;

        /// <summary>
        /// エッジコリジョン拘束
        /// </summary>
        public EdgeCollisionConstraint.EdgeCollisionData[] edgeCollisionDataList;
        public ReferenceDataIndex[] edgeCollisionReferenceList;
        public int edgeCollisionWriteBufferCount;

        //=========================================================================================
        /// <summary>
        /// データハッシュ計算
        /// </summary>
        /// <returns></returns>
        public override int GetDataHash()
        {
            int hash = 0;
            hash += VertexUseCount.GetDataHash();
            hash += selectionData.Count.GetDataHash();
            hash += maxLevel.GetDataHash();
            hash += vertexFlagLevelList.Count.GetDataHash();
            hash += vertexDepthList.Count.GetDataHash();
            hash += rootList.Count.GetDataHash();
            hash += parentList.Count.GetDataHash();

            hash += StructDistanceConstraintCount.GetDataHash();
            hash += BendDistanceConstraintCount.GetDataHash();
            hash += NearDistanceConstraintCount.GetDataHash();
            hash += ClampDistanceConstraintCount.GetDataHash();
            hash += RestoreRotationConstraintCount.GetDataHash();
            hash += ClampRotationConstraintDataCount.GetDataHash();
            hash += AdjustRotationConstraintCount.GetDataHash();
            hash += TriangleBendConstraintCount.GetDataHash();
            hash += VolumeConstraintCount.GetDataHash();
            hash += LineRotationWorkerCount.GetDataHash();

            return hash;
        }

        //=========================================================================================
        /// <summary>
        /// 使用頂点数
        /// </summary>
        public int VertexUseCount
        {
            get
            {
                return useVertexList.Count;
            }
        }

        /// <summary>
        /// 距離復元拘束数（構造接続）
        /// </summary>
        public int StructDistanceConstraintCount
        {
            get
            {
                return structDistanceDataList != null ? structDistanceDataList.Length : 0;
            }
        }

        /// <summary>
        /// 距離復元拘束数（ベンド接続）
        /// </summary>
        public int BendDistanceConstraintCount
        {
            get
            {
                return bendDistanceDataList != null ? bendDistanceDataList.Length : 0;
            }
        }

        /// <summary>
        /// 距離復元拘束数（近接続）
        /// </summary>
        public int NearDistanceConstraintCount
        {
            get
            {
                return nearDistanceDataList != null ? nearDistanceDataList.Length : 0;
            }
        }

        /// <summary>
        /// ルート距離拘束数
        /// </summary>
        public int ClampDistanceConstraintCount
        {
            get
            {
                return rootDistanceDataList != null ? rootDistanceDataList.Length : 0;
            }
        }

        /// <summary>
        /// 回転復元拘束数
        /// </summary>
        public int RestoreRotationConstraintCount
        {
            get
            {
                return restoreRotationDataList != null ? restoreRotationDataList.Length : 0;
            }
        }

        /// <summary>
        /// 回転制限拘束数
        /// </summary>
        public int ClampRotationConstraintDataCount
        {
            get
            {
                return clampRotationDataList != null ? clampRotationDataList.Length : 0;
            }
        }

        /// <summary>
        /// 回転角度拘束数
        /// </summary>
        public int ClampRotationConstraintRootCount
        {
            get
            {
                return clampRotationRootInfoList != null ? clampRotationRootInfoList.Length : 0;
            }
        }

        /// <summary>
        /// 回転調整拘束数
        /// </summary>
        public int AdjustRotationConstraintCount
        {
            get
            {
                return adjustRotationDataList != null ? adjustRotationDataList.Length : 0;
            }
        }

        /// <summary>
        /// トライアングルベンド拘束数
        /// </summary>
        public int TriangleBendConstraintCount
        {
            get
            {
                return triangleBendDataList != null ? triangleBendDataList.Length : 0;
            }
        }

        public int EdgeCollisionConstraintCount
        {
            get
            {
                return edgeCollisionDataList != null ? edgeCollisionDataList.Length : 0;
            }
        }

        /// <summary>
        /// ボリューム拘束数
        /// </summary>
        public int VolumeConstraintCount
        {
            get
            {
                return volumeDataList != null ? volumeDataList.Length : 0;
            }
        }

        /// <summary>
        /// ライン回転調整数
        /// </summary>
        public int LineRotationWorkerCount
        {
            get
            {
                return lineRotationDataList != null ? lineRotationDataList.Length : 0;
            }
        }

        /// <summary>
        /// 選択頂点が無効か判定
        /// </summary>
        /// <param name="vindex"></param>
        /// <returns></returns>
        public bool IsInvalidVertex(int vindex)
        {
            Debug.Assert(selectionData != null && vindex < selectionData.Count);
            return selectionData[vindex] == SelectionData.Invalid;
        }

        /// <summary>
        /// 選択頂点が固定か判定
        /// </summary>
        /// <param name="vindex"></param>
        /// <returns></returns>
        public bool IsFixedVertex(int vindex)
        {
            Debug.Assert(selectionData != null && vindex < selectionData.Count);
            return selectionData[vindex] == SelectionData.Fixed;
        }

        /// <summary>
        /// 選択頂点が移動か判定
        /// </summary>
        /// <param name="vindex"></param>
        /// <returns></returns>
        public bool IsMoveVertex(int vindex)
        {
            Debug.Assert(selectionData != null && vindex < selectionData.Count);
            return selectionData[vindex] == SelectionData.Move;
        }

        /// <summary>
        /// 選択頂点が拡張か判定
        /// </summary>
        /// <param name="vindex"></param>
        /// <returns></returns>
        public bool IsExtendVertex(int vindex)
        {
            Debug.Assert(selectionData != null && vindex < selectionData.Count);
            return selectionData[vindex] == SelectionData.Extend;
        }

        /// <summary>
        /// 指定頂点が末端か判定
        /// </summary>
        /// <param name="vindex"></param>
        /// <returns></returns>
        public bool IsLastLevel(int vindex)
        {
            return IsFlag(vindex, VertexFlag_End);
        }

        /// <summary>
        /// 頂点フラグ取得
        /// </summary>
        /// <param name="vindex"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool IsFlag(int vindex, uint flag)
        {
            return (vertexFlagLevelList[vindex] & flag) != 0;
        }

        /// <summary>
        /// 頂点フラグ設定
        /// </summary>
        /// <param name="vindex"></param>
        /// <param name="flag"></param>
        public void SetFlag(int vindex, uint flag)
        {
            vertexFlagLevelList[vindex] |= flag;
        }

        /// <summary>
        /// 頂点レベル取得
        /// </summary>
        /// <param name="vindex"></param>
        /// <returns></returns>
        public int GetLevel(int vindex)
        {
            return (int)(vertexFlagLevelList[vindex] & 0xffff);
        }

        //=========================================================================================
        public override int GetVersion()
        {
            return DATA_VERSION;
        }

        /// <summary>
        /// 現在のデータが正常（実行できる状態）か返す
        /// </summary>
        /// <returns></returns>
        public override Define.Error VerifyData()
        {
            if (dataHash == 0)
                return Define.Error.InvalidDataHash;
            //if (dataVersion != GetVersion())
            //    return Define.Error.DataVersionMismatch;
            int vcnt = VertexUseCount;
            if (vcnt == 0)
                return Define.Error.VertexUseCountZero;
            if (selectionData.Count == 0 || selectionData.Count != vcnt)
                return Define.Error.SelectionDataCountMismatch;
            if (vertexFlagLevelList.Count == 0 || vertexFlagLevelList.Count != vcnt)
                return Define.Error.VertexCountMismatch;
            if (vertexDepthList.Count == 0 || vertexDepthList.Count != vcnt)
                return Define.Error.VertexCountMismatch;
            if (rootList.Count == 0 || rootList.Count != vcnt)
                return Define.Error.RootListCountMismatch;

            return Define.Error.None;
        }

        //=========================================================================================
        private class RestoreDistanceWork
        {
            public uint vertexPair;
            public float dist;
        }

        //=========================================================================================
        /// <summary>
        /// クロスデータの作成
        /// </summary>
        /// <param name="teamData"></param>
        /// <param name="editMesh"></param>
        public void CreateData(
            PhysicsTeam team,
            ClothParams clothParams,
            PhysicsTeamData teamData,
            MeshData meshData,
            IEditorMesh editMesh,
            List<int> selData,
            System.Action<List<int>, List<int>, List<Vector3>, List<Vector3>, List<Vector3>, List<int>, List<int>> extensionAction = null
            )
        {
            Debug.Assert(teamData != null);
            Debug.Assert(editMesh != null);

            // メッシュデータ
            int vertexCount = 0;
            List<Vector3> wposList;
            List<Vector3> wnorList;
            List<Vector3> wtanList;
            List<int> triangleList;
            List<int> lineList;
            vertexCount = editMesh.GetEditorPositionNormalTangent(out wposList, out wnorList, out wtanList);
            triangleList = editMesh.GetEditorTriangleList();
            lineList = editMesh.GetEditorLineList();

            // 使用頂点データ作成
            useVertexList.Clear();
            Debug.Assert(vertexCount == selData.Count);
            for (int i = 0; i < vertexCount; i++)
            {
                if (selData[i] != SelectionData.Invalid)
                {
                    useVertexList.Add(i);
                    selectionData.Add(selData[i]);
                }
            }

            // 頂点データの拡張（オプション）
            if (extensionAction != null)
            {
                extensionAction(useVertexList, selectionData, wposList, wnorList, wtanList, triangleList, lineList);
                vertexCount = wposList.Count;
            }

            // 頂点データ作成
            CreateVertexData(vertexCount, lineList, triangleList);

            // 拘束データ作成
            CreateConstraintData(team, clothParams, teamData, vertexCount, wposList, wnorList, wtanList, lineList, triangleList);

            // データ検証とハッシュ
            CreateVerifyData();
        }

        /// <summary>
        /// 頂点データ作成
        /// </summary>
        /// <param name="vertexCount"></param>
        /// <param name="wposList"></param>
        /// <param name="wrotList"></param>
        /// <param name="triangleList"></param>
        void CreateVertexData(
            int vertexCount,
            List<int> lineList,
            List<int> triangleList
            )
        {
            vertexDepthList.Clear();
            vertexFlagLevelList.Clear();
            maxLevel = 0;

            if (vertexCount == 0)
            {
                return;
            }
            float[] depthList = new float[vertexCount]; // 0で初期化
            uint[] levelList = new uint[vertexCount];

            // ライン／トライアングル情報を分解して各頂点の接続頂点をリスト化
            List<HashSet<int>> vlink = MeshUtility.GetTriangleToVertexLinkList(vertexCount, lineList, triangleList);

            // 固定頂点を抜粋する
            uint lv = 1;
            HashSet<int> processedSet = new HashSet<int>();
            for (int i = 0; i < vertexCount; i++)
            {
                int index = useVertexList.IndexOf(i);
                if (index < 0)
                {
                    // 無効頂点
                    processedSet.Add(i);
                }

                else if (IsInvalidVertex(index) || IsExtendVertex(index))
                {
                    // 無効頂点、拡張頂点
                    processedSet.Add(i);
                }
                else if (IsFixedVertex(index))
                {
                    // 固定頂点
                    processedSet.Add(i);
                    levelList[i] = lv;
                }
            }

            // 階層
            int hitcnt;
            do
            {
                hitcnt = 0;
                for (int i = 0; i < vertexCount; i++)
                {
                    if (processedSet.Contains(i))
                        continue;

                    var vlist = vlink[i];
                    foreach (var vindex in vlist)
                    {
                        if (levelList[vindex] == lv)
                        {
                            // 次のレベルへ設定する
                            levelList[i] = lv + 1;
                            processedSet.Add(i);
                            hitcnt++;
                            break;
                        }
                    }
                }

                if (hitcnt > 0)
                    lv++;
            }
            while (hitcnt > 0);

            //Debug.Log("max level->" + lv);

            // デプス設定
            if (lv > 1)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    uint level = levelList[i];
                    if (level > 0)
                    {
                        depthList[i] = Mathf.Clamp01((float)(level - 1) / (float)(lv - 1));
                    }
                }
            }
            foreach (var vindex in useVertexList)
            {
                vertexDepthList.Add(depthList[vindex]);
                vertexFlagLevelList.Add(levelList[vindex]);
            }

            maxLevel = lv;
        }

        /// <summary>
        /// 拘束データ作成
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="vertexCount"></param>
        /// <param name="wposList"></param>
        /// <param name="wrotList"></param>
        /// <param name="triangleList"></param>
        void CreateConstraintData(
            PhysicsTeam team,
            ClothParams clothParams,
            PhysicsTeamData teamData,
            int vertexCount,
            List<Vector3> wposList,
            List<Vector3> wnorList,
            List<Vector3> wtanList,
            List<int> lineList,
            List<int> triangleList
            //List<int> tetraList,
            //List<float> tetraSizeList
            )
        {
            parentList.Clear();
            rootList.Clear();
            structDistanceDataList = null;
            structDistanceReferenceList = null;
            bendDistanceDataList = null;
            bendDistanceReferenceList = null;
            nearDistanceDataList = null;
            nearDistanceReferenceList = null;
            rootDistanceDataList = null;
            rootDistanceReferenceList = null;
            restoreRotationDataList = null;
            restoreRotationReferenceList = null;
            adjustRotationDataList = null;
            triangleBendDataList = null;
            triangleBendReferenceList = null;
            triangleBendWriteBufferCount = 0;
            volumeDataList = null;
            volumeReferenceList = null;
            volumeWriteBufferCount = 0;
            clampRotationDataList = null;
            clampRotationRootInfoList = null;
            lineRotationDataList = null;
            lineRotationRootInfoList = null;
            edgeCollisionDataList = null;
            edgeCollisionReferenceList = null;
            edgeCollisionWriteBufferCount = 0;
            //teamSelfCollisionDataList = null;
            if (vertexCount == 0)
                return;

            // ライン／トライアングル情報を分解して各頂点の接続頂点をリスト化
            List<HashSet<int>> meshVLink = MeshUtility.GetTriangleToVertexLinkList(vertexCount, lineList, triangleList);

            // 使用頂点ベースの接続頂点情報
            List<HashSet<int>> useVLink = new List<HashSet<int>>();
            for (int i = 0; i < useVertexList.Count; i++)
            {
                int vindex0 = useVertexList[i];
                HashSet<int> hset = new HashSet<int>();
                foreach (var vindex1 in meshVLink[vindex0])
                {
                    int ui = useVertexList.IndexOf(vindex1);
                    if (ui >= 0)
                    {
                        hset.Add(ui);
                    }
                }
                useVLink.Add(hset);
            }

            // トライアングルパックデータ
            //List<ulong> meshTrianglePackList = MeshUtility.GetTrianglePackList(triangleList);

            // 使用頂点の親頂点を計算してリスト化
            parentList = GetUseParentVertexList(vertexCount, meshVLink, wposList, vertexDepthList);
            //parentList = GetUseParentVertexList_Old(vertexCount, meshVLink, wposList, vertexDepthList);

            // 末端頂点の判定
            for (int i = 0; i < useVertexList.Count; i++)
            {
                if (parentList.Contains(i) == false && selectionData[i] != SelectionData.Extend)
                {
                    // この頂点は末端
                    SetFlag(i, VertexFlag_End);
                }
            }

            // 使用頂点のルート頂点を計算してリスト化
            rootList = GetUseRootVertexList(parentList);

            // メッシュ頂点リストに対応するデプス値
            List<float> meshVertexDepthList = GetMeshVertexDepthList(vertexCount, vertexDepthList);

            // 使用頂点ベースのルートから続く一連の頂点リスト
            List<List<int>> rootLineList = GetUseRootLineList(parentList);

            // 重力方向減衰
            var directionalDampingTarget = clothParams.DirectionalDampingObject ? clothParams.DirectionalDampingObject : team.transform;
            directionalDampingUpDir = directionalDampingTarget.InverseTransformDirection(Vector3.up);

            // 構造距離拘束リスト作成（構造接続）
            var restoreDistanceData = new List<RestoreDistanceConstraint.RestoreDistanceData>();
            HashSet<uint> distSet = new HashSet<uint>();
            for (int i = 0; i < useVertexList.Count; i++)
            {
                if (IsInvalidVertex(i) || IsExtendVertex(i))
                    continue;

                int vindex0 = useVertexList[i];

                var vlist = meshVLink[vindex0];
                foreach (var vindex in vlist)
                {
                    int vi = useVertexList.IndexOf(vindex);
                    if (vi < 0)
                        continue;

                    if (IsInvalidVertex(vi) || IsExtendVertex(vi))
                        continue;
                    if ((IsFixedVertex(i) || IsExtendVertex(i)) && (IsFixedVertex(vi) || IsExtendVertex(vi)))
                        continue;

                    uint pack = DataUtility.PackPair(i, vi);
                    if (distSet.Contains(pack))
                        continue;

                    // 登録
                    //float dist = Vector3.Distance(wposList[vindex0], wposList[vindex]);
                    // (1)
                    var data = new RestoreDistanceConstraint.RestoreDistanceData();
                    data.vertexIndex = (ushort)i;
                    data.targetVertexIndex = (ushort)vi;
                    //data.length = dist;
                    restoreDistanceData.Add(data);

                    // (2)
                    data = new RestoreDistanceConstraint.RestoreDistanceData();
                    data.vertexIndex = (ushort)vi;
                    data.targetVertexIndex = (ushort)i;
                    //data.length = dist;
                    restoreDistanceData.Add(data);

                    distSet.Add(pack);
                }
            }
            if (restoreDistanceData.Count > 0)
            {
                // ジョブシステム用にデータを加工して登録
                var builder = new ReferenceDataBuilder<RestoreDistanceConstraint.RestoreDistanceData>();
                builder.Init(useVertexList.Count);
                foreach (var data in restoreDistanceData)
                {
                    builder.AddData(data, data.vertexIndex);
                }
                (var refDataList, var dataList) = builder.GetDirectReferenceData();
                this.structDistanceDataList = dataList.ToArray();
                this.structDistanceReferenceList = refDataList.ToArray();
            }

            // 距離拘束リスト作成（ベンド接続）
            if (clothParams.UseBendDistance)
            {
                restoreDistanceData.Clear();
                for (int i = 0; i < useVertexList.Count; i++)
                {
                    if (IsInvalidVertex(i) || IsExtendVertex(i))
                        continue;
                    int vindex0 = useVertexList[i];
                    var pos0 = wposList[vindex0];

                    // vindex0から接続されるすべての頂点を総当りで連結する
                    List<RestoreDistanceWork> bendList = new List<RestoreDistanceWork>();
                    var vlist = new List<int>(meshVLink[vindex0]);
                    for (int j = 0; j < vlist.Count - 1; j++)
                    {
                        int vindex1 = vlist[j];
                        int vi1 = useVertexList.IndexOf(vindex1);
                        if (vi1 < 0)
                            continue;
                        if (IsInvalidVertex(vi1) || IsExtendVertex(vi1))
                            continue;

                        for (int k = j + 1; k < vlist.Count; k++)
                        {
                            int vindex2 = vlist[k];
                            int vi2 = useVertexList.IndexOf(vindex2);
                            if (vi2 < 0)
                                continue;
                            if (IsInvalidVertex(vi2) || IsExtendVertex(vi2))
                                continue;

                            if ((IsFixedVertex(vi1) || IsExtendVertex(vi1)) && (IsFixedVertex(vi2) || IsExtendVertex(vi2)))
                                continue;

                            uint pack = DataUtility.PackPair(vi1, vi2);
                            var work = new RestoreDistanceWork();
                            work.vertexPair = pack;
                            // ラインvindex1 - vindex2 と vindex0 の最近接点距離を求める
                            var pos1 = wposList[vindex1];
                            var pos2 = wposList[vindex2];
                            float3 d = MathUtility.ClosestPtPointSegment(pos0, pos1, pos2);
                            work.dist = math.distance(pos0, d);
                            bendList.Add(work);
                        }
                    }

                    // 距離の昇順でソート
                    bendList.Sort((a, b) => a.dist < b.dist ? -1 : 1);

                    // 最大BendDistanceMaxCountまでを接続する
                    for (int j = 0; j < bendList.Count && j < clothParams.BendDistanceMaxCount; j++)
                    {
                        var work = bendList[j];

                        // すでに登録済みならば無視
                        if (distSet.Contains(work.vertexPair))
                            continue;

                        int vi1, vi2;
                        DataUtility.UnpackPair(work.vertexPair, out vi1, out vi2);
                        int vindex1 = useVertexList[vi1];
                        int vindex2 = useVertexList[vi2];

                        // 登録
                        //float dist = Vector3.Distance(wposList[vindex1], wposList[vindex2]);
                        // (1)
                        var data = new RestoreDistanceConstraint.RestoreDistanceData();
                        data.vertexIndex = (ushort)vi1;
                        data.targetVertexIndex = (ushort)vi2;
                        //data.length = dist;
                        restoreDistanceData.Add(data);

                        // (2)
                        data = new RestoreDistanceConstraint.RestoreDistanceData();
                        data.vertexIndex = (ushort)vi2;
                        data.targetVertexIndex = (ushort)vi1;
                        //data.length = dist;
                        restoreDistanceData.Add(data);

                        distSet.Add(work.vertexPair);
                    }
                }

                if (restoreDistanceData.Count > 0)
                {
                    // ジョブシステム用にデータを加工して登録
                    var builder = new ReferenceDataBuilder<RestoreDistanceConstraint.RestoreDistanceData>();
                    builder.Init(useVertexList.Count);
                    foreach (var data in restoreDistanceData)
                    {
                        builder.AddData(data, data.vertexIndex);
                    }
                    (var refDataList, var dataList) = builder.GetDirectReferenceData();
                    this.bendDistanceDataList = dataList.ToArray();
                    this.bendDistanceReferenceList = refDataList.ToArray();
                }
            }

            // 距離拘束リスト作成（近接続）
            if (clothParams.UseNearDistance)
            {
                restoreDistanceData.Clear();
                for (int i = 0; i < useVertexList.Count; i++)
                {
                    if (IsMoveVertex(i) == false)
                        continue;

                    int vindex0 = useVertexList[i];
                    var wpos0 = wposList[vindex0];
                    float depth = vertexDepthList[i];

                    // デプス判定
                    if (vertexDepthList[i] > clothParams.NearDistanceMaxDepth)
                        continue;

                    List<RestoreDistanceWork> linkList = new List<RestoreDistanceWork>();
                    var useVinfo = useVLink[i];
                    for (int j = 0; j < useVertexList.Count; j++)
                    {
                        if (i == j)
                            continue;
                        // すでに接続されている点は無視
                        if (useVinfo.Contains(j))
                            continue;

                        // デプス判定
                        if (vertexDepthList[j] > clothParams.NearDistanceMaxDepth)
                            continue;

                        uint pack = DataUtility.PackPair(i, j);
                        if (distSet.Contains(pack))
                            continue;

                        // 距離判定
                        int vindex1 = useVertexList[j];
                        var wpos1 = wposList[vindex1];
                        float dist = Vector3.Distance(wpos0, wpos1);
                        if (dist <= clothParams.GetNearDistanceLength().Evaluate(vertexDepthList[i]))
                        {
                            var work = new RestoreDistanceWork();
                            work.vertexPair = pack;
                            work.dist = dist;
                            linkList.Add(work);
                        }
                    }

                    // 距離の昇順でソート
                    linkList.Sort((a, b) => a.dist < b.dist ? -1 : 1);

                    // 最大NearDistanceMaxCountまでを接続する
                    for (int j = 0; j < linkList.Count && j < clothParams.NearDistanceMaxCount; j++)
                    {
                        var work = linkList[j];

                        // すでに登録済みならば無視
                        if (distSet.Contains(work.vertexPair))
                            continue;

                        int i1, i2;
                        DataUtility.UnpackPair(work.vertexPair, out i1, out i2);

                        // 接続する
                        // (1)
                        var data = new RestoreDistanceConstraint.RestoreDistanceData();
                        data.vertexIndex = (ushort)i1;
                        data.targetVertexIndex = (ushort)i2;
                        //data.length = dist;
                        restoreDistanceData.Add(data);

                        // (2)
                        data = new RestoreDistanceConstraint.RestoreDistanceData();
                        data.vertexIndex = (ushort)i2;
                        data.targetVertexIndex = (ushort)i1;
                        //data.length = dist;
                        restoreDistanceData.Add(data);

                        distSet.Add(work.vertexPair);
                    }
                }

                if (restoreDistanceData.Count > 0)
                {
                    // ジョブシステム用にデータを加工して登録
                    var builder = new ReferenceDataBuilder<RestoreDistanceConstraint.RestoreDistanceData>();
                    builder.Init(useVertexList.Count);
                    foreach (var data in restoreDistanceData)
                    {
                        builder.AddData(data, data.vertexIndex);
                    }
                    (var refDataList, var dataList) = builder.GetDirectReferenceData();
                    this.nearDistanceDataList = dataList.ToArray();
                    this.nearDistanceReferenceList = refDataList.ToArray();
                }
            }

            // ルート距離拘束
            var rootDistanceData = new List<ClampDistanceConstraint.ClampDistanceData>();
            if (clothParams.UseClampDistanceRatio)
            {
                for (int i = 0; i < useVertexList.Count; i++)
                {
                    if (IsInvalidVertex(i))
                        continue;
                    if (IsMoveVertex(i) == false)
                        continue;

                    if (rootList[i] >= 0)
                    {
                        //int vindex = useVertexList[i];
                        //int rvindex = useVertexList[rootList[i]];

                        //float dist = Vector3.Distance(wposList[vindex], wposList[rvindex]);

                        // 登録
                        var data = new ClampDistanceConstraint.ClampDistanceData();
                        data.vertexIndex = (ushort)i;
                        data.targetVertexIndex = (ushort)rootList[i];
                        //data.length = dist;
                        rootDistanceData.Add(data);
                    }
                }
            }
            if (rootDistanceData.Count > 0)
            {
                // ジョブシステム用にデータを加工して登録
                var builder = new ReferenceDataBuilder<ClampDistanceConstraint.ClampDistanceData>();
                builder.Init(useVertexList.Count);
                foreach (var data in rootDistanceData)
                {
                    builder.AddData(data, data.vertexIndex);
                }
                (var refDataList, var dataList) = builder.GetDirectReferenceData();
                this.rootDistanceDataList = dataList.ToArray();
                this.rootDistanceReferenceList = refDataList.ToArray();
            }

            // 回転復元拘束（親ラインに対して）
            var restoreRotationData = new List<RestoreRotationConstraint.RotationData>();
            if (clothParams.UseRestoreRotation)
            {
                for (int i = 0; i < useVertexList.Count; i++)
                {
                    if (IsInvalidVertex(i) || IsExtendVertex(i))
                        continue;
                    if (IsMoveVertex(i) == false)
                        continue;

                    int pindex = parentList[i];
                    if (pindex >= 0)
                    {
                        // 登録
                        var data = new RestoreRotationConstraint.RotationData();
                        data.vertexIndex = (ushort)i;
                        data.targetVertexIndex = (ushort)pindex;
                        restoreRotationData.Add(data);
                    }
                }
            }
            if (restoreRotationData.Count > 0)
            {
                // ジョブシステム用にデータを加工して登録
                var builder = new ReferenceDataBuilder<RestoreRotationConstraint.RotationData>();
                builder.Init(useVertexList.Count);
                foreach (var data in restoreRotationData)
                {
                    builder.AddData(data, data.vertexIndex);
                }
                (var refDataList, var dataList) = builder.GetDirectReferenceData();
                this.restoreRotationDataList = dataList.ToArray();
                this.restoreRotationReferenceList = refDataList.ToArray();
            }

            // 回転角度拘束（ルートラインごと）
            var clampRotationData = new List<ClampRotationConstraint.ClampRotationData>();
            var clampRotationRootInfo = new List<ClampRotationConstraint.ClampRotationRootInfo>();
            if (clothParams.UseClampRotation)
            {
                foreach (var lineIndexList in rootLineList)
                {
                    if (lineIndexList.Count <= 1)
                        continue;

                    var info = new ClampRotationConstraint.ClampRotationRootInfo();
                    info.startIndex = (ushort)clampRotationData.Count;
                    info.dataLength = (ushort)lineIndexList.Count;

                    for (int i = 0; i < lineIndexList.Count; i++)
                    {
                        int index = lineIndexList[i];
                        int pindex = parentList[index];

                        var data = new ClampRotationConstraint.ClampRotationData();
                        data.vertexIndex = index;
                        data.parentVertexIndex = pindex;
                        clampRotationData.Add(data);
                    }
                    clampRotationRootInfo.Add(info);
                }
            }
            if (clampRotationData.Count > 0)
            {
                this.clampRotationDataList = clampRotationData.ToArray();
                this.clampRotationRootInfoList = clampRotationRootInfo.ToArray();
            }

            // ライン回転調整
            var lineRotationData = new List<LineWorker.LineRotationData>();
            var lineRotationRootInfo = new List<LineWorker.LineRotationRootInfo>();
            if (lineList.Count > 0)
            {
                // 頂点情報リスト
                var infoList = GetUseVertexInfoList(parentList);

                var compSet = new HashSet<int>();

                // ラインデータ基準
                for (int i = 0; i < lineList.Count; i++)
                {
                    int vindex = lineList[i];
                    if (compSet.Contains(vindex))
                        continue; // 処理済み
                    compSet.Add(vindex);

                    int index = useVertexList.IndexOf(vindex);
                    if (index < 0)
                        continue;

                    var vinfo = infoList[index];
                    if (vinfo.parentVertexIndex >= 0)
                        continue;

                    // ここから頂点階層をトレースする
                    var info = new LineWorker.LineRotationRootInfo();
                    info.startIndex = (ushort)lineRotationData.Count;
                    int startIndex = lineRotationData.Count;
                    int cnt = 0;

                    var vqueue = new Queue<VertexInfo>();
                    vqueue.Enqueue(vinfo);
                    while (vqueue.Count > 0)
                    {
                        vinfo = vqueue.Dequeue();

                        // データ作成
                        var data = new LineWorker.LineRotationData();
                        index = vinfo.vertexIndex;
                        data.vertexIndex = index;
                        //data.parentVertexIndex = -1;
                        data.localPos = float3.zero;
                        data.localRot = quaternion.identity;
                        if (vinfo.parentInfo != null)
                        {
                            // parent index
                            int pindex = vinfo.parentVertexIndex;
                            //data.parentVertexIndex = pindex;

                            // lpos, lrot
                            int mvindex = useVertexList[index];
                            int pmvindex = useVertexList[pindex];

                            Vector3 v = wposList[mvindex] - wposList[pmvindex];
                            //v.Normalize();
                            var pq = Quaternion.LookRotation(wnorList[pmvindex], wtanList[pmvindex]);
                            var ipq = Quaternion.Inverse(pq);
                            Vector3 lpos = ipq * v;

                            var q = Quaternion.LookRotation(wnorList[mvindex], wtanList[mvindex]);
                            Quaternion lrot = ipq * q;

                            data.localPos = lpos;
                            data.localRot = lrot;
                        }

                        if (vinfo.childInfoList.Count > 0)
                        {
                            data.childCount = vinfo.childInfoList.Count;
                            data.childStartDataIndex = startIndex + 1;
                        }
                        lineRotationData.Add(data);
                        cnt++;

                        startIndex += vinfo.childInfoList.Count;

                        // 子
                        foreach (var cinfo in vinfo.childInfoList)
                        {
                            vqueue.Enqueue(cinfo);
                        }
                    }

                    info.dataLength = (ushort)cnt;
                    lineRotationRootInfo.Add(info);
                }
            }
            if (lineRotationData.Count > 0)
            {
                this.lineRotationDataList = lineRotationData.ToArray();
                this.lineRotationRootInfoList = lineRotationRootInfo.ToArray();
            }

            // トライアングルベンド拘束
            var triangleBendData = new List<TriangleBendConstraint.TriangleBendData>();
            if (clothParams.UseTriangleBend)
            {
                Dictionary<uint, List<int>> triangleEdgeDict = MeshUtility.GetTriangleEdgePair(triangleList);
                foreach (var keyVal in triangleEdgeDict)
                {
                    // 辺の頂点分解
                    int vindex2, vindex3;
                    DataUtility.UnpackPair(keyVal.Key, out vindex2, out vindex3);
                    int v2 = useVertexList.IndexOf(vindex2);
                    int v3 = useVertexList.IndexOf(vindex3);
                    if (v2 < 0 || v3 < 0)
                        continue;

                    if (IsInvalidVertex(v2) || IsExtendVertex(v2))
                        continue;
                    if (IsInvalidVertex(v3) || IsExtendVertex(v3))
                        continue;
                    if ((IsFixedVertex(v2) || IsExtendVertex(v2)) && (IsFixedVertex(v3) || IsExtendVertex(v3)))
                        continue;

                    // 接続トライアングル
                    List<int> tlist = keyVal.Value;
                    for (int i = 0; i < (tlist.Count - 1); i++)
                    {
                        int tindex0 = tlist[i];
                        int vindex0 = RestTriangleVertex(tindex0, vindex2, vindex3, triangleList);

                        int v0 = useVertexList.IndexOf(vindex0);
                        if (v0 < 0)
                            continue;

                        if (IsInvalidVertex(v0) || IsExtendVertex(v0))
                            continue;

                        for (int j = i + 1; j < tlist.Count; j++)
                        {
                            int tindex1 = tlist[j];
                            int vindex1 = RestTriangleVertex(tindex1, vindex2, vindex3, triangleList);
                            int v1 = useVertexList.IndexOf(vindex1);

                            if (v1 < 0)
                                continue;

                            if (IsInvalidVertex(v1) || IsExtendVertex(v1))
                                continue;

                            // 登録
                            //   v2 +
                            //     /|\
                            // v0 + | + v1
                            //     \|/
                            //   v3 +
                            // v2-v3が接続辺

                            // 復元角度を求める
                            float restAngle;
                            if (CalcTriangleBendRestAngle(wposList[vindex0], wposList[vindex1], wposList[vindex2], wposList[vindex3], out restAngle))
                            {
                                var data = new TriangleBendConstraint.TriangleBendData();
                                data.vindex0 = v0;
                                data.vindex1 = v1;
                                data.vindex2 = v2;
                                data.vindex3 = v3;

                                data.restAngle = restAngle;
                                data.depth = (vertexDepthList[v2] + vertexDepthList[v3]) * 0.5f;

                                triangleBendData.Add(data);
                            }
                        }
                    }
                }
            }
            if (triangleBendData.Count > 0)
            {
                // ジョブシステム用にデータを加工して登録
                var builder = new ReferenceDataBuilder<TriangleBendConstraint.TriangleBendData>();
                builder.Init(useVertexList.Count);
                foreach (var data in triangleBendData)
                {
                    builder.AddData(data, data.vindex0, data.vindex1, data.vindex2, data.vindex3);
                }
                (var refDataList, var dataIndexList, var dataToDataIndexList) = builder.GetIndirectReferenceData();
                for (int i = 0; i < triangleBendData.Count; i++)
                {
                    var data = triangleBendData[i];
                    data.writeIndex0 = dataToDataIndexList[i][0];
                    data.writeIndex1 = dataToDataIndexList[i][1];
                    data.writeIndex2 = dataToDataIndexList[i][2];
                    data.writeIndex3 = dataToDataIndexList[i][3];
                    triangleBendData[i] = data;
                }
                this.triangleBendDataList = triangleBendData.ToArray();
                this.triangleBendReferenceList = refDataList.ToArray();
                this.triangleBendWriteBufferCount = dataIndexList.Count;
            }

#if false
            // エッジコリジョン拘束
            var edgeCollisionData = new List<EdgeCollisionConstraint.EdgeCollisionData>();
            if (clothParams.UseEdgeCollision)
            {
                // 両方移動頂点のエッジをデータ化する
                distSet.Clear();
                for (int i = 0; i < useVertexList.Count; i++)
                {
                    if (IsInvalidVertex(i) || IsExtendVertex(i))
                        continue;

                    int vindex0 = useVertexList[i];

                    var vlist = meshVLink[vindex0];
                    foreach (var vindex in vlist)
                    {
                        int vi = useVertexList.IndexOf(vindex);
                        if (vi < 0)
                            continue;

                        if (IsInvalidVertex(vi) || IsExtendVertex(vi))
                            continue;
                        if ((IsFixedVertex(i) || IsExtendVertex(i)) && (IsFixedVertex(vi) || IsExtendVertex(vi)))
                            continue;

                        uint pack = DataUtility.PackPair(i, vi);
                        if (distSet.Contains(pack))
                            continue;

                        // 登録
                        var data = new EdgeCollisionConstraint.EdgeCollisionData();
                        data.vindex0 = (ushort)i;
                        data.vindex1 = (ushort)vi;
                        edgeCollisionData.Add(data);

                        distSet.Add(pack);
                    }
                }
            }
            if (edgeCollisionData.Count > 0)
            {
                // ジョブシステム用にデータを加工して登録
                var builder = new ReferenceDataBuilder<EdgeCollisionConstraint.EdgeCollisionData>();
                builder.Init(useVertexList.Count);
                foreach (var data in edgeCollisionData)
                {
                    builder.AddData(data, data.vindex0, data.vindex1);
                }
                (var refDataList, var dataIndexList, var dataToDataIndexList) = builder.GetIndirectReferenceData();
                for (int i = 0; i < edgeCollisionData.Count; i++)
                {
                    var data = edgeCollisionData[i];
                    data.writeIndex0 = dataToDataIndexList[i][0];
                    data.writeIndex1 = dataToDataIndexList[i][1];
                    edgeCollisionData[i] = data;
                }
                this.edgeCollisionDataList = edgeCollisionData.ToArray();
                this.edgeCollisionReferenceList = refDataList.ToArray();
                this.edgeCollisionWriteBufferCount = dataIndexList.Count;
            }
#endif

#if false
            // ボリューム拘束
            var volumeData = new List<VolumeConstraint.VolumeData>();
            if (clothParams.UseVolume)
            {
                // テトラメッシュ構築（これはメッシュの全頂点から）
                int tetraCount;
                List<int> tetraIndexList;
                List<float> tetraSizeList;
                MeshUtility.CalcTetraMesh(wposList, out tetraCount, out tetraIndexList, out tetraSizeList);

                // テトラメッシュ選別（利用頂点のみ＋テトラサイズ）
                for (int i = 0; i < tetraCount; i++)
                {
                    if (tetraSizeList[i] > clothParams.GetMaxVolumeLength())
                        continue;

                    int index = i * 4;
                    int moveCnt = 0;
                    List<int> indexList = new List<int>();
                    for (int j = 0; j < 4; j++)
                    {
                        // 使用頂点インデックスに変換
                        int vi = useVertexList.IndexOf(tetraIndexList[index + j]);

                        // 使用頂点判定
                        if (vi < 0)
                        {
                            break;
                        }

                        // 移動頂点判定
                        if (IsMoveVertex(vi))
                            moveCnt++;
                        indexList.Add(vi);
                    }

                    // ４点がすべて使用され１つ以上の移動点を含む場合のみボリューム拘束を作成する
                    if (indexList.Count == 4 && moveCnt > 0)
                    {
                        var vindex0 = tetraIndexList[index];
                        var vindex1 = tetraIndexList[index + 1];
                        var vindex2 = tetraIndexList[index + 2];
                        var vindex3 = tetraIndexList[index + 3];

                        // 方向性拘束の作成
                        // テトラの４面のうちメッシュのトライアングルに含まれるもの、かつその中でデプスの平均値が最も小さいものを選出
                        List<int> dirList = SortTetra(vindex0, vindex1, vindex2, vindex3, meshVertexDepthList);
                        if (dirList != null)
                        {
                            // 並び替え
                            vindex0 = dirList[0];
                            vindex1 = dirList[1];
                            vindex2 = dirList[2];
                            vindex3 = dirList[3];
                        }

                        var v0 = useVertexList.IndexOf(vindex0);
                        var v1 = useVertexList.IndexOf(vindex1);
                        var v2 = useVertexList.IndexOf(vindex2);
                        var v3 = useVertexList.IndexOf(vindex3);

                        // ボリューム行列（データ作成時だけで大丈夫か？）
                        var ivMat = float3x3.identity;
                        float3x3 m;
                        var pos0 = wposList[vindex0];
                        var pos1 = wposList[vindex1];
                        var pos2 = wposList[vindex2];
                        var pos3 = wposList[vindex3];
                        m.c0 = pos1 - pos0;
                        m.c1 = pos2 - pos0;
                        m.c2 = pos3 - pos0;
                        float det = math.determinant(m);
                        if (math.abs(det) > 1e-6f)
                        {
                            // 方向性
                            int dir = 0;
                            if (dirList != null)
                            {
                                // 方向性
                                var n = math.normalize(math.cross(m.c0, m.c1));
                                dir = math.dot(n, m.c2) > 0.0f ? 1 : -1;
                            }

                            // 登録
                            if (dir != 0)
                            {
                                var data = new VolumeConstraint.VolumeData();
                                data.vindex0 = v0;
                                data.vindex1 = v1;
                                data.vindex2 = v2;
                                data.vindex3 = v3;
                                data.depth = (vertexDepthList[v0] + vertexDepthList[v1] + vertexDepthList[v2] + vertexDepthList[v3]) / 4.0f;
                                data.ivMat = math.inverse(m);
                                data.direction = dir;
                                volumeData.Add(data);
                            }
                        }
                    }
                }
            }
            if (volumeData.Count > 0)
            {
                // ジョブシステム用にデータを加工して登録
                var builder = new ReferenceDataBuilder<VolumeConstraint.VolumeData>();
                builder.Init(useVertexList.Count);
                foreach (var data in volumeData)
                {
                    builder.AddData(data, data.vindex0, data.vindex1, data.vindex2, data.vindex3);
                }
                (var refDataList, var dataIndexList, var dataToDataIndexList) = builder.GetIndirectReferenceData();
                for (int i = 0; i < volumeData.Count; i++)
                {
                    var data = volumeData[i];
                    data.writeIndex0 = dataToDataIndexList[i][0];
                    data.writeIndex1 = dataToDataIndexList[i][1];
                    data.writeIndex2 = dataToDataIndexList[i][2];
                    data.writeIndex3 = dataToDataIndexList[i][3];
                    volumeData[i] = data;
                }
                this.volumeDataList = volumeData.ToArray();
                this.volumeReferenceList = refDataList.ToArray();
                this.volumeWriteBufferCount = dataIndexList.Count;
            }
#endif
        }

        List<int> SortTetra(int v0, int v1, int v2, int v3, List<float> meshVertexDepthList)
        {
            List<int> result = new List<int>();
            result.Add(v0);
            result.Add(v1);
            result.Add(v2);
            result.Add(v3);

            // デプスの昇順にソート
            result.Sort((a, b) => meshVertexDepthList[a] < meshVertexDepthList[b] ? -1 : 1);

            return result;
        }

        /// <summary>
        /// テトラの４面に対してメッシュトライアングルに属する面を算出しデプスソートしたのち最もデプスが小さい面構成を返す
        /// 返された面の頂点0,1,2が基準面となり、3が方向性を示す頂点となる
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="trianglePackSet"></param>
        /// <param name="meshVertexDepthList"></param>
        /// <returns></returns>
        List<int> CheckTetraDirection(int v0, int v1, int v2, int v3, HashSet<ulong> trianglePackSet, List<float> meshVertexDepthList)
        {
            // メッシュに属する面のリストを摘出する
            ulong pack0 = DataUtility.PackTriple(v0, v1, v2);
            ulong pack1 = DataUtility.PackTriple(v0, v2, v3);
            ulong pack2 = DataUtility.PackTriple(v0, v3, v1);
            ulong pack3 = DataUtility.PackTriple(v1, v2, v3);

            List<ulong> packList = new List<ulong>();
            packList.Add(pack0);
            packList.Add(pack1);
            packList.Add(pack2);
            packList.Add(pack3);

            for (int i = 0; i < packList.Count;)
            {
                if (trianglePackSet.Contains(packList[i]) == false)
                {
                    packList.RemoveAt(i);
                    continue;
                }
                i++;
            }
            if (packList.Count == 0)
                return null; // なし

            // 面をデプス値の昇順でソートする
            // ソート後の最初の面が基準面となる
            if (packList.Count >= 2)
            {
                packList.Sort((a, b) =>
                {
                    int a0, a1, a2, b0, b1, b2;
                    DataUtility.UnpackTriple(a, out a0, out a1, out a2);
                    DataUtility.UnpackTriple(b, out b0, out b1, out b2);
                    float depth0 = (meshVertexDepthList[a0] + meshVertexDepthList[a1] + meshVertexDepthList[a2]) / 3.0f;
                    float depth1 = (meshVertexDepthList[b0] + meshVertexDepthList[b1] + meshVertexDepthList[b2]) / 3.0f;

                    //return depth0 < depth1 ? -1 : 1;
                    return depth0 > depth1 ? -1 : 1;
                });
            }

            // データ整形（メッシュトライアングルの頂点を0,1,2に格納、残りの１つを3に格納）
            List<int> restList = new List<int>();
            restList.Add(v0);
            restList.Add(v1);
            restList.Add(v2);
            restList.Add(v3);
            ulong pack = packList[0];
            DataUtility.UnpackTriple(pack, out v0, out v1, out v2);
            restList.Remove(v0);
            restList.Remove(v1);
            restList.Remove(v2);
            v3 = restList[0];
            if (meshVertexDepthList[v3] == 0.0f)
                return null;

            List<int> result = new List<int>();
            result.Add(v0);
            result.Add(v1);
            result.Add(v2);
            result.Add(v3);

            return result;
        }

        /// <summary>
        /// トライアングルベンドの復元角度を求める
        /// ２つの三角形、p2-p3が共通辺で、p0/p1が端の独立点
        ///   p2 +
        ///     /|\
        /// p0 + | + p1
        ///     \|/
        ///   p3 +
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns>トライアングル角度(ラジアン)</returns>
        bool CalcTriangleBendRestAngle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, out float restAngle)
        {
            restAngle = 0;

            Vector3 e = p3 - p2;
            float elen = e.magnitude;
            if (elen < 1e-6f)
                return false;

            Vector3 n1 = Vector3.Cross((p2 - p0), (p3 - p0));
            Vector3 n2 = Vector3.Cross((p3 - p1), (p2 - p1));

            // これいる？
            n1 /= n1.sqrMagnitude;
            n2 /= n2.sqrMagnitude;

            n1.Normalize();
            n2.Normalize();
            float dot = Vector3.Dot(n1, n2);

            if (dot < -1.0f)
                dot = -1.0f;
            if (dot > 1.0f)
                dot = 1.0f;

            // ただしこれでは折り曲げが左右どちらでも同じ角度で同じ値になってしまうが。。問題はないか？
            restAngle = Mathf.Acos(dot);

            return true;
        }

        /// <summary>
        /// ポリゴン番号とその２つの頂点を与え、残り１つの頂点番号を返す
        /// </summary>
        /// <param name="tindex"></param>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="triangleList"></param>
        /// <returns></returns>
        int RestTriangleVertex(int tindex, int v0, int v1, List<int> triangleList)
        {
            int index = tindex * 3;
            for (int i = 0; i < 3; i++)
            {
                int n = triangleList[index + i];
                if (n != v0 && n != v1)
                    return n;
            }

            return 0;
        }

        /// <summary>
        /// メッシュの頂点に対応するデプスリストを作成して返す
        /// </summary>
        /// <param name="vertexCount"></param>
        /// <param name="depthList"></param>
        /// <returns></returns>
        List<float> GetMeshVertexDepthList(int vertexCount, List<float> depthList)
        {
            var meshVertexDepthList = new List<float>();
            for (int i = 0; i < vertexCount; i++)
            {
                float depth = 0;
                int vindex = i;
                int vi = useVertexList.IndexOf(vindex);
                if (vi >= 0)
                {
                    depth = depthList[vi];
                }
                meshVertexDepthList.Add(depth);
            }

            return meshVertexDepthList;
        }

        /// <summary>
        /// 使用頂点ごとの親頂点をリスト化して返す
        /// 親が発見できない場合は(-1)となる。
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="vertexCount"></param>
        /// <param name="vlink">頂点が接続する他の頂点セット</param>
        /// <param name="wposList"></param>
        /// <param name="depthList"></param>
        /// <returns></returns>
        List<int> GetUseParentVertexList(int vertexCount, List<HashSet<int>> vlink, List<Vector3> wposList, List<float> depthList)
        {
            // アプローチ３
            var parentArray = new List<int>();
            List<int> sortIndexList = new List<int>();
            List<int> fixedIndexList = new List<int>();
            List<List<Vector3>> childPosList = new List<List<Vector3>>();
            for (int i = 0; i < vertexCount; i++)
            {
                parentArray.Add(-1);

                int index = useVertexList.IndexOf(i);
                if (index < 0)
                    continue;

                //Debug.Log("[" + i + "] vi:" + vi);
                sortIndexList.Add(index);

                if (IsFixedVertex(index))
                    fixedIndexList.Add(index);

                childPosList.Add(new List<Vector3>());
            }

            // 最も近い固定頂点の座標と距離を求める
            List<Vector3> nearFixedPosList = new List<Vector3>();
            List<float> nearFixedDistList = new List<float>();
            for (int i = 0; i < depthList.Count; i++)
            {
                Vector3 fpos = Vector3.zero;
                float fdist = 0;

                var vi = useVertexList[i];
                var pos = wposList[vi];
                float min = 10000;
                Vector3 dir = Vector3.zero;
                foreach (var fixindex in fixedIndexList)
                {
                    int fvi = useVertexList[fixindex];
                    var len = Vector3.Distance(pos, wposList[fvi]);
                    if (len < min)
                    {
                        min = len;
                        fpos = wposList[fvi];
                        fdist = len;
                    }
                }

                nearFixedPosList.Add(fpos);
                nearFixedDistList.Add(fdist);
            }

            if (fixedIndexList.Count > 0)
            {
                // デプスの降順でソート、同じ場合は最寄りの固定頂点距離の降順でソート
                sortIndexList.Sort((a, b) =>
                {
                    if (depthList[a] > depthList[b])
                        return -1;
                    if (depthList[a] < depthList[b])
                        return 1;

                    // 2nd
                    return nearFixedDistList[a] > nearFixedDistList[b] ? -1 : 1;
                });

                // デプスの降順で処理
                HashSet<int> compSet = new HashSet<int>();
                foreach (var index in sortIndexList)
                {
                    if (IsMoveVertex(index) == false)
                        continue;

                    int vi = useVertexList[index];
                    var pos = wposList[vi];

                    // 最も近い固定頂点への方向
                    Vector3 dir = (nearFixedPosList[index] - pos).normalized;

#if false
                    // 現在の接続子の方向
                    if (childPosList[index].Count > 0)
                    {
                        Vector3 cdir = Vector3.zero;
                        foreach (var cpos in childPosList[index])
                        {
                            cdir += (pos - cpos).normalized;
                        }
                        cdir.Normalize();

                        // 今までの子の方向割合を強めにする
                        cdir *= 2.0f; // 2.0f?

                        // 最も近い固定頂点方向と平均化する
                        dir = (dir + cdir).normalized;
                    }
#endif

                    // 接続点の中で最も固定点方向へ近いものを親としてつなぐ
                    var vset = vlink[vi];
                    int parent = -1;
                    int pindex = -1;
                    Vector3 ppos = Vector3.zero;
                    float maxdot = -10000;
                    foreach (var vindex in vset)
                    {
                        var lindex = useVertexList.IndexOf(vindex);
                        if (lindex < 0)
                            continue;
                        if (depthList[lindex] > depthList[index])
                            continue;

                        var linkpos = wposList[vindex];
                        Vector3 d = (linkpos - pos).normalized;

                        var dot = Vector3.Dot(dir, d);
                        if (dot > maxdot)
                        {
                            maxdot = dot;
                            parent = vindex;
                            pindex = lindex;
                            ppos = linkpos;
                        }
                    }

                    // 接続
                    if (pindex >= 0)
                    {
                        parentArray[vi] = parent;
                        childPosList[pindex].Add(pos);
                    }
                }
            }

            var parentList = new List<int>();
            foreach (var vindex in useVertexList)
                parentList.Add(useVertexList.IndexOf(parentArray[vindex]));

            // 最後に末端のパーティクルは親の親方向と合致する向きに切り替える
            //for (int i = 0; i < useVertexList.Count; i++)
            //{
            //    if (parentList.Contains(i) == false && selectionData[i] != SelectionData.Extend && IsMoveVertex(i))
            //    {
            //        // この頂点は末端
            //        Debug.Log("a:" + i);
            //    }
            //}

            return parentList;
        }

#if false
        /// <summary>
        /// 使用頂点ごとの親頂点をリスト化して返す
        /// 親が発見できない場合は(-1)となる。
        /// 親が複数ある場合は最も距離が近い親を選択する
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="vertexCount"></param>
        /// <param name="vlink">頂点が接続する他の頂点セット</param>
        /// <param name="wposList"></param>
        /// <param name="depthList"></param>
        /// <returns></returns>
        List<int> GetUseParentVertexList_Old(int vertexCount, List<HashSet<int>> vlink, List<Vector3> wposList, List<float> depthList)
        {
            // アプローチ２
            int[] parentArray = new int[vertexCount];
            int[] levelArray = new int[vertexCount];
            HashSet<int> useVertexSet = new HashSet<int>();
            List<int> restVertexList = new List<int>();
            for (int i = 0; i < vertexCount; i++)
            {
                restVertexList.Add(i);
                parentArray[i] = -1;
            }

            int level = 0;
            while (restVertexList.Count > 0)
            {
                int oldcnt = useVertexSet.Count;

                for (int i = 0; i < restVertexList.Count;)
                {
                    int vindex = restVertexList[i];
                    int vi = useVertexList.IndexOf(vindex);

                    if (level == 0)
                    {
                        // 最初に無効、固定頂点を登録する
                        if (vi < 0 || IsInvalidVertex(vi) || IsFixedVertex(vi) || IsExtendVertex(vi))
                        {
                            parentArray[vindex] = -1;
                            useVertexSet.Add(vindex);
                            restVertexList.RemoveAt(i);
                            levelArray[vindex] = level;
                            continue;
                        }
                    }
                    else if (level == 1)
                    {
                        // 固定頂点につながる移動頂点を登録
                        int parentIndex = -1;
                        float mindist = 10000;
                        var vlist = vlink[vindex];
                        foreach (var vindex2 in vlist)
                        {
                            int vi2 = useVertexList.IndexOf(vindex2);

                            if (vi2 < 0)
                                continue;

                            if (IsFixedVertex(vi2) == false)
                                continue;

                            // 対象への距離判定
                            var v1 = wposList[vindex2] - wposList[vindex];
                            var len = v1.magnitude;
                            if (len < mindist)
                            {
                                parentIndex = vindex2;
                                mindist = len;
                            }
                        }
                        if (parentIndex >= 0)
                        {
                            parentArray[vindex] = parentIndex;
                            useVertexSet.Add(vindex);
                            restVertexList.RemoveAt(i);
                            levelArray[vindex] = level;
                            continue;
                        }
                    }
                    else
                    {
                        // 最後に固定頂点に隣接する移動頂点を計算
                        int parentIndex = -1;
                        float minang = 360.0f;
                        var vlist = vlink[vindex];
                        foreach (var vindex2 in vlist)
                        {
                            int vi2 = useVertexList.IndexOf(vindex2);

                            if (vi2 < 0)
                                continue;

                            if (IsMoveVertex(vi2) == false)
                                continue;

                            if (useVertexSet.Contains(vindex2) == false)
                                continue;

                            //if (depthList[vindex2] >= depthList[vindex])
                            //    continue;

                            // レベル判定
                            if (levelArray[vindex2] >= level)
                                continue;

                            // 対象への方向と、その対象から親への方向を取得
                            var v1 = wposList[vindex2] - wposList[vindex];
                            var v2 = wposList[parentArray[vindex2]] - wposList[vindex2];

                            // 角度
                            var ang = Vector3.Angle(v1, v2);
                            if (ang < minang)
                            {
                                parentIndex = vindex2;
                                minang = ang;
                            }
                        }
                        if (parentIndex >= 0)
                        {
                            parentArray[vindex] = parentIndex;
                            useVertexSet.Add(vindex);
                            restVertexList.RemoveAt(i);
                            levelArray[vindex] = level;
                            continue;
                        }
                    }
                    i++;
                }
                level++;

                // 内容に一切変化がない場合は終了
                if (oldcnt == useVertexSet.Count)
                    break;
            }

            var parentList = new List<int>();
            foreach (var vindex in useVertexList)
                parentList.Add(useVertexList.IndexOf(parentArray[vindex]));
            return parentList;
        }
#endif

        /// <summary>
        /// 使用頂点ごとのルート頂点をリスト化して返す
        /// ルートが発見できない場合は(-1)となる。
        /// </summary>
        /// <param name="parentVertexList"></param>
        /// <returns></returns>
        List<int> GetUseRootVertexList(List<int> parentVertexList)
        {
            List<int> rootList = new List<int>();
            for (int i = 0; i < useVertexList.Count; i++)
            {
                int rootIndex = -1;
                int parentIndex = parentVertexList[i];
                while (parentIndex >= 0)
                {
                    rootIndex = parentIndex;
                    parentIndex = parentVertexList[parentIndex];
                }

                rootList.Add(rootIndex);
            }

            return rootList;
        }

        /// <summary>
        /// 頂点作業クラス
        /// </summary>
        private class VertexInfo
        {
            public int vertexIndex;
            public int parentVertexIndex = -1;
            public List<int> childVertexList = new List<int>();

            public VertexInfo parentInfo;
            public List<VertexInfo> childInfoList = new List<VertexInfo>();
        }

        /// <summary>
        /// 頂点作業情報の作成
        /// </summary>
        /// <param name="parentVertexList"></param>
        /// <returns></returns>
        List<VertexInfo> GetUseVertexInfoList(List<int> parentVertexList)
        {
            List<VertexInfo> infoList = new List<VertexInfo>();
            for (int i = 0; i < useVertexList.Count; i++)
            {
                var info = new VertexInfo();
                info.vertexIndex = i;
                infoList.Add(info);
            }

            for (int i = 0; i < useVertexList.Count; i++)
            {
                int parentIndex = parentVertexList[i];
                if (parentIndex >= 0)
                {
                    var info = infoList[i];
                    var pinfo = infoList[parentIndex];

                    info.parentVertexIndex = parentIndex;
                    info.parentInfo = pinfo;

                    pinfo.childVertexList.Add(i);
                    pinfo.childInfoList.Add(info);
                }
            }

            return infoList;
        }

        /// <summary>
        /// 使用頂点ベースのルートから続く一連の頂点リストを返す
        /// </summary>
        /// <param name="parentVertexList"></param>
        /// <returns></returns>
        List<List<int>> GetUseRootLineList(List<int> parentVertexList)
        {
            // 頂点情報作成
            var linkList = GetUseVertexInfoList(parentVertexList);

            // ルートごとのルートから続く一連のインデックスリストを構築
            List<List<int>> rootLine = new List<List<int>>();
            foreach (var link in linkList)
            {
                if (link.parentVertexIndex >= 0)
                    continue;

                // ただしルートから分岐する子は別ラインとして登録する
                for (int i = 0; i < link.childVertexList.Count; i++)
                {
                    List<int> indexList = new List<int>();
                    Queue<VertexInfo> vq = new Queue<VertexInfo>();
                    indexList.Add(link.vertexIndex);

                    vq.Enqueue(linkList[link.childVertexList[i]]);

                    while (vq.Count > 0)
                    {
                        var vdata = vq.Dequeue();
                        indexList.Add(vdata.vertexIndex);
                        foreach (var cindex in vdata.childVertexList)
                        {
                            vq.Enqueue(linkList[cindex]);
                        }
                    }

                    rootLine.Add(indexList);
                }
            }

            return rootLine;
        }
    }
}
