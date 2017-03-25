//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
using UnityEngine;

namespace CCAthlete
{
    /// <summary>
    /// <para></para> CharacterController にジャンプ機能を与えるコンポーネント。
    /// <para></para> Depends on... 
    /// <para></para> Is depended on... 
    /// </summary>
    public class CCJump : MonoBehaviour
    {
        public bool Jumping { get; set; }
        public float UpwardPower { get; set; }
        private CharacterController character;
        private LandingChecker landingChecker;


        private void Start()
        {
            Jumping = false;
            UpwardPower = 0.16f;
            character = GetComponent<CharacterController>();
            
            landingChecker = new LandingChecker(GetComponent<GroundDetectorSphere>(), this);
        }



        private void Update()
        {
            if (Input.GetButton("Jump")) { Jumping = true; }
        }


        private void FixedUpdate()
        {
            if (Jumping == false) { return; }

            character.Move(character.transform.up * UpwardPower);

            landingChecker.Update();
        }

        


        private class LandingChecker
        {
            public bool HasTakenOff { get; private set; }
            private GroundDetectorSphere detector;
            private CCJump ccJump;


            public LandingChecker(GroundDetectorSphere _detector, CCJump _ccJump)
            {
                this.detector = _detector;
                this.ccJump = _ccJump;
            }


            public void Update()
            {
                // ジャンプ後に接地判定がfalse になったら離陸後ということにする。
                if ( HasTakenOff == false && 
                    detector.OnGround == false)
                {
                    HasTakenOff = true;
                }

                // 離陸後に接地判定が再びtrue となったらジャンプ終了。
                if ( HasTakenOff && detector.OnGround)
                {
                    FinishJump();
                }
            }
            

            public void FinishJump()
            {
                HasTakenOff = false;
                ccJump.Jumping = false;
            }
        }
    }
}