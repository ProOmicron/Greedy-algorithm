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
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red; //Для большей констрактности картинки.
        }

        private void InportList()
        {
            _scannerList = GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().Export(); //Можно было положить данную функцию, в тело короутины, но для более лучшей читабельности.
        }

        IEnumerator Move()
        {            
            InportList();
            _timer = Time.time;
            transform.position = _scannerList[0];
            _oldPosition = transform.position;

            while (_targetpoint != _scannerList.Count)
            {                
                transform.position = Vector3.MoveTowards(transform.position, _scannerList[_targetpoint], _speed * Time.deltaTime); //Движение выполняет данный метод, который обеспечивает постоянную скорость.

                //Данный метод возвращает номер следующей точки, при достижении цели. Можно было расчитывать длину пути через каждый кадр, 
                // но это откровенно не оптимизировано, поэтому грубо говоря рачитываем растояние от старой точки до новой.
                if (transform.position == _scannerList[_targetpoint]) 
                {
                    _track += Vector3.Distance(transform.position, _oldPosition);
                    _targetpoint++;
                    _oldPosition = transform.position;
                }
                if (_targetpoint == _scannerList.Count) //Если дошли до последней точки, завершаем работу класса, и запиываем время(по ТЗ).
                {
                    _timer = Time.time - _timer;
                    _finish = true;
                    StopAllCoroutines();
                }
                
                yield return new WaitForEndOfFrame();
            }         
        }

        public void StartMove() //Получаем сигнал о старте работы данного класса.
        { 
            StartCoroutine(Move());
        }

        public void StartCoroutine(float speed) //Получаем сигнал о старте работы данного класса. С изменённой скоростью тестового объекта.
        {
            _speed = speed;
            StartCoroutine(Move());
        }

        public bool Finish()//Посылаем сигнал, о том что завершили работу данного класса.
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

        public string MyToString() //Собственный ToString() метод, для класса отвечающий UI.
        {
            return "Длина пути : " + _track + " unit.\r\nКоличество пройденных точек: " + _targetpoint + " шт.\r\nВремя на прохождение: " + _timer + " сек.";
        }
    }
}