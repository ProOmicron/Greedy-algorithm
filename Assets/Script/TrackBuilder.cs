using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace IISCI.Viktor
{
    public class TrackBuilder : MonoBehaviour
    {
        #region Parameters
        private Scanner _scanner;
        private List<Vector3> _inportList;        

        [SerializeField] //Длины всех самых коротких путей относительно всех начальных точек.
        private List<float> _trackLengthList; 
        [SerializeField] //Длина самой короткой пути.
        private float _minTrack; 
        [SerializeField] //Номер точки относительно которой самый короткий путь.
        private int _indexInMinTrack;
        [SerializeField] //Расположение точек который образует самый короткий путь.
        private List<Vector3> _minTrackPointsList;

        private bool _finish = false;
        #endregion

        public void StartOf() // После завершения генерации объектов данный флаг запускает инпорт и сортировку.
        {
            InportList(); //Закешируем инпортированный лист.
            SearchMinTrack(); //Ищем самый короткий путь.            
            _finish = true;            
        }

        private void InportList()
        {
            try
            {
                _inportList = GameObject.Find("Scanner").GetComponent<Scanner>().PosList();

            }
            catch (Exception ex)
            {
                Debug.Log("При инпорте массива произошла ошибка: " + ex);
                throw;
            }

        }

        private void SearchMinTrack() //Поиск минимального пути моё виденние алгоритнма Дейкстры
        {
            for (int startPoint = 0; startPoint < _inportList.Count; startPoint++) //Ищем самые короткие пути относительно конкретной начальной точки.
            {                
                List<Vector3> trackList = new List<Vector3>();//Чтобы бы лучше читался код добавлю буферную коллекцию из векторов.

                trackList.AddRange(SearchMinTrack(_inportList, startPoint));//Сперва находим кротчайший путь с помощью метода SearchMinTrack с заданным начальной точкой.

                _trackLengthList.Add(LengthOfTrack(trackList));//Потом после того как нашли кротчайший путь, вычисляем его длину и записываем в базу данных путей.
            }

            _indexInMinTrack = _trackLengthList.FindIndex(x => x == _trackLengthList.Min()); //Присваиваем начальную точку, относительно которой самый короткий путь.

            _minTrackPointsList.AddRange(SearchMinTrack(_inportList, _indexInMinTrack));

            _minTrack = LengthOfTrack(_minTrackPointsList);

        }

        private List<Vector3> SearchMinTrack(List<Vector3> list, int startPoint) //Этот метод возвращает массив точек, расположенных таким образом, что формирую самый короткий путь, относительно начальной точки.
        {            
            List<Vector3> sortList = new List<Vector3>();
            List<Vector3> pointsList = new List<Vector3>();

            pointsList.AddRange(list);
            sortList.Add(pointsList[startPoint]);
            pointsList.RemoveAt(startPoint);

            for (int i = 0; i < sortList.Count; i++)
            {
                int index = 0;
                float minDistance = Single.PositiveInfinity;
                for (int j = 0; j < pointsList.Count; j++)
                {
                    if (minDistance > (sortList[i] - pointsList[j]).sqrMagnitude) //В целях оптимизации мы отказываемся, от этого выражения Vector3.Distance(_sortList[i], _pointsList[j])
                    {
                        minDistance = (sortList[i] - pointsList[j]).sqrMagnitude;
                        index = j;
                    }
                }
                if (pointsList.Count >= 1)
                {
                    sortList.Add(pointsList[index]);
                    pointsList.RemoveAt(index);
                }
            }          
            return sortList;
        }    
        
        private float LengthOfTrack(List<Vector3> list) //Метод возвращающий длину пути
        {
            float lengthOfTrack = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                lengthOfTrack += Vector3.Distance(list[i], list[i + 1]);
            }
            return lengthOfTrack;
        }

        public List<Vector3> Export()
        {
            return _minTrackPointsList;
        }

        public bool Finish()
        {
            if (_finish)
            {
                StopAllCoroutines();
            }
            return _finish;
        }
    }
}
