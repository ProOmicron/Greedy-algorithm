using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private List<Vector3> _scannerList;
        [SerializeField]
        private int _targetpoint = 0;
        private bool _finish = false;

        private void InportList()
        {
            _scannerList = GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().Export();
        }

        IEnumerator Move()
        {
            while (_targetpoint != _scannerList.Count)
            {
                float step = _speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _scannerList[_targetpoint], step);                
                if (transform.position == _scannerList[_targetpoint])
                {
                    _targetpoint++;
                }
                if (_targetpoint == _scannerList.Count)
                {
                    _finish = true;
                    StopAllCoroutines();
                }
                yield return new WaitForEndOfFrame();
            }         
        }

        IEnumerator StartMove()
        {
            InportList();
            transform.position = _scannerList[0];
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            yield return new WaitForEndOfFrame();
            StartCoroutine("Move");
        }

        public void StartOf()
        {
            StartCoroutine("StartMove");
        }
        public bool Finish()
        {
            return _finish;
        }
    }
}