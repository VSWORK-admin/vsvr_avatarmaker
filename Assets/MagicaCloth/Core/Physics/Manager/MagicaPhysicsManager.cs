// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using System;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_2018 || UNITY_2019_1 || UNITY_2019_2)
using UnityEngine.Experimental.LowLevel;
#else
using UnityEngine.LowLevel;
#endif

namespace MagicaCloth
{
    /// <summary>
    /// MagicaCloth物理マネージャ
    /// </summary>
    [HelpURL("https://magicasoft.jp/magica-cloth-physics-manager/")]
    public partial class MagicaPhysicsManager : CreateSingleton<MagicaPhysicsManager>
    {
        /// <summary>
        /// 更新管理
        /// </summary>
        [SerializeField]
        UpdateTimeManager updateTime = new UpdateTimeManager();

        /// <summary>
        /// パーティクルデータ
        /// </summary>
        PhysicsManagerParticleData particle = new PhysicsManagerParticleData();

        /// <summary>
        /// トランスフォームデータ
        /// </summary>
        PhysicsManagerBoneData bone = new PhysicsManagerBoneData();

        /// <summary>
        /// メッシュデータ
        /// </summary>
        PhysicsManagerMeshData mesh = new PhysicsManagerMeshData();

        /// <summary>
        /// チームデータ
        /// </summary>
        PhysicsManagerTeamData team = new PhysicsManagerTeamData();

        /// <summary>
        /// 風データ
        /// </summary>
        PhysicsManagerWindData wind = new PhysicsManagerWindData();

        /// <summary>
        /// 物理計算処理
        /// </summary>
        PhysicsManagerCompute compute = new PhysicsManagerCompute();


        //=========================================================================================
        /// <summary>
        /// 遅延実行の有無
        /// ランタイムで変更できるようにバッファリング
        /// </summary>
        private bool useDelay = false;

        //=========================================================================================
        public UpdateTimeManager UpdateTime
        {
            get
            {
                return updateTime;
            }
        }

        public PhysicsManagerParticleData Particle
        {
            get
            {
                particle.SetParent(this);
                return particle;
            }
        }

        public PhysicsManagerBoneData Bone
        {
            get
            {
                bone.SetParent(this);
                return bone;
            }
        }

        public PhysicsManagerMeshData Mesh
        {
            get
            {
                mesh.SetParent(this);
                return mesh;
            }
        }

        public PhysicsManagerTeamData Team
        {
            get
            {
                team.SetParent(this);
                return team;
            }
        }

        public PhysicsManagerWindData Wind
        {
            get
            {
                wind.SetParent(this);
                return wind;
            }
        }

        public PhysicsManagerCompute Compute
        {
            get
            {
                compute.SetParent(this);
                return compute;
            }
        }

        public bool IsDelay
        {
            get
            {
                return useDelay;
            }
        }

        //=========================================================================================
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void InitSingleton()
        {
            Particle.Create();
            Bone.Create();
            Mesh.Create();
            Team.Create();
            Wind.Create();
            Compute.Create();
        }

        /// <summary>
        /// ２つ目の破棄されるマネージャの通知
        /// </summary>
        /// <param name="duplicate"></param>
        protected override void DuplicateDetection(MagicaPhysicsManager duplicate)
        {
            // 設定をコピーする
            UpdateMode = duplicate.UpdateMode;
            UpdatePerSeccond = duplicate.UpdatePerSeccond;
            FuturePredictionRate = duplicate.FuturePredictionRate;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        private void OnDestroy()
        {
            Compute.Dispose();
            Wind.Dispose();
            Team.Dispose();
            Mesh.Dispose();
            Bone.Dispose();
            Particle.Dispose();
        }

        //=========================================================================================
        /// <summary>
        /// Update()後の更新
        /// </summary>
        private void AfterUpdate()
        {
            //Debug.Log("After Update!" + Time.frameCount);

            // シミュレーションに必要なボーンの状態をもとに戻す
            Compute.InitJob();
            Compute.UpdateRestoreBone();

            Compute.CompleteJob();
        }

        /// <summary>
        /// LateUpdate()前の更新
        /// </summary>
        private void BeforeLateUpdate()
        {
            //Debug.Log("Before Late Update!" + Time.frameCount);
        }

        /// <summary>
        /// LateUpdate()後の更新
        /// </summary>
        private void AfterLateUpdate()
        {
            //Debug.Log("After Late Update!" + Time.frameCount);
            //Debug.Log("dtime:" + Time.deltaTime + " smooth:" + Time.smoothDeltaTime);

            // 遅延実行の切り替え判定
            if (useDelay != UpdateTime.IsDelay)
            {
                if (useDelay == false)
                {
                    // 結果の保持
                    Compute.UpdateSwapBuffer();
                    Compute.UpdateSyncBuffer();
                }
                useDelay = UpdateTime.IsDelay;
            }

            if (useDelay == false)
            {
                // 即時
                Compute.UpdateTeamAlways();
                Compute.InitJob();
                Compute.UpdateReadBone();
                Compute.UpdateStartSimulation(updateTime);
                Compute.UpdateWriteBone();
                Compute.UpdateCompleteSimulation();
                Compute.UpdateWriteMesh();
            }
        }

        /// <summary>
        /// PostLateUpdate.ScriptRunDelayedDynamicFrameRateの後
        /// LateUpdate()やアセットバンドルロード完了コールバックでクロスコンポーネントをインスタンス化すると、
        /// Start()が少し遅れてPostLateUpdateのScriptRunDelayedDynamicFrameRateで呼ばれることになる。
        /// 遅延実行時にこの処理が入ると、すでにクロスシミュレーションのジョブが開始されているため、
        /// Start()の初期化処理などでNativeリストにアクセスすると例外が発生してしまう。
        /// 従って遅延実行時はクロスコンポーネントのStart()が完了するScriptRunDelayedDynamicFrameRate
        /// の後にシミュレーションを開始するようにする。(v1.5.1)
        /// </summary>
        private void PostLateUpdate()
        {
            //Debug.Log("Post Late Update!" + Time.frameCount);
            if (useDelay)
            {
                // 遅延実行
                Compute.UpdateTeamAlways();
                Compute.InitJob();
                Compute.UpdateReadWriteBone();
                Compute.UpdateStartSimulation(updateTime);
                Compute.ScheduleJob();
                Compute.UpdateWriteMesh();
            }
        }

        /// <summary>
        /// レンダリング完了後の更新
        /// </summary>
        private void AfterRendering()
        {
            //Debug.Log("After Rendering Update!" + Time.frameCount);
            if (useDelay)
            {
                // 遅延実行
                // シミュレーション終了待機
                Compute.UpdateCompleteSimulation();
                // 結果の保持
                Compute.UpdateSwapBuffer();
                Compute.UpdateSyncBuffer();
            }
        }

        //=========================================================================================
        /// <summary>
        /// カスタム更新ループ登録
        /// </summary>
        [RuntimeInitializeOnLoadMethod()]
        static void InitCustomGameLoop()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetDefaultPlayerLoop();

#if false
            // debug
            foreach (var header in playerLoop.subSystemList)
            {
                Debug.LogFormat("------{0}------", header.type.Name);
                foreach (var subSystem in header.subSystemList)
                {
                    Debug.LogFormat("{0}.{1}", header.type.Name, subSystem.type.Name);
                }
            }
#endif

            PlayerLoopSystem afterUpdate = new PlayerLoopSystem()
            {
                type = typeof(MagicaPhysicsManager),
                updateDelegate = () =>
                {
                    if (IsInstance())
                    {
                        Instance.AfterUpdate();
                    }
                }
            };

            PlayerLoopSystem beforeLateUpdate = new PlayerLoopSystem()
            {
                type = typeof(MagicaPhysicsManager),
                updateDelegate = () =>
                {
                    if (IsInstance())
                    {
                        Instance.BeforeLateUpdate();
                    }
                }
            };

            PlayerLoopSystem afterLateUpdate = new PlayerLoopSystem()
            {
                type = typeof(MagicaPhysicsManager),
                updateDelegate = () =>
                {
                    if (IsInstance())
                    {
                        Instance.AfterLateUpdate();
                    }
                }
            };

            PlayerLoopSystem postLateUpdate = new PlayerLoopSystem()
            {
                type = typeof(MagicaPhysicsManager),
                updateDelegate = () =>
                {
                    if (IsInstance())
                    {
                        Instance.PostLateUpdate();
                    }
                }
            };

            PlayerLoopSystem afterRendering = new PlayerLoopSystem()
            {
                type = typeof(MagicaPhysicsManager),
                updateDelegate = () =>
                {
                    if (IsInstance())
                    {
                        Instance.AfterRendering();
                    }
                }
            };


            // update
            int sysIndex = Array.FindIndex(playerLoop.subSystemList, (s) => s.type.Name == "Update");
            PlayerLoopSystem updateSystem = playerLoop.subSystemList[sysIndex];
            var updateSubsystemList = new List<PlayerLoopSystem>(updateSystem.subSystemList);
            int index = updateSubsystemList.FindIndex(h => h.type.Name.Contains("ScriptRunBehaviourUpdate"));
            updateSubsystemList.Insert(index + 1, afterUpdate); // Update() after
            updateSystem.subSystemList = updateSubsystemList.ToArray();
            playerLoop.subSystemList[sysIndex] = updateSystem;

            // late update
            sysIndex = Array.FindIndex(playerLoop.subSystemList, (s) => s.type.Name == "PreLateUpdate");
            PlayerLoopSystem lateUpdateSystem = playerLoop.subSystemList[sysIndex];
            var lateUpdateSubsystemList = new List<PlayerLoopSystem>(lateUpdateSystem.subSystemList);
            index = lateUpdateSubsystemList.FindIndex(h => h.type.Name.Contains("ScriptRunBehaviourLateUpdate"));
            lateUpdateSubsystemList.Insert(index, beforeLateUpdate); // LateUpdate() before
            lateUpdateSubsystemList.Insert(index + 2, afterLateUpdate); // LateUpdate() after
            lateUpdateSystem.subSystemList = lateUpdateSubsystemList.ToArray();
            playerLoop.subSystemList[sysIndex] = lateUpdateSystem;

            // post late update
            sysIndex = Array.FindIndex(playerLoop.subSystemList, (s) => s.type.Name == "PostLateUpdate");
            PlayerLoopSystem postLateUpdateSystem = playerLoop.subSystemList[sysIndex];
            var postLateUpdateSubsystemList = new List<PlayerLoopSystem>(postLateUpdateSystem.subSystemList);
            index = postLateUpdateSubsystemList.FindIndex(h => h.type.Name.Contains("ScriptRunDelayedDynamicFrameRate"));
            postLateUpdateSubsystemList.Insert(index + 1, postLateUpdate); // postLateUpdate()
            postLateUpdateSystem.subSystemList = postLateUpdateSubsystemList.ToArray();
            playerLoop.subSystemList[sysIndex] = postLateUpdateSystem;

            // rendering
            sysIndex = Array.FindIndex(playerLoop.subSystemList, (s) => s.type.Name == "PostLateUpdate");
            PlayerLoopSystem postLateSystem = playerLoop.subSystemList[sysIndex];
            var postLateSubsystemList = new List<PlayerLoopSystem>(postLateSystem.subSystemList);
            index = postLateSubsystemList.FindIndex(h => h.type.Name.Contains("FinishFrameRendering"));
            postLateSubsystemList.Insert(index + 1, afterRendering); // rendering after
            postLateSystem.subSystemList = postLateSubsystemList.ToArray();
            playerLoop.subSystemList[sysIndex] = postLateSystem;

            PlayerLoop.SetPlayerLoop(playerLoop);
        }
    }
}
