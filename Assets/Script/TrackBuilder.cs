using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace IISCI.Viktor
{
    public class TrackBuilder : MonoBehaviour
    {
        private Scanner _scanner;
        private List<Vector3> _scannerList;
                
        [SerializeField]
        private List<Vector3> _minTrackList;
      

        [SerializeField]
        private List<float> _sortPointsDisCount;
        [SerializeField]
        private float _minPointsDis;
        [SerializeField]
        private int index;

        private bool _flag = false;
        
        private void Update()
        {
            if (_flag)
            {
                _flag = false;
                ArrayInport();
                SearchMinTrack();                
            }            
            DrawTrack(_minTrackList, Color.blue);
        }

        private void ArrayInport()
        {            
            _scannerList = GameObject.Find("Scanner").GetComponent<Scanner>().PosList();
        }

        private void SearchMinTrack() //Поиск минимального пути
        {

            for (int startPoint = 0; startPoint < _scannerList.Count; startPoint++) //Ищем самые короткие пути относительно конкретной начальной точки.
            {
                float _sortPointsTrack = 0;
                List<Vector3> trackList = new List<Vector3>();

                trackList.AddRange(SearchTrack(_scannerList, startPoint));

                _sortPointsTrack = LengthOfTrack(trackList);

                _sortPointsDisCount.Add(_sortPointsTrack);
            }

            index = _sortPointsDisCount.FindIndex(x => x == _sortPointsDisCount.Min()); //Присваиваем начальную точку, относительно которой самый короткий путь.

            _minTrackList.AddRange(SearchTrack(_scannerList, index));

            _minPointsDis = LengthOfTrack(_minTrackList);

        }

        private List<Vector3> SearchTrack(List<Vector3> list, int startPoint) //Этот метод возвращает массив точек, расположенных таким образом, что формирую самый короткий путь
        {            
            List<Vector3> _sortList = new List<Vector3>();
            List<Vector3> _pointsList = new List<Vector3>();

            _pointsList.AddRange(list);
            _sortList.Add(_pointsList[startPoint]);
            _pointsList.RemoveAt(startPoint);

            for (int i = 0; i < _sortList.Count; i++)
            {
                int index = 0;
                float minDistance = 1000;
                for (int j = 0; j < _pointsList.Count; j++)
                {
                    if (minDistance > (_sortList[i] - _pointsList[j]).sqrMagnitude) //В целях оптимизации мы отказываемся, от этого выражения Vector3.Distance(_sortList[i], _pointsList[j])
                    {
                        minDistance = (_sortList[i] - _pointsList[j]).sqrMagnitude;
                        index = j;
                    }
                }
                if (_pointsList.Count >= 1)
                {
                    _sortList.Add(_pointsList[index]);
                    _pointsList.RemoveAt(index);
                }
            }
            
            return _sortList;
        }

        private void DrawTrack(List<Vector3> arr, Color color)
        {
            for (int i = 0; i < arr.Count - 1; i++)
            {
                Debug.DrawLine(arr[i], arr[i + 1], color);
            }
        }

        private float LengthOfTrack (List<Vector3> list)
        {
            float lengthOfTrack = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                lengthOfTrack += Vector3.Distance(list[i], list[i + 1]);
            }
            return lengthOfTrack;
        }

        public void Flag() // После завершения генерации объектов данный флаг запускает инпорт и сортировку.
        {
            _flag = true;
        }
    }
}
