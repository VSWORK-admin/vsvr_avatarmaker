// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MagicaCloth
{
    /// <summary>
    /// ライン回転調整ワーカー
    /// </summary>
    public class LineWorker : PhysicsManagerWorker
    {
        /// <summary>
        /// データ
        /// </summary>
        [System.Serializable]
        public struct LineRotationData
        {
            /// <summary>
            /// 頂点インデックス
            /// </summary>
            public int vertexIndex;

            /// <summary>
            /// 親頂点インデックス
            /// </summary>
            //public int parentVertexIndex;

            /// <summary>
            /// 子頂点の数
            /// </summary>
            public int childCount;

            /// <summary>
            /// 子頂点の開始データ配列インデックス
            /// </summary>
            public int childStartDataIndex;

            /// <summary>
            /// ローカル位置（単位ベクトル）
            /// </summary>
            public float3 localPos;

            /// <summary>
            /// ローカル回転
            /// </summary>
            public quaternion localRot;

            /// <summary>
            /// データが有効か判定する
            /// </summary>
            /// <returns></returns>
            //public bool IsValid()
            //{
            //    return vertexIndex != 0 || parentVertexIndex != 0;
            //}
        }
        FixedChunkNativeArray<LineRotationData> dataList;

        [System.Serializable]
        public struct LineRotationRootInfo
        {
            public ushort startIndex;
            public ushort dataLength;
        }
        FixedChunkNativeArray<LineRotationRootInfo> rootInfoList;

        /// <summary>
        /// グループごとの拘束データ
        /// </summary>
        public struct GroupData
        {
            public int teamId;

            /// <summary>
            /// 回転補間
            /// </summary>
            public int avarage;

            public ChunkData dataChunk;
            public ChunkData rootInfoChunk;
        }
        public FixedNativeList<GroupData> groupList;

        /// <summary>
        /// ルートごとのチームインデックス
        /// </summary>
        FixedChunkNativeArray<int> rootTeamList;

        //=========================================================================================
        public override void Create()
        {
            dataList = new FixedChunkNativeArray<LineRotationData>();
            rootInfoList = new FixedChunkNativeArray<LineRotationRootInfo>();
            groupList = new FixedNativeList<GroupData>();
            rootTeamList = new FixedChunkNativeArray<int>();
        }

        public override void Release()
        {
            dataList.Dispose();
            rootInfoList.Dispose();
            groupList.Dispose();
            rootTeamList.Dispose();
        }

        public int AddGroup(
            int teamId,
            bool avarage,
            LineRotationData[] dataArray,
            LineRotationRootInfo[] rootInfoArray
            )
        {
            if (dataArray == null || dataArray.Length == 0 || rootInfoArray == null || rootInfoArray.Length == 0)
                return -1;

            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];

            var gdata = new GroupData();
            gdata.teamId = teamId;
            gdata.avarage = avarage ? 1 : 0;
            gdata.dataChunk = dataList.AddChunk(dataArray.Length);
            gdata.rootInfoChunk = rootInfoList.AddChunk(rootInfoArray.Length);

            // チャンクデータコピー
            dataList.ToJobArray().CopyFromFast(gdata.dataChunk.startIndex, dataArray);
            rootInfoList.ToJobArray().CopyFromFast(gdata.rootInfoChunk.startIndex, rootInfoArray);

            int group = groupList.Add(gdata);

            // ルートごとのチームインデックス
            var c = rootTeamList.AddChunk(rootInfoArray.Length);
            rootTeamList.Fill(c, teamId);

            return group;
        }

        public override void RemoveGroup(int teamId)
        {
            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];
            int group = teamData.lineWorkerGroupIndex;
            if (group < 0)
                return;

            var cdata = groupList[group];

            // チャンクデータ削除
            dataList.RemoveChunk(cdata.dataChunk);
            rootInfoList.RemoveChunk(cdata.rootInfoChunk);
            rootTeamList.RemoveChunk(cdata.rootInfoChunk);

            // データ削除
            groupList.Remove(group);
        }

        public void ChangeParam(int teamId, bool avarage)
        {
            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];
            int group = teamData.lineWorkerGroupIndex;
            if (group < 0)
                return;

            var gdata = groupList[group];
            gdata.avarage = avarage ? 1 : 0;
            groupList[group] = gdata;
        }

        //=========================================================================================
        /// <summary>
        /// トランスフォームリード中に実行する処理
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public override void Warmup()
        {
        }

        //=========================================================================================
        /// <summary>
        /// 物理更新前処理
        /// </summary>
        /// <param name="jobHandle"></param>
        /// <returns></returns>
        public override JobHandle PreUpdate(JobHandle jobHandle)
        {
            return jobHandle;
        }

        //=========================================================================================
        /// <summary>
        /// 物理更新後処理
        /// </summary>
        /// <param name="jobHandle"></param>
        /// <returns></returns>
        public override JobHandle PostUpdate(JobHandle jobHandle)
        {
            if (groupList.Count == 0)
                return jobHandle;

            // ラインの回転調整（ルートラインごと）
            var job1 = new LineRotationJob()
            {
                dataList = dataList.ToJobArray(),
                rootInfoList = rootInfoList.ToJobArray(),
                rootTeamList = rootTeamList.ToJobArray(),
                groupList = groupList.ToJobArray(),

                teamDataList = Manager.Team.teamDataList.ToJobArray(),

                flagList = Manager.Particle.flagList.ToJobArray(),

                posList = Manager.Particle.posList.ToJobArray(),
                rotList = Manager.Particle.rotList.ToJobArray(),
            };
            jobHandle = job1.Schedule(rootTeamList.Length, 8, jobHandle);

            return jobHandle;
        }

        /// <summary>
        /// ラインの回転調整
        /// </summary>
        [BurstCompile]
        struct LineRotationJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<LineRotationData> dataList;
            [ReadOnly]
            public NativeArray<LineRotationRootInfo> rootInfoList;
            [ReadOnly]
            public NativeArray<int> rootTeamList;
            [ReadOnly]
            public NativeArray<GroupData> groupList;

            // チーム
            [ReadOnly]
            public NativeArray<PhysicsManagerTeamData.TeamData> teamDataList;

            [ReadOnly]
            public NativeArray<PhysicsManagerParticleData.ParticleFlag> flagList;

            [ReadOnly]
            public NativeArray<float3> posList;

            [NativeDisableParallelForRestriction]
            public NativeArray<quaternion> rotList;

            // ルートラインごと
            public void Execute(int rootIndex)
            {
                // チーム
                int teamIndex = rootTeamList[rootIndex];
                if (teamIndex == 0)
                    return;
                var team = teamDataList[teamIndex];
                if (team.IsActive() == false || team.lineWorkerGroupIndex < 0)
                    return;

                // グループデータ
                var gdata = groupList[team.lineWorkerGroupIndex];

                // データ
                var rootInfo = rootInfoList[rootIndex];
                int dstart = gdata.dataChunk.startIndex;
                int dataIndex = rootInfo.startIndex + dstart;
                int dataCount = rootInfo.dataLength;
                int pstart = team.particleChunk.startIndex;

                if (dataCount <= 1)
                    return;

                for (int i = 0; i < dataCount; i++)
                {
                    var data = dataList[dataIndex + i];

                    var pindex = data.vertexIndex;
                    pindex += pstart;

                    var flag = flagList[pindex];
                    if (flag.IsValid() == false)
                        continue;

                    // 自身の現在姿勢
                    var pos = posList[pindex];
                    var rot = rotList[pindex];

                    // 子の回転調整
                    if (data.childCount > 0)
                    {
                        // 子への平均ベクトル
                        float3 ctv = 0;
                        float3 cv = 0;

                        for (int j = 0; j < data.childCount; j++)
                        {
                            var cdata = dataList[data.childStartDataIndex + dstart + j];
                            int cindex = cdata.vertexIndex + pstart;

                            // 子の現在姿勢
                            var cpos = posList[cindex];

                            // 子の本来のベクトル
                            float3 tv = math.normalize(math.mul(rot, cdata.localPos));
                            ctv += tv;

                            // 子の現在ベクトル
                            float3 v = math.normalize(cpos - pos);
                            cv += v;

                            // 回転
                            var q = MathUtility.FromToRotation(tv, v);

                            // 子の姿勢確定
                            var crot = math.mul(rot, cdata.localRot);
                            crot = math.mul(q, crot);
                            rotList[cindex] = crot;
                        }

                        // 固定は回転させない判定(v1.5.2)
                        if (team.IsFlag(PhysicsManagerTeamData.Flag_FixedNonRotation) && flag.IsKinematic())
                            continue;

                        // 子の移動方向変化に伴う回転調整
                        var cq = MathUtility.FromToRotation(ctv, cv);

                        // 回転補間
                        if (gdata.avarage == 1)
                        {
                            cq = math.slerp(quaternion.identity, cq, 0.5f); // 50%
                        }

                        rot = math.mul(cq, rot);
                        rotList[pindex] = math.normalize(rot); // 正規化しないとエラーになる場合がある
                    }
                }
            }
        }

