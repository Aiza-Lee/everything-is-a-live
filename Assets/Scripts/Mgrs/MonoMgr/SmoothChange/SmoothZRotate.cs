using UnityEngine;

namespace GameLogic {
	public class SmoothZRotate : SmoothChangeBase<float> {
		public override float GetCurVal() {
			return transform.localEulerAngles.z;
		}

		protected override void SetCurVal_Derived(float val) {
			var angles = transform.localEulerAngles;
			angles.z = val;
			transform.localEulerAngles = angles;
		}

		protected override float Add(float lhv, float rhv) => lhv + rhv;
		protected override float Mul(float lhv, float rhv) => lhv * rhv;
		protected override float Sub(float lhv, float rhv) => Mathf.DeltaAngle(rhv, lhv) + rhv;
	}
}