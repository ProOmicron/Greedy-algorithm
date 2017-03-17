using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> _objectPos;
        
        private GameObject _foundObj;        

        private void Update()
        {
            _foundObj = GameObject.FindGameObjectWithTag("Object");
            if (_foundObj)
            {
                _objectPos.Add(_foundObj.transform.position);
                _foundObj.tag = "Registered";
            }            
        }

        public List<Vector3> PosList()
        {
            return _objectPos;
        }
    }
}
