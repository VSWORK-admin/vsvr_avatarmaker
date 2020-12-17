// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// クロス基本パラメータ
    /// </summary>
    [System.Serializable]
    public class ClothParams
    {
        // パーティクルサイズ
        [SerializeField]
        private BezierParam radius = new BezierParam(0.02f, 0.02f, true, 0.0f, false);

        // パーティクルの重さ
        [SerializeField]
        private BezierParam mass = new BezierParam(1.0f, 1.0f, true, 0.0f, false);

        // パーティクル重力加速度(m/s)
        [SerializeField]
        private bool useGravity = true;
        [SerializeField]
        private BezierParam gravity = new BezierParam(-9.8f, -9.8f, false, 0.0f, false);
        [SerializeField]
        private bool useDirectionalDamping = true;
        [SerializeField]
        private Transform directionalDampingObject = null;
        [SerializeField]
        private BezierParam directionalDamping = new BezierParam(1.0f, 0.1f, true, -0.5f, true);

        // パーティクル空気抵抗値
        [SerializeField]
        private bool useDrag = true;
        [SerializeField]
        private BezierParam drag = new BezierParam(0.02f, 0.02f, true, 0.0f, false);

        //  パーティクル最大速度(m/s)
        [SerializeField]
        private bool useMaxVelocity = true;
        [SerializeField]
        private BezierParam maxVelocity = new BezierParam(3.0f, 3.0f, false, 0.0f, false);

        // ワールド移動影響
        [SerializeField]
        private Transform influenceTarget = null;
        [SerializeField]
        private BezierParam worldMoveInfluence = new BezierParam(0.5f, 0.5f, false, 0.0f, false);
        [SerializeField]
        private BezierParam worldRotationInfluence = new BezierParam(0.5f, 0.5f, false, 0.0f, false);

        // 外力
        [SerializeField]
        private float massInfluence = 0.3f;
        [SerializeField]
        private float windInfluence = 1.0f;
        [SerializeField]
        private float windRandomScale = 0.7f;

        // 距離無効化
        [SerializeField]
        private bool useDistanceDisable = false;
        [SerializeField]
        private Transform disableReferenceObject = null;
        [SerializeField]
        private float disableDistance = 20.0f;
        [SerializeField]
        private float disableFadeDistance = 5.0f;

        // テレポート
        [SerializeField]
        private bool useResetTeleport = false;
        [SerializeField]
        private float teleportDistance = 0.2f;
        [SerializeField]
        private float teleportRotation = 45.0f;

        // ルートからの最小最大距離拘束
        [SerializeField]
        private bool useClampDistanceRatio = true;
        [SerializeField]
        private float clampDistanceMinRatio = 0.7f;
        [SerializeField]
        private float clampDistanceMaxRatio = 1.1f;
        [SerializeField]
        private float clampDistanceVelocityInfluence = 0.2f;

        // 原点からの移動範囲拘束
        [SerializeField]
        private bool useClampPositionLength = false;
        [SerializeField]
        private BezierParam clampPositionLength = new BezierParam(0.03f, 0.2f, true, 0.0f, false);
        [SerializeField]
        private float clampPositionRatioX = 1.0f;
        [SerializeField]
        private float clampPositionRatioY = 1.0f;
        [SerializeField]
        private float clampPositionRatioZ = 1.0f;
        [SerializeField]
        private float clampPositionVelocityInfluence = 0.2f;

        // 最大回転角度拘束
        [SerializeField]
        private bool useClampRotation = false;
        [SerializeField]
        private BezierParam clampRotationAngle = new BezierParam(30.0f, 30.0f, true, 0.0f, false);
        //[SerializeField]
        //private BezierParam clampRotationStiffness = new BezierParam(0.1f, 0.1f, true, 0.0f, false);
        [SerializeField]
        private float clampRotationVelocityInfluence = 0.2f;

        // 距離復元拘束
        [SerializeField]
        private float restoreDistanceVelocityInfluence = 1.0f;
        [SerializeField]
        private BezierParam structDistanceStiffness = new BezierParam(1.0f, 1.0f, false, 0.0f, false);
        [SerializeField]
        private bool useBendDistance = false;
        [SerializeField]
        private int bendDistanceMaxCount = 2;
        [SerializeField]
        private BezierParam bendDistanceStiffness = new BezierParam(0.5f, 0.5f, false, 0.0f, false);
        [SerializeField]
        private bool useNearDistance = false;
        [SerializeField]
        private int nearDistanceMaxCount = 3;
        [SerializeField]
        private float nearDistanceMaxDepth = 1.0f;
        [SerializeField]
        private BezierParam nearDistanceLength = new BezierParam(0.1f, 0.1f, true, 0.0f, false);
        [SerializeField]
        private BezierParam nearDistanceStiffness = new BezierParam(0.3f, 0.3f, false, 0.0f, false);

        // 回転復元拘束
        [SerializeField]
        private bool useRestoreRotation = false;
        [SerializeField]
        private BezierParam restoreRotation = new BezierParam(0.3f, 0.1f, true, 0.0f, false);
        [SerializeField]
        private float restoreRotationVelocityInfluence = 0.2f;

        // スプリング拘束
        [SerializeField]
        private bool useSpring = false;
        [SerializeField]
        private float springPower = 0.017f;
        [SerializeField]
        private float springRadius = 0.1f;
        [SerializeField]
        private float springScaleX = 1;
        [SerializeField]
        private float springScaleY = 1;
        [SerializeField]
        private float springScaleZ = 1;
        [SerializeField]
        private float springIntensity = 1.0f;
        [SerializeField]
        private BezierParam springDirectionAtten = new BezierParam(1.0f, 0.0f, true, 0.234f, true);
        [SerializeField]
        private BezierParam springDistanceAtten = new BezierParam(1.0f, 0.0f, true, 0.395f, true);

        // スプリング回転調整拘束
        [SerializeField]
        private bool useAdjustRotation = false;
        public enum AdjustMode
        {
            None = 0,
            XYMove = 1,
            XZMove = 2,
            YZMove = 3,
        }
        [SerializeField]
        private AdjustMode adjustMode;
        [SerializeField]
        private float adjustRotationPower = 5.0f;

        // トライアングル曲げ拘束
        [SerializeField]
        private bool useTriangleBend = false;
        [SerializeField]
        private BezierParam triangleBend = new BezierParam(0.5f, 0.5f, true, 0.0f, false);

        // ボリューム拘束
        [SerializeField]
        private bool useVolume = false;
        [SerializeField]
        private float maxVolumeLength = 0.1f;
        [SerializeField]
        private BezierParam volumeStretchStiffness = new BezierParam(0.5f, 0.5f, true, 0.0f, false);
        [SerializeField]
        private BezierParam volumeShearStiffness = new BezierParam(0.5f, 0.5f, true, 0.0f, false);

        // コライダーコリジョン拘束
        [SerializeField]
        private bool useCollision = false;
        [SerializeField]
        private float friction = 0.2f;
        //[SerializeField]
        //private bool useEdgeCollision = false;
        [SerializeField]
        private bool keepInitialShape = false;
        //[SerializeField]
        //private float frictionDampingRate = 0.6f;

        // 回転補間
        [SerializeField]
        private bool useLineAvarageRotation = true;
        [SerializeField]
        private bool useFixedNonRotation = false;

        // コライダー方向移動制限拘束
        //[SerializeField]
        //private bool useDirectionMoveLimit = false;
        //[SerializeField]
        //private BezierParam directionMoveLimit = new BezierParam(0.03f, 0.03f, true, 0.0f, false);

        // セルフコリジョン拘束
        //[SerializeField]
        //private bool useSelfCollision = false;
        //[SerializeField]
        //private float selfCollisionInfluenceRange = 0.2f;
        //[SerializeField]
        //private int maxSelfCollisionCount = 6;
        //[SerializeField]
        //private BezierParam selfCollisionStiffness = new BezierParam(0.3f, 0.01f, true, 0.0f, false);
        //[SerializeField]
        //private BezierParam selfCollisionThickness = new BezierParam(0.01f, 0.01f, true, 0.0f, false);


        //=========================================================================================
        // 変更チェック用
        public enum ParamType
        {
            Radius,
            Mass,
            Gravity,
            Drag,
            MaxVelocity,
            WorldInfluence,
            ClampDistance,
            ClampPosition,
            ClampRotation,
            RestoreDistance,
            RestoreRotation,
            Spring,
            AdjustRotation,
            AirLine,
            TriangleBend,
            Volume,
            ColliderCollision,
            RotationInterpolation,
            DistanceDisable,
            ExternalForce,
            //DirectionMoveLimit,
            //SelfCollision,
        }

        // 変更記録セット
        private HashSet<ParamType> changeSet = new HashSet<ParamType>();

        public void SetChangeParam(ParamType ptype)
        {
            changeSet.Add(ptype);
        }

        public bool ChangedParam(ParamType ptype)
        {
            return changeSet.Contains(ptype);
        }

        public void ClearChangeParam()
        {
            changeSet.Clear();
        }

        //=========================================================================================
        // radius
        public void SetRadius(float sval, float eval)
        {
            radius.SetParam(sval, eval);
        }

        public float GetRadius(float depth)
        {
            return radius.Evaluate(depth);
        }

        // mass
        public void SetMass(float sval, float eval, bool useEval = true, float cval = 0.0f, bool useCval = false)
        {
            mass.SetParam(sval, eval, useEval, cval, useCval);
        }

        public BezierParam GetMass()
        {
            return mass;
        }

        // gravity
        public void SetGravity(bool sw, float sval = -9.8f, float eval = -9.8f)
        {
            useGravity = sw;
            gravity.SetParam(sval, eval);
        }

        public bool UseGravity
        {
            get
            {
                return useGravity;
            }
        }

        public BezierParam GetGravity()
        {
            if (useGravity)
                return gravity;
            else
                return new BezierParam(0.0f);
        }

        public void SetDirectionalDamping(bool sw, float sval = 1.0f, float eval = 0.1f, float curve = -0.5f, Transform target = null)
        {
            useDirectionalDamping = sw;
            directionalDamping.SetParam(sval, eval, true, curve, true);
            directionalDampingObject = target;
        }

        public bool UseDirectionalDamping
        {
            get
            {
                return useDirectionalDamping;
            }
        }

        public Transform DirectionalDampingObject
        {
            get
            {
                return directionalDampingObject;
            }
            set
            {
                directionalDampingObject = value;
            }
        }

        public BezierParam GetDirectionalDamping()
        {
            if (useGravity)
                return directionalDamping;
            else
                return new BezierParam(0.0f);
        }



        // drag
        public void SetDrag(bool sw, float sval = 0.015f, float eval = 0.015f)
        {
            useDrag = sw;
            drag.SetParam(sval, eval);
        }

        public bool UseDrag
        {
            get
            {
                return useDrag;
            }
        }

        public BezierParam GetDrag()
        {
            if (useDrag)
                return drag;
            else
                return new BezierParam(0);
        }

        // max velocity
        public void SetMaxVelocity(bool sw, float sval = 3, float eval = 3)
        {
            useMaxVelocity = sw;
            maxVelocity.SetParam(sval, eval);
        }

        public bool UseMaxVelocity
        {
            get
            {
                return useMaxVelocity;
            }
        }

        public BezierParam GetMaxVelocity()
        {
            if (useMaxVelocity)
                return maxVelocity;
            else
                return new BezierParam(1000);
        }

        // external force
        public void SetExternalForce(float massInfluence, float windInfluence, float windRandomScale)
        {
            this.massInfluence = massInfluence;
            this.windInfluence = windInfluence;
            this.windRandomScale = windRandomScale;
        }

        public float MassInfluence
        {
            get
            {
                return massInfluence;
            }
            set
            {
                massInfluence = value;
            }
        }

        public float WindInfluence
        {
            get
            {
                return windInfluence;
            }
            set
            {
                windInfluence = value;
            }
        }

        public float WindRandomScale
        {
            get
            {
                return windRandomScale;
            }
            set
            {
                windRandomScale = value;
            }
        }

        // world move/rot influence
        public void SetWorldInfluence(float moveval, float rotval)
        {
            worldMoveInfluence.SetParam(moveval, moveval, false);
            worldRotationInfluence.SetParam(rotval, rotval, false);
        }

        public BezierParam GetWorldMoveInfluence()
        {
            return worldMoveInfluence;
        }

        public BezierParam GetWorldRotationInfluence()
        {
            return worldRotationInfluence;
        }

        public Transform GetInfluenceTarget()
        {
            return influenceTarget;
        }

        public void SetInfluenceTarget(Transform t)
        {
            influenceTarget = t;
        }

        // reset teleport
        public void SetTeleport(bool sw, float distance = 0.2f, float rotation = 45.0f)
        {
            useResetTeleport = sw;
            teleportDistance = distance;
            teleportRotation = rotation;
        }

        public bool UseResetTeleport
        {
            get
            {
                return useResetTeleport;
            }
        }

        public float TeleportDistance
        {
            get
            {
                return useResetTeleport ? teleportDistance : 100000.0f;
            }
        }

        public float TeleportRotation
        {
            get
            {
                return useResetTeleport ? teleportRotation : 360.0f;
            }
        }

        // disable distance
        public void SetDistanceDisable(bool sw, float distance = 20.0f, float fadeDistance = 5.0f, Transform referenceObject = null)
        {
            useDistanceDisable = sw;
            disableReferenceObject = referenceObject;
            disableDistance = distance;
            disableFadeDistance = fadeDistance;
        }

        public bool UseDistanceDisable
        {
            get
            {
                return useDistanceDisable;
            }
            set
            {
                useDistanceDisable = value;
            }
        }

        public Transform DisableReferenceObject
        {
            get
            {
                return disableReferenceObject;
            }
            set
            {
                disableReferenceObject = value;
            }
        }

        public float DisableDistance
        {
            get
            {
                return disableDistance;
            }
            set
            {
                disableDistance = value;
            }
        }

        public float DisableFadeDistance
        {
            get
            {
                return disableFadeDistance;
            }
            set
            {
                disableFadeDistance = value;
            }
        }

        // clamp distance
        public void SetClampDistanceRatio(bool sw, float minval = 0.1f, float maxval = 1.05f, float influence = 0.2f)
        {
            useClampDistanceRatio = sw;
            clampDistanceMinRatio = minval;
            clampDistanceMaxRatio = maxval;
            clampDistanceVelocityInfluence = influence;
        }

        public bool UseClampDistanceRatio
        {
            get
            {
                return useClampDistanceRatio;
            }
        }

        public float ClampDistanceMinRatio
        {
            get
            {
                return useClampDistanceRatio ? clampDistanceMinRatio : 0;
            }
        }

        public float ClampDistanceMaxRatio
        {
            get
            {
                return useClampDistanceRatio ? clampDistanceMaxRatio : 0;
            }
        }

        public float ClampDistanceVelocityInfluence
        {
            get
            {
                return useClampDistanceRatio ? clampDistanceVelocityInfluence : 1;
            }
        }

        // clamp position
        public void SetClampPositionLength(bool sw, float sval = 0.03f, float eval = 0.2f, float ratioX = 1, float ratioY = 1, float ratioZ = 1, float influence = 0.2f)
        {
            useClampPositionLength = sw;
            clampPositionLength.SetParam(sval, eval);
            clampPositionRatioX = ratioX;
            clampPositionRatioY = ratioY;
            clampPositionRatioZ = ratioZ;
            clampPositionVelocityInfluence = influence;
        }

        public bool UseClampPositionLength
        {
            get
            {
                return useClampPositionLength;
            }
        }

        public Vector3 ClampPositionAxisRatio
        {
            get
            {
                return new Vector3(clampPositionRatioX, clampPositionRatioY, clampPositionRatioZ);
            }
        }

        public BezierParam GetClampPositionLength()
        {
            return clampPositionLength;
        }

        public float ClampPositionVelocityInfluence
        {
            get
            {
                return useClampPositionLength ? clampPositionVelocityInfluence : 1;
            }
        }

        // clamp rotation
        public void SetClampRotationAngle(bool sw, float sval = 30.0f, float eval = 30.0f, float influence = 0.2f)
        {
            useClampRotation = sw;
            clampRotationAngle.SetParam(sval, eval);
            clampRotationVelocityInfluence = influence;
        }

        public bool UseClampRotation
        {
            get
            {
                return useClampRotation;
            }
        }

        public BezierParam GetClampRotationAngle()
        {
            return clampRotationAngle;
        }

        //public BezierParam GetClampRotationStiffness()
        //{
        //    return clampRotationStiffness;
        //}

        public float ClampRotationVelocityInfluence
        {
            get
            {
                return useClampRotation ? clampRotationVelocityInfluence : 1;
            }
        }

        // restore distance
        public void SetRestoreDistance(float influence = 1.0f, float structStiffness = 1.0f)
        {
            restoreDistanceVelocityInfluence = influence;
            structDistanceStiffness.SetParam(structStiffness, structStiffness, false);
        }

        public float RestoreDistanceVelocityInfluence
        {
            get
            {
                return restoreDistanceVelocityInfluence;
            }
        }

        public BezierParam GetStructDistanceStiffness()
        {
            return structDistanceStiffness;
        }

        public bool UseBendDistance
        {
            get
            {
                return useBendDistance;
            }
        }

        public int BendDistanceMaxCount
        {
            get
            {
                return bendDistanceMaxCount;
            }
        }

        public BezierParam GetBendDistanceStiffness()
        {
            return bendDistanceStiffness;
        }

        public bool UseNearDistance
        {
            get
            {
                return useNearDistance;
            }
        }

        public int NearDistanceMaxCount
        {
            get
            {
                return nearDistanceMaxCount;
            }
        }

        public float NearDistanceMaxDepth
        {
            get
            {
                return nearDistanceMaxDepth;
            }
        }

        public BezierParam GetNearDistanceLength()
        {
            return nearDistanceLength;
        }

        public BezierParam GetNearDistanceStiffness()
        {
            return nearDistanceStiffness;
        }

        // restore rotation
        public void SetRestoreRotation(bool sw, float sval = 0.02f, float eval = 0.001f, float influence = 0.3f)
        {
            useRestoreRotation = sw;
            restoreRotation.SetParam(sval, eval);
            restoreRotationVelocityInfluence = influence;
        }

        public bool UseRestoreRotation
        {
            get
            {
                return useRestoreRotation;
            }
        }

        public BezierParam GetRotationPower()
        {
            return restoreRotation;
        }

        public float RestoreRotationVelocityInfluence
        {
            get
            {
                return useRestoreRotation ? restoreRotationVelocityInfluence : 1;
            }
        }

        // spring
        public void SetSpring(bool sw, float power = 0.0f, float r = 0.0f, float sclx = 1, float scly = 1, float sclz = 1, float intensity = 1)
        {
            useSpring = sw;
            springPower = power;
            springRadius = r;
            springScaleX = sclx;
            springScaleY = scly;
            springScaleZ = sclz;
            springIntensity = intensity;
        }

        public void SetSpringDirectionAtten(float sval, float eval, float cval)
        {
            springDirectionAtten.SetParam(sval, eval, true, cval, true);
        }

        public void SetSpringDistanceAtten(float sval, float eval, float cval)
        {
            springDistanceAtten.SetParam(sval, eval, true, cval, true);
        }

        public bool UseSpring
        {
            get
            {
                return useSpring;
            }
        }

        public float GetSpringPower()
        {
            if (useSpring)
                return springPower;
            else
                return 0;
        }

        public float SpringRadius
        {
            get
            {
                return springRadius;
            }
        }

        public Vector3 SpringRadiusScale
        {
            get
            {
                return new Vector3(springScaleX, springScaleY, springScaleZ);
            }
        }

        public float SpringIntensity
        {
            get
            {
                return springIntensity;
            }
        }

        public float GetSpringDirectionAtten(float ratio)
        {
            return springDirectionAtten.Evaluate(ratio);
        }

        public float GetSpringDistanceAtten(float ratio)
        {
            return springDistanceAtten.Evaluate(ratio);
        }

        // adjust spring rotation
        public void SetAdjustRotation(bool sw, AdjustMode amode = AdjustMode.None, float power = 0.0f)
        {
            useAdjustRotation = sw;
            adjustMode = amode;
            adjustRotationPower = power;
        }

        public bool UseAdjustRotation
        {
            get
            {
                return useAdjustRotation;
            }
        }

        public AdjustMode AdjustRotationMode
        {
            get
            {
                return adjustMode;
            }
        }

        public Vector3 AdjustRotationVector
        {
            get
            {
                if (useAdjustRotation)
                {
                    // 移動軸調整の場合は、各軸の回転力が入る
                    Vector3 vec = Vector3.one;
                    vec *= adjustRotationPower;
                    return vec;
                }
                else
                    return Vector3.zero;
            }
        }

        // triangle bend
        public void SetTriangleBend(bool sw, float sval = 0.03f, float eval = 0.03f)
        {
            useTriangleBend = sw;
            triangleBend.SetParam(sval, eval);
        }

        public bool UseTriangleBend
        {
            get
            {
                return useTriangleBend;
            }
        }

        public float GetTriangleBendPower(float depth)
        {
            if (useTriangleBend)
                return triangleBend.Evaluate(depth);
            else
                return 0;
        }

        public BezierParam GetTriangleBendStiffness()
        {
            return triangleBend;
        }

        // volume
        public void SetVolume(bool sw, float maxLength = 0.05f, float stiffness = 0.5f, float shear = 0.5f)
        {
            useVolume = sw;
            maxVolumeLength = maxLength;
            volumeShearStiffness.SetParam(stiffness, stiffness, false);
            volumeShearStiffness.SetParam(shear, shear, false);
        }

        public bool UseVolume
        {
            get
            {
                return useVolume;
            }
        }

        public float GetMaxVolumeLength()
        {
            if (useVolume)
            {
                return maxVolumeLength;
            }
            else
                return 0;
        }

        public BezierParam GetVolumeStretchStiffness()
        {
            return volumeStretchStiffness;
        }

        public BezierParam GetVolumeShearStiffness()
        {
            return volumeShearStiffness;
        }

        // collider collision
        public void SetCollision(bool sw, float friction = 0.2f)
        {
            useCollision = sw;
            this.friction = friction;
        }

        public bool UseCollision
        {
            get
            {
                return useCollision;
            }
        }

        public float Friction
        {
            get
            {
                return friction;
            }
        }

        //public bool UseEdgeCollision
        //{
        //    get
        //    {
        //        return useEdgeCollision;
        //    }
        //}

        public bool KeepInitialShape
        {
            get
            {
                return keepInitialShape;
            }
        }

        // rotation interpolation
        public bool UseLineAvarageRotation
        {
            get
            {
                return useLineAvarageRotation;
            }
        }

        public bool UseFixedNonRotation
        {
            get
            {
                return useFixedNonRotation;
            }
        }

        // self collision
        //public void SetSelfCollision(bool sw)
        //{
        //    useSelfCollision = sw;
        //}

        //public bool UseSelfCollision
        //{
        //    get
        //    {
        //        return useSelfCollision;
        //    }
        //}

        //public float SelfCollisionINfluenceRange
        //{
        //    get
        //    {
        //        return selfCollisionInfluenceRange;
        //    }
        //}

        //public int MaxSelfCollisionCount
        //{
        //    get
        //    {
        //        return maxSelfCollisionCount;
        //    }
        //}

        //public float GetSelfCollisionStiffness(float depth)
        //{
        //    if (useSelfCollision)
        //        return selfCollisionStiffness.Evaluate(depth);
        //    else
        //        return 0.0f;
        //}

        //public float GetSelfCollisionThickness(float depth)
        //{
        //    if (useSelfCollision)
        //        return selfCollisionThickness.Evaluate(depth);
        //    else
        //        return 0.0f;
        //}

        // direction move limit
        //public void SetDirectionMoveLimit(bool sw, float sval = 0.05f, float eval = 0.05f)
        //{
        //    useDirectionMoveLimit = sw;
        //    directionMoveLimit.SetParam(sval, eval);
        //}

        //public bool UseDirectionMoveLimit
        //{
        //    get
        //    {
        //        return useDirectionMoveLimit;
        //    }
        //}

        //public float GetDirectionMoveLength(float depth)
        //{
        //    if (useDirectionMoveLimit)
        //        return directionMoveLimit.Evaluate(depth);
        //    else
        //        return -1; // 無効
        //}
    }
}
