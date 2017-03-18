//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//

namespace CCAthlete
{
    public class GroundDetectorSphere : MonoBehaviour
    {
        #region
        public bool OnGround { get; private set; }
        private float radius = 0.5f;
        private float rayLength = 0.8f;
        
        public GameObject LastDetectedObject { get; private set; }
        private SphereCaster sphereCaster;
        private DetectionEconomizer economizer;
        #endregion



        private void Start()
        {
            OnGround = false;
            
            // 1 << gameObject.layer で自分と同一のレイヤー以外を無視、
            // ~でビットをすべて反転(?) つまり自分と同一のレイヤーのみ無視
            int layerMask = ~(1 << this.gameObject.layer);

            economizer = new DetectionEconomizer();
            sphereCaster = new SphereCaster(layerMask);

            Debug.Log("<color=blue>" + "The gameObject GroundDetectorSphere is attached to is in  [" + LayerMask.LayerToName(this.gameObject.layer) + "]  layer." + "\n" + "And OnGroundDetector ignores this layer. " + "</color>");
        }




        private void Update()
        {
            if (economizer.NeedsDetecting(this.transform) == false) { return; }

            OnGround = sphereCaster.Cast(this.transform.position, radius, rayLength);
            this.LastDetectedObject = sphereCaster.LastDetectedObject;

           // Debug.Log(OnGround + " / " + LastDetectedObject);
        }

        
        // // SphereCast 範囲のテスト用
        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawSphere(this.transform.position, radius);
        //    Gizmos.DrawSphere(this.transform.position + (Vector3.down * rayLength), radius);
        //}



        private class SphereCaster
        {
            private LayerMask mask;
            private Vector3 offset;
            private  RaycastHit hitInfo;
            public GameObject LastDetectedObject { get; private set; }


            public SphereCaster(LayerMask _mask)
            {
                this.mask = _mask;
            }


            /// <summary>
            /// SphereCast はそのSphereの内側にあるCollider を検出しない。SphereCast のorigin地点からすでに何かと重なっている場合、そのCollider は無視されるので注意。
            /// </summary>
            /// <param name="currentPosition"></param>
            /// <returns></returns>
            public bool Cast(Vector3 currentPosition, float radius, float rayLength, float _offset = 0)
            {
                this.offset = Vector3.up * _offset;
                
                // SphereCast についての注意点...内側にあるCollider を検出しない。厳密な意味で"接触"した時のみtrueを返す。
                // Notes: SphereCast will not detect colliders for which the sphere overlaps the collider.
                bool result = Physics.SphereCast(
                    currentPosition + offset,
                    radius,
                    Vector3.down,
                    out hitInfo,
                    rayLength,
                    mask,
                    QueryTriggerInteraction.Ignore);
                
                if (result) { LastDetectedObject = hitInfo.collider.gameObject; }
                //if (result) { LastDetectedObject = null; }
                
                return result;
            }
        }


        /// <summary>
        /// 毎フレーム接地確認を行わずに済むよう、確認の必要/不必要を判断するクラス。でも書いてみたら判断自体が軽いかどうか怪しい気が。
        /// </summary>
        private class DetectionEconomizer
        {
            // 全体的にこのクラスは命名がいまいちな気がする。
            private Vector3 lastPosition;


            public DetectionEconomizer()
            {
                lastPosition = Vector3.zero;
            }


            // Update 関数で呼ぶ。
            public bool NeedsDetecting(Transform _transform)
            {
                bool result = true;

                // 前のフレームでの座標(transform.position)と現在のフレームでの座標をMathf.Approximately() で比較、同じなら検知の必要なしとする。
                if (IsSameVector3(lastPosition, _transform.position))
                {
                    result = false;
                } 

                // 最後に、現在のフレームでの座標をlastPositionに代入しておく。
                lastPosition = _transform.position;

                return result;
            }


            // bool を返すメソッドの動詞は三単現で書くらしい。
            private static bool IsSameVector3(Vector3 vectorA, Vector3 vectorB)
            {
                // Mathf.Approximately() は float 型の誤差を考慮した上で比較を行うメソッド。

                if ( Mathf.Approximately(vectorA.x, vectorB.x) == false) { return false; }
                if ( Mathf.Approximately(vectorA.y, vectorB.y) == false) { return false; }
                if ( Mathf.Approximately(vectorA.z, vectorB.z) == false) { return false; }

                return true;
            }
        }
    }
}