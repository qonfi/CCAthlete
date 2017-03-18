//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//

namespace CCAthlete
{
    public class CCCameraAttacher : MonoBehaviour
    {
        #region
        [SerializeField]
        private GameObject face;
        public bool isFirstPersonAngle = false;
        #endregion


        private void Start()
        {
            AdjustCameraPosition(isFirstPersonAngle);
        }


        private void AdjustCameraPosition(bool firstPersonAngle)
        {
            Camera.main.transform.parent = face.transform;
            Camera.main.transform.localRotation = Quaternion.identity;

            if (firstPersonAngle)
            {
                Camera.main.transform.localPosition = new Vector3(0, 1.8f, 0.5f);
                return;
            }

            Camera.main.transform.localPosition = new Vector3(0, 2.4f, -4f);
            Camera.main.transform.Rotate(new Vector3(6f, 0, 0));
        }
    }
}