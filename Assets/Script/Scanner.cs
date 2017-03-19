using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Данный класс сканирует всю сцену на наличие новых объектов с тегом Object, 
 * если поиск по тегу противоречит ТЗ, можно изменить на другие методы обработки.  
 */

namespace IISCI.Viktor
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> _objectPos;
        private GameObject _foundObj;

        private bool _finish = false;

        private void Update()
        {
            if (_finish)
            {
                _foundObj = GameObject.FindGameObjectWithTag("Object");
                if (_foundObj)
                {
                    _objectPos.Add(_foundObj.transform.position);
                    _foundObj.tag = "Registered";
                }
            }            
        }

        public List<Vector3> PosList()
        {
            return _objectPos;
        }

        public void StartCoroutine()
        {
            _finish = true;
        }

        public void Stop()
        {
            _finish = false;
        }
    }
}