#if false
        /// <summary>
        /// ライン回転調整ジョブ
        /// </summary>
        [BurstCompile]
        struct LineRotationJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<LineRotationData> dataList;
            [ReadOnly]
            public NativeArray<GroupData> groupList;
            [ReadOnly]
            public NativeArray<ReferenceDataIndex> refDataList;

            // チーム
            [ReadOnly]
            public NativeArray<PhysicsManagerTeamData.TeamData> teamDataList;
            [ReadOnly]
            public NativeArray<int> teamIdList;

            [ReadOnly]
            public NativeArray<quaternion> baseRotList;
            [ReadOnly]
            public NativeArray<float3> posList;

            [WriteOnly]
            public NativeArray<quaternion> rotList;

            // パーティクルごと
            public void Execute(int index)
            {
                // チーム
                int teamIndex = teamIdList[index];
                var team = teamDataList[teamIndex];
                if (team.IsActive() == false || team.lineWorkerGroupIndex < 0)
                    return;

                // 更新確認
                if (team.IsUpdate() == false)
                    return;

                int pstart = team.particleChunk.startIndex;
                int vindex = index - pstart;
                if (vindex < 0)
                    return;

                // データ
                var gdata = groupList[team.lineWorkerGroupIndex];
                //if (gdata.active == false)
                //    return;

                // 参照情報
                var refdata = refDataList[gdata.refChunk.startIndex + vindex];
                if (refdata.count > 0)
                {
                    quaternion baserot = baseRotList[index]; // 常に本来の回転から算出する
                    var nextrot = baserot;
                    var nextpos = posList[index];

                    float3 addpos = 0;
                    float t = 1.0f / refdata.count;

                    int dataIndex = gdata.dataChunk.startIndex + refdata.startIndex;
                    for (int i = 0; i < refdata.count; i++, dataIndex++)
                    {
                        var data = dataList[dataIndex];

                        if (data.IsValid() == false)
                            continue;


                        float3 tvec = data.localPos;

                        // 回転ラインベース
                        float3 v2 = new float3(0, 0, 1);
                        float3 tv = v2;

                        if (data.targetIndex < 0)
                        {
                            // 親がターゲット
                            int tindex = pstart + -data.targetIndex - 1; // さらに(-1)する
                            float3 ppos = posList[tindex];

                            // 現在のベクトル
                            v2 = nextpos - ppos;

                            // 本来あるべきベクトル
                            tv = math.mul(baseRotList[tindex], tvec);
                        }
                        else
                        {
                            // 子がターゲット
                            int tindex = pstart + data.targetIndex;
                            float3 cpos = posList[tindex];

                            // 現在のベクトル
                            v2 = cpos - nextpos;

                            // 本来あるべきベクトル
                            tv = math.mul(baserot, tvec);
                        }

                        // 補正回転
                        // ターゲットが複数ある場合は均等に回転補間を行う
                        quaternion q = MathUtility.FromToRotation(tv, v2);
                        quaternion rot = math.slerp(quaternion.identity, q, t);

                        // 最終回転
                        nextrot = math.mul(rot, nextrot);
                    }

                    // 書き出し
                    nextrot = math.normalize(nextrot); // 正規化しないとエラーになる場合がある
                    rotList[index] = nextrot;
                }
            }
        }
#endif
    }
}
