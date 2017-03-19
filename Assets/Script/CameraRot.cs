using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI
{
    public class CameraRot : MonoBehaviour
    {        
        private float _speed = 10;
        private float _rot;

        IEnumerator Move() //Данный метод просто вращает объект, к которому прикреплена камера.
        {
            while (true)
            {
                transform.rotation = Quaternion.AngleAxis(_rot, Vector3.up);
                _rot += Time.deltaTime * _speed;
                yield return new WaitForEndOfFrame();
            }            
        }

        public void StartCoroutine()
        {
            StartCoroutine(Move());
        }
    }
}
