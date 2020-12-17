﻿// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// 球コライダー
    /// </summary>
    [HelpURL("https://magicasoft.jp/magica-cloth-sphere-collider/")]
    [AddComponentMenu("MagicaCloth/MagicaSphereCollider")]
    public class MagicaSphereCollider : ColliderComponent
    {
        [SerializeField]
        [Range(0.001f, 0.5f)]
        private float radius = 0.05f;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                // 半径変更
                foreach (var c in particleDict.Values)
                    for (int i = 0; i < c.dataLength; i++)
                        MagicaPhysicsManager.Instance.Particle.SetRadius(c.startIndex + i, radius);
            }
        }

        //=========================================================================================
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        /// <summary>
        /// データハッシュ計算
        /// </summary>
        /// <returns></returns>
        public override int GetDataHash()
        {
            int hash = base.GetDataHash();
            hash += radius.GetDataHash();
            return hash;
        }

        protected override ChunkData CreateColliderParticleReal(int teamId)
        {
            uint flag = 0;
            flag |= PhysicsManagerParticleData.Flag_Kinematic;
            flag |= PhysicsManagerParticleData.Flag_Collider;
            flag |= PhysicsManagerParticleData.Flag_Transform_Read_Base;
            flag |= PhysicsManagerParticleData.Flag_Step_Update;
            flag |= PhysicsManagerParticleData.Flag_Reset_Position;
            //flag |= PhysicsManagerParticleData.Flag_Transform_Read_Scl; // 現在スケールは見ていない

            var c = CreateParticle(
                flag,
                teamId, // team
                0.0f, // depth
                radius,
                Vector3.zero
                );

            MagicaPhysicsManager.Instance.Team.AddCollider(teamId, c.startIndex);

            return c;
        }

        /// <summary>
        /// スケール値を取得
        /// </summary>
        /// <returns></returns>
        public float GetScale()
        {
            // X軸のみを見る
            return transform.lossyScale.x;
        }


        /// <summary>
        /// 指定座標に最も近い衝突点pと、中心軸からのpへの方向dirを返す。
        /// ※エディタ計算用
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="p"></param>
        /// <param name="dir"></param>
        public override bool CalcNearPoint(Vector3 pos, out Vector3 p, out Vector3 dir, out Vector3 d)
        {
            dir = Vector3.zero;

            d = transform.position;
            var v = pos - d;
            float vlen = v.magnitude;
            if (vlen <= Radius)
            {
                // 衝突している
                p = pos;
                if (vlen > 0.0f)
                    dir = v.normalized;
                return true;
            }
            else
            {
                dir = v.normalized;
                p = d + dir * Radius;
                return false;
            }
        }
    }
}