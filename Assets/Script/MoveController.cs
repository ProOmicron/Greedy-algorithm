using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 10f;        
        [SerializeField]
        private List<Vector3> _scannerList;
        [SerializeField]
        private int _targetpoint = 0;
        [SerializeField]
        private float _timer;
        [SerializeField]
        private float _track;
        private Vector3 _oldPosition;

        private bool _finish = false;

        private void InportList()
        {
            _scannerList = GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().Export();
        }

        IEnumerator Move()
        {
            while (_targetpoint != _scannerList.Count)
            {                
                transform.position = Vector3.MoveTowards(transform.position, _scannerList[_targetpoint], _speed * Time.deltaTime);
                
                if (transform.position == _scannerList[_targetpoint])
                {
                    _track += Vector3.Distance(transform.position, _oldPosition);
                    _targetpoint++;
                    _oldPosition = transform.position;
                }
                if (_targetpoint == _scannerList.Count)
                {
                    _timer = Time.time - _timer;
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
            _oldPosition = transform.position;
            yield return new WaitForEndOfFrame();
            StartCoroutine("Move");
        }

        public void StartOf()
        {
            StartCoroutine("StartMove");
            _timer = Time.time;
        }

        public bool Finish()
        {
            return _finish;
        }

        public string MyToString()
        {
            return "Длина пути : " + _track + "\r\nКоличество пройденных точек: " + _targetpoint + "\r\nВремя на прохождение: " + _timer;
        }
    }
}