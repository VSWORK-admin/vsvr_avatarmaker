// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp

using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// BaseCloth API
    /// </summary>
    public abstract partial class BaseCloth : PhysicsTeam
    {
        /// <summary>
        /// クロスの物理シミュレーションをリセットします
        /// Reset cloth physics simulation.
        /// </summary>
        public void ResetCloth()
        {
            if (IsValid())
            {
                MagicaPhysicsManager.Instance.Team.SetFlag(teamId, PhysicsManagerTeamData.Flag_Reset_WorldInfluence, true);
                MagicaPhysicsManager.Instance.Team.SetFlag(teamId, PhysicsManagerTeamData.Flag_Reset_Position, true);
            }
        }

        /// <summary>
        /// タイムスケールを変更します
        /// Change the time scale.
        /// </summary>
        /// <param name="timeScale">0.0-1.0</param>
        public void SetTimeScale(float timeScale)
        {
            if (IsValid())
                MagicaPhysicsManager.Instance.Team.SetTimeScale(teamId, Mathf.Clamp01(timeScale));
        }

        /// <summary>
        /// タイムスケールを取得します
        /// Get the time scale.
        /// </summary>
        /// <returns></returns>
        public float GetTimeScale()
        {
            if (IsValid())
                return MagicaPhysicsManager.Instance.Team.GetTimeScale(teamId);
            else
                return 1.0f;
        }

        /// <summary>
        /// 外力を与えます
        /// Add external force.
        /// </summary>
        /// <param name="force"></param>
        public void AddForce(Vector3 force, PhysicsManagerTeamData.ForceMode mode)
        {
            if (IsValid() && IsActive())
                MagicaPhysicsManager.Instance.Team.SetImpactForce(teamId, force, mode);
        }

        /// <summary>
        /// 元の姿勢とシミュレーション結果とのブレンド率
        /// Blend ratio between original posture and simulation result.
        /// (0.0 = 0%, 1.0 = 100%)
        /// </summary>
        public float BlendWeight
        {
            get
            {
                return TeamData.UserBlendRatio;
            }
            set
            {
                TeamData.UserBlendRatio = value;
                UpdateBlend();
            }
        }

        //=========================================================================================
        // [Distance Disable] Parameters access.
        //=========================================================================================
        /// <summary>
        /// アクティブ設定
        /// Active settings.
        /// </summary>
        public bool DistanceDisable_Active
        {
            get
            {
                return clothParams.UseDistanceDisable;
            }
            set
            {
                clothParams.UseDistanceDisable = value;
            }
        }

        /// <summary>
        /// 距離計測の対象設定
        /// nullを指定するとメインカメラが参照されます。
        /// Target setting for distance measurement.
        /// If null is specified, the main camera is referred.
        /// </summary>
        public Transform DistanceDisable_ReferenceObject
        {
            get
            {
                return clothParams.DisableReferenceObject;
            }
            set
            {
                clothParams.DisableReferenceObject = value;
            }
        }

        /// <summary>
        /// シミュレーションを無効化する距離
        /// Distance to disable simulation.
        /// </summary>
        public float DistanceDisable_Distance
        {
            get
            {
                return clothParams.DisableDistance;
            }
            set
            {
                clothParams.DisableDistance = Mathf.Max(value, 0.0f);
            }
        }

        /// <summary>
        /// シミュレーションを無効化するフェード距離
        /// DistanceDisable_DistanceからDistanceDisable_FadeDistanceの距離を引いた位置からフェードが開始します。
        /// Fade distance to disable simulation.
        /// Fade from DistanceDisable_Distance minus DistanceDisable_FadeDistance distance.
        /// </summary>
        public float DistanceDisable_FadeDistance
        {
            get
            {
                return clothParams.DisableFadeDistance;
            }
            set
            {
                clothParams.DisableFadeDistance = Mathf.Max(value, 0.0f);
            }
        }

        //=========================================================================================
        // [External Force] Parameter access.
        //=========================================================================================
        /// <summary>
        /// パーティクル重量の影響率(0.0-1.0)
        /// Particle weight effect rate (0.0-1.0).
        /// </summary>
        public float ExternalForce_MassInfluence
        {
            get
            {
                return clothParams.MassInfluence;
            }
            set
            {
                clothParams.MassInfluence = value;
                MagicaPhysicsManager.Instance.Team.SetExternalForce(TeamId, clothParams.MassInfluence, clothParams.WindInfluence, clothParams.WindRandomScale);
            }
        }

        /// <summary>
        /// 風の影響率(1.0 = 100%)
        /// Wind influence rate (1.0 = 100%).
        /// </summary>
        public float ExternalForce_WindInfluence
        {
            get
            {
                return clothParams.WindInfluence;
            }
            set
            {
                clothParams.WindInfluence = value;
                MagicaPhysicsManager.Instance.Team.SetExternalForce(TeamId, clothParams.MassInfluence, clothParams.WindInfluence, clothParams.WindRandomScale);
            }
        }

        /// <summary>
        /// 風のランダム率(1.0 = 100%)
        /// Wind random rate (1.0 = 100%).
        /// </summary>
        public float ExternalForce_WindRandomScale
        {
            get
            {
                return clothParams.WindRandomScale;
            }
            set
            {
                clothParams.WindRandomScale = value;
                MagicaPhysicsManager.Instance.Team.SetExternalForce(TeamId, clothParams.MassInfluence, clothParams.WindInfluence, clothParams.WindRandomScale);
            }
        }
    }
}
