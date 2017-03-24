//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//

namespace CCAthlete
{
    public class CCWalk : MonoBehaviour
    {
        #region
        private CharacterController character;
        public float FrontBackWalkSpeed { get; set; }
        public float SideWalkSpeed { get; set; }
        #endregion


        private void Start()
        {
            character = GetComponent<CharacterController>();

            // Test
            FrontBackWalkSpeed = 6f;
            SideWalkSpeed = 6f;
        }


        private void FixedUpdate()
        {
            // テスト中 !!
            character.Move(
                transform.forward * Input.GetAxis("Vertical") * FrontBackWalkSpeed * 0.02f+
                transform.right * Input.GetAxis("Horizontal") * SideWalkSpeed * 0.02f
                );
        }
    }
}