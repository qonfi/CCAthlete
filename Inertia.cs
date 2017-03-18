//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//

# region 予定めも
// 設置しているオブジェクトのFixedUpdate ごとの移動量を調べ、Athlete の移動量に加算する。
// 接地が終了したら、接地していたオブジェクトの移動量を追跡するのはやめ、
// 最後に与えられた力を適用しながら減速していく(慣性による移動の継続、空気抵抗による減速、重力による落下)
// 慣性で与えられた力がなくなる前に設置した場合、接地後にその力の数分の1 ぐらいが与えられる？
// 
#endregion

namespace CCAthlete
{
    // 慣性という単語をあまり厳密に使っていない。不適切な箇所もあるかもしれないが、代替となる言葉が思いつくまでそのまんまで。
    public class Inertia : MonoBehaviour
    {
        #region
        private CharacterController character;
        private GroundDetectorSphere detector;
        public GameObject RelativeGround { get; private set; }
        private VelocityMeasurer measurer;
        private Vector3 translating;
        #endregion



        private void Start()
        {
            character = GetComponent<CharacterController>();
            detector = GetComponent<GroundDetectorSphere>();
            measurer = new VelocityMeasurer();
        }


        private void FixedUpdate()
        {
            if (detector.OnGround == false) { return; }
            RelativeGround = detector.LastDetectedObject;
            if (RelativeGround == null) { return; }


            Vector3 positionInThisFrame = RelativeGround.transform.position;
            measurer.CurrentPosition = positionInThisFrame;


            Debug.Log(detector.LastDetectedObject + " was last :" + measurer.LastPosition + " current " + measurer.CurrentPosition);
            InertialMove();


            // FixedUpdate の最後に。
            measurer.LastPosition = positionInThisFrame;
        }


        private void InertialMove()
        {
            
            translating = VelocityMeasurer.CalcTranslatingPerFixedUpdate(measurer.LastPosition, measurer.CurrentPosition);

            //Debug.Log(translating);
            // 50 を乗算するとちょうどいいが、なぜなのかわからぬい。
            character.SimpleMove(translating * 50);  // SimpleMove は メートル/秒 らしい。それってややこしくない?
        }



        private class VelocityMeasurer
        {
            // position ではダメだ。前フレームと現フレームでオブジェクトが違う場合検知できない。gameobjectに書き換える。
            public Vector3 LastPosition { get; set; }
            public Vector3 CurrentPosition { get; set; }


            public static Vector3 CalcTranslatingPerFixedUpdate(Vector3 lastPosition, Vector3 currentPosition)
            {
                Vector3 lastToCurrent = currentPosition - lastPosition;
                
                return lastToCurrent;
            }
        }
    }
}