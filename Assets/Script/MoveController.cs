using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace IISCI.Viktor
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> _scannerList;
        [SerializeField]
        private int _targetpoint = 0;
        [SerializeField]
        private float _timer;
        [SerializeField]
        private float _track;
        private float _speed = 10f;
        private Vector3 _oldPosition;

        private bool _finish = false;

        private void Start()
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        private void InportList()
        {
            _scannerList = GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().Export();
        }

        IEnumerator Move()
        {            
            InportList();
            _timer = Time.time;
            transform.position = _scannerList[0];
            _oldPosition = transform.position;

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

        public void StartCoroutine()
        { 
            StartCoroutine(Move());
        }

        public void StartCoroutine(float speed)
        {
            _speed = speed;
            StartCoroutine(Move());
        }

        public bool Finish()
        {
            if (_finish)
            {
                StopAllCoroutines();
            }           
            return _finish;
        }

        public List<Vector3> Export() //По ТЗ путь должен быть наследован от Тестового объекта.
        {
            return _scannerList;
        }

        public string MyToString()
        {
            return "Длина пути : " + _track + " unit.\r\nКоличество пройденных точек: " + _targetpoint + " шт.\r\nВремя на прохождение: " + _timer + " сек.";
        }
    }
}