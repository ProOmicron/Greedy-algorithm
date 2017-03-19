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
        private List<Vector3> _inportList;        

        [SerializeField] //Длины всех самых коротких путей, относительно всех начальных точек.
        private List<float> _trackLengthList; 
        [SerializeField] //Длина самой короткой пути.
        private float _minTrack = 0; 
        [SerializeField] //Номер точки относительно которой самый короткий путь.
        private int _indexInMinTrack;
        [SerializeField] //Расположение точек которые образуют самый короткий путь.
        private List<Vector3> _minTrackPointsList;

        private bool _finish = false;
        #endregion

        public void StartTrackBuilder() // После завершения генерации объектов данный метод запускает инпорт и сортировку.
        {
            _inportList = GameObject.Find("Scanner").GetComponent<Scanner>().PosList(); //Закешируем и инпортируем лист.
            SearchMinTrack(); //Ищем самый короткий путь.
            _finish = true;            
        }

        //Поиск минимального пути моё виденние алгоритма Дейкстры. 
        //Созданный мной алгоритм, проверяет и возвращает самую ближайшую точку, относительно начальной точки.
        //После нахождения ближайшей точки, его присваиваем в нашу коллекцию, и уже относительно этой точки 
        //находим ближайший к нему. И так далее пока не доёдем до последней точки. 
        //Таким образом получаем, массив точке расположенных таким образом, что формируют самый короткий путь 
        //относительно начальной точки.

        private void SearchMinTrack() 
        {
            for (int startPoint = 0; startPoint < _inportList.Count; startPoint++) //Задаёт начальную точку.
            {                
                List<Vector3> trackList = new List<Vector3>();//Чтобы бы лучше читался код добавлю буферную коллекцию из векторов.

                trackList.AddRange(SearchMinTrack(_inportList, startPoint));//Сперва находим кротчайший путь с помощью метода SearchMinTrack с заданным начальной точкой.

                _trackLengthList.Add(LengthOfTrack(trackList));//Потом после того как нашли кротчайший путь, вычисляем его длину и записываем в базу данных путей.
            }

            _indexInMinTrack = _trackLengthList.FindIndex(x => x == _trackLengthList.Min()); //Присваиваем индекс начальной точки, относительно которой самый короткий путь.

            _minTrackPointsList.AddRange(SearchMinTrack(_inportList, _indexInMinTrack)); //Массив точке расположенных таким образом, что формируют самый короткий путь относительно начальной точки у которого самый короткий путь.

            _minTrack = LengthOfTrack(_minTrackPointsList); //Для большей информативности, и дебага ищем ещё раз длину пути. 

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
            for (int i = 0; i < list.Count - 1; i++) //Расчитываем расстояние между точками.
            {
                lengthOfTrack += Vector3.Distance(list[i], list[i + 1]);
            }
            return lengthOfTrack;
        }

        public List<Vector3> Export() //Подготавливаем сетод для экспорта.
        {
            return _minTrackPointsList;
        }

        public bool Finish()//Посылаем сигнал, о том что завершили работу данного класса.
        {
            if (_finish)
            {
                StopAllCoroutines();//Не забываем останавливать все крутины(оптимизация).
            }
            return _finish;
        }
    }
}
