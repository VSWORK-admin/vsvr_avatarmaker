// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// 各チームのデータ
    /// </summary>
    [System.Serializable]
    public class PhysicsTeamData : IDataHash
    {
        // チーム固有のコライダーリスト
        [SerializeField]
        private List<ColliderComponent> colliderList = new List<ColliderComponent>();

        /// <summary>
        /// ユーザー設定ブレンド率
        /// </summary>
        [SerializeField]
        private float userBlendRatio = 1.0f;

        //=========================================================================================
        /// <summary>
        /// データハッシュを求める
        /// </summary>
        /// <returns></returns>
        public int GetDataHash()
        {
            return colliderList.GetDataHash();
        }

        //=========================================================================================
        public void Init(int teamId)
        {
            // コライダーをチームに参加させる
            foreach (var collider in colliderList)
            {
                if (collider)
                {
                    collider.CreateColliderParticle(teamId);
                }
            }
        }

        public void Dispose(int teamId)
        {
            if (MagicaPhysicsManager.IsInstance())
            {
                // コライダーをチームから除外する
                foreach (var collider in colliderList)
                {
                    if (collider)
                    {
                        collider.RemoveColliderParticle(teamId);
                    }
                }
            }
        }

        //=========================================================================================
        public int ColliderCount
        {
            get
            {
                return colliderList.Count;
            }
        }

        public List<ColliderComponent> ColliderList
        {
            get
            {
                return colliderList;
            }
        }

        public float UserBlendRatio
        {
            get
            {
                return userBlendRatio;
            }
            set
            {
                userBlendRatio = value;
            }
        }

        //=========================================================================================
        /// <summary>
        /// コライダーリスト検証
        /// </summary>
        public void ValidateColliderList()
        {
            // コライダーのnullや重複を削除する
            ShareDataObject.RemoveNullAndDuplication(colliderList);
        }
    }
}
