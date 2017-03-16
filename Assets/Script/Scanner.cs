using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> _objectPos;
        [SerializeField]
        private GameObject _foundObj;
        private GameObject _oldObj;

        private bool _findNewObj;
        
        private void Update()
        {
            _foundObj = GameObject.FindGameObjectWithTag("Object");
            if (_oldObj != _foundObj)
            {
                _objectPos.Add(_foundObj.transform.position);
            }
            _oldObj = _foundObj;
        }

        public Vector3[] PosArray()
        {
            Vector3[] posArray = new Vector3[_objectPos.Count];
            int i = 0;
            foreach (Vector3 pos in _objectPos)
            {
                posArray[i] = pos;
                i++;
            }
            return posArray;
        }

        public int Lenght()
        {            
            int i = 0;
            foreach (Vector3 pos in _objectPos)
            {                
                i++;
            }
            return i;
        }
    }
}
