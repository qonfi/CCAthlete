//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//

namespace CCAthlete
{
    

    /// <summary>
    /// CharacterController.SimpleMove という関数を使えば重力を適用してくれるらしいのでこれはいらなかったかも。
    /// </summary>
    public class CCGravitySimulator : MonoBehaviour
    {
        #region
        CCGroundDetector detector;
        CharacterController character;
        public float FloatingTime { get; set; }
        public float FallAmount { get; set; }
        private float gravityAccel = 0.2f;
        #endregion


        private void Start()
        {
            detector = GetComponent<CCGroundDetector>();
            character = GetComponent<CharacterController>();
        }


        private void Update()
        {
            CalcGravity();
            FallCC();
        }


        public void CalcGravity()
        {
            if (detector.OnGround)
            {
                FloatingTime = 0;
                FallAmount = 0;
                return;
            }

            FloatingTime += Time.deltaTime;
            FallAmount = FloatingTime * gravityAccel;
        }


        public void ResetFalling()
        {
            FloatingTime = 0;
            FallAmount = 0;
        }


        private void FallCC()
        {
            character.Move(character.transform.up * -FallAmount);
        }
    }

    
}