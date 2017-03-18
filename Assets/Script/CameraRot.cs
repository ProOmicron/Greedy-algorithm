using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI
{
    public class CameraRot : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        private float _rot;

        IEnumerator Move()
        {
            while (true)
            {
                transform.rotation = Quaternion.AngleAxis(_rot, Vector3.up);
                _rot += Time.deltaTime * _speed;
                yield return new WaitForEndOfFrame();
            }            
        }

        public void StartOf()
        {
            StartCoroutine("Move");
        }
    }
}
