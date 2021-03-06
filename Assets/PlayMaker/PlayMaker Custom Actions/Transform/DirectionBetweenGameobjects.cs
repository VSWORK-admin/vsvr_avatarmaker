// Custom Action by DumbGameDev
// www.dumbgamedev.com
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Transform)]
    [Tooltip("Calculate the direction between two gameobjects with the option to normalize. This is also known as the heading.")]

	public class DirectionBetweenGameobjects : FsmStateAction
	{
        [RequiredField]
		[Title ("Source")]
		[Tooltip("Source Gameobject.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Title ("Target")]
        [Tooltip("Target Gameobject.")]
		public FsmGameObject target;

		[UIHint(UIHint.Variable)]
		[Title ("Direction")]
		[Tooltip("Direction between gameobjects.")]
		public FsmVector3 direction;

		public FsmBool everyFrame;

		[ActionSection ("Normalize")]

		[Tooltip("Normaize the direction between these two gameobjects using distance. Only enable when needed as this does an extra step of calculation.")]
		public FsmBool normalize;

		[UIHint(UIHint.Variable)]
		[Title ("Distance")]
		[Tooltip("Distance between objects is only calculated when normalize is toggled.")]
		public FsmFloat distance;

		[UIHint(UIHint.Variable)]
		[Title ("Direction Normalized")]
		[Tooltip("Direction between gameobjects normalized. Also known as heading")]
		public FsmVector3 directionNormalized;

		private GameObject _target;
		private GameObject _player;
		private Vector3 _direction;

        public override void Reset()
		{

			target = null;
			direction = null;
			distance = null;
			directionNormalized = null;
			normalize = false;
			gameObject = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
            CornersCalculation();

            if (!everyFrame.Value)
			{
				Finish();
			}
		}

        public override void OnUpdate()
		{
                CornersCalculation();
		}

		void CornersCalculation()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			_target = target.Value;
			_player = go;

			if (!normalize.Value)
            {
				direction.Value = _target.transform.position - _player.transform.position;
			}

			if (normalize.Value)
            {
				// Do normal direction calculations
				direction.Value = _target.transform.position - _player.transform.position;
				_direction = direction.Value;

				//Distance is calculated by the direction magnitude
				distance.Value = _direction.magnitude;

				//Normalized direction
				directionNormalized.Value = direction.Value / distance.Value;
			}       
        }
	}
}