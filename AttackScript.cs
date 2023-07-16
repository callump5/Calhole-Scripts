using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AttackScript : MonoBehaviour 
{
    #region --- Variables ---
        public float _PunchSpeed = 10f;
        public float _PunchResetSpeed = 14f; 

        public TwoBoneIKConstraint _RightHand = null;
        public Transform _Target;
        public Transform _PullbackZone;
        public Transform _AttackZone;
        public Transform[] _Enemy = new Transform[3];

        private RigAnimMode _Mode = RigAnimMode.off;
        private AttackMode _AttackMode = AttackMode.off;
        private bool _isCoroutineRunning = false;

    #endregion
    #region --- Enums ---
        private enum RigAnimMode{
            off,
            inc,
            dec
        }

        private enum AttackMode{
            off,
            stab,
            swing
        }
    #endregion
    #region --- Helpers ---
        public void StartAnimationCoroutine(){
            _isCoroutineRunning = true;
            _RightHand.weight = 0;
            _Mode = RigAnimMode.inc;
        }

        public void FinishAnimationCoroutine(){
            _Mode = RigAnimMode.dec;
            _isCoroutineRunning = false;
        }
    #endregion
    #region --- Coroutines ---
        private IEnumerator AttackCoroutine()
        {
            Debug.Log("Attack Coroutine");
            yield return StartCoroutine(AttackWindup());
            yield return StartCoroutine(Stab());
            FinishAnimationCoroutine();
        }
        private IEnumerator AttackWindup()
        {
            Debug.Log("Wind Up");

            while (Vector3.Distance(_Target.position, _PullbackZone.position) >= 0.01f)
            {
                _Target.position = Vector3.Lerp(_Target.position, _PullbackZone.position, _PunchSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator Stab()
        {
            Debug.Log("Stab");

            while (Vector3.Distance(_Target.position, _AttackZone.position) >= 0.01f)
            {
                _Target.position = Vector3.Lerp(_Target.position, _AttackZone.position, _PunchSpeed * Time.deltaTime);
                yield return null;
            }
        }


    #endregion
    #region --- Unity Functions ---
        private void Start()
        {
            _RightHand.weight = 0;        
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Starting Attack");
                if (!_isCoroutineRunning)
                {
                    StartAnimationCoroutine();
                    StartCoroutine(AttackCoroutine());
                }
            }

            switch (_Mode)
            {
                case RigAnimMode.inc:
                    _RightHand.weight = Mathf.MoveTowards(_RightHand.weight, 1f, _PunchSpeed * Time.deltaTime);
                    break;
                case RigAnimMode.dec:
                    _RightHand.weight = Mathf.MoveTowards(_RightHand.weight, 0f, _PunchResetSpeed * Time.deltaTime);
                    if (Mathf.Approximately(_RightHand.weight, 0f))
                    {
                        _Mode = RigAnimMode.off;
                    }
                    break;
                case RigAnimMode.off:
                    _RightHand.weight = 0f;
                    break;
            }
        }
    #endregion

}