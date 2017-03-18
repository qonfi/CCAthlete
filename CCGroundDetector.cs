//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//


namespace CCAthlete
{
    public class CCGroundDetector : MonoBehaviour
    {
        #region
        public bool OnGround { get; private set; }
        public float OffsetToFoot = -1f;
        private int layerMask;
        private Vector3 footingPosition;
        private Vector3 detectionBoxHalf;
        #endregion



        private void Start()
        {
            OnGround = false;
            footingPosition = new Vector3(0, 0, 0);
            detectionBoxHalf = new Vector3(0.2f, 0.1f, 0.2f);

            Debug.Log("<color=navy>" + "The gameObject OnGroundDetector is attached to is in  [" + LayerMask.LayerToName(this.gameObject.layer) + "]  layer." + "\n" + "And OnGroundDetector ignores this layer. " + "</color>");

            // 1 << gameObject.layer で自分と同一のレイヤー以外を無視、
            // ~でビットをすべて反転(?) つまり自分と同一のレイヤーのみ無視
            layerMask = ~(1 << this.gameObject.layer);
        }



        private void Update()
        {
            // 毎フレームチェックを行うのは負荷が高そう。
            // transform.position が変化している間だけとかにする...?
            footingPosition = transform.position + Vector3.up * OffsetToFoot;
            OnGround = Detect(footingPosition);

            //Debug.Log(OnGround);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">衝突検知範囲の中心です。</param>
        private bool Detect(Vector3 position)
        {
            // LayerMask で判定するレイヤーを決める。
            // QueryTriggerInteraction.Ignore で、トリガーとして設定されているCollision は対象としない設定になる。
            return Physics.CheckBox(
                        position,
                        detectionBoxHalf,
                        Quaternion.identity,
                        layerMask,
                        QueryTriggerInteraction.Ignore
                        );
        }
    }
}