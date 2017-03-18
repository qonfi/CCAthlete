//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//

namespace CCAthlete
{
    public class CCLookAround : MonoBehaviour
    {
        #region
        public float HorizontalSensitivity { get; private set; }
        public float VerticalSensitivity { get; private set; }
        private float horizontalRotation;
        private float verticalRotation;

        [SerializeField]        private GameObject face;
        private LookAroundLimitter limitter;
        #endregion


        private void Start()
        {
            HorizontalSensitivity = 5f;
            VerticalSensitivity = 5f;

            limitter = new LookAroundLimitter(70f, 70f);
        }


        private void Update()
        {
            horizontalRotation = Input.GetAxis("Mouse X") * HorizontalSensitivity;
            verticalRotation = Input.GetAxis("Mouse Y") * VerticalSensitivity * -1;
            
            verticalRotation = limitter.Filtering(face.transform, verticalRotation);

        }



        private void FixedUpdate()
        {
            AntiTiltRotate(gameObject, horizontalRotation, 0);
            AntiTiltRotate(face, 0, verticalRotation);
        }





        private class LookAroundLimitter
        {
            private float upwardLimitEuler;
            private float downwardLimitEuler;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="upwardLimit">Front is zero, just above is 90</param>
            /// <param name="downwardLimit">Front is zero, just below is 90</param>
            public LookAroundLimitter(float upwardLimit = 70f, float downwardLimit = 70f)
            {
                #region eulerAngles の数値について
                //  x 軸の回転は transform.eulerAngles を見ると、
                // 正面をゼロとして、プラスが下方向となる。
                // 90が真下、180が真後ろを逆立ちして見ている状態、240が真上、360はゼロ...つまり正面。
                // なので少し上を向くと320などとなる。
                #endregion
                upwardLimitEuler = 360f - upwardLimit;
                downwardLimitEuler = downwardLimit;
            }

            public float Filtering(Transform rotatingObject, float verticalRotation)
            {
                float currentAngle = rotatingObject.eulerAngles.x;
                float filteredRotation = verticalRotation;

                if (currentAngle < upwardLimitEuler &&
                    currentAngle > 180f &&
                    verticalRotation < 0)
                {
                    filteredRotation = 0;
                }

                if (currentAngle > downwardLimitEuler &&
                    currentAngle < 180f &&
                    verticalRotation > 0)
                {
                    filteredRotation = 0;
                }

                return filteredRotation;
            }
        }


        /// <summary>
        /// 上下左右の回転のみで、Z軸の(ドアノブをひねるような)回転はしないRotate。
        /// </summary>
        /// <param name="objectToRotate"></param>
        /// <param name="horizontalRotation"></param>
        /// <param name="verticalRotation"></param>
        private static void AntiTiltRotate(GameObject objectToRotate, float horizontalRotation, float verticalRotation)
        {
            // 回転がローカル軸に対してか、ワールド軸に対してか指定することができる。
            // Y軸方向の回転はワールド軸でないと、あちこち回転しているうちにオブジェクトが傾いてしまう。
            objectToRotate.transform.Rotate(0, horizontalRotation, 0, Space.World);
            objectToRotate.transform.Rotate(verticalRotation, 0, 0);
        }

    }
}