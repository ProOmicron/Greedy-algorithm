using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IISCI.Viktor
{
    public class TrackBuilder : MonoBehaviour
    {
        private Scanner _scannerList;
        [SerializeField]
        private Vector3[] _points;
        [SerializeField]
        private float _pointsDis;
        [SerializeField]
        private Vector3[] _sortPoints;
        [SerializeField]
        private float _sortPointsDis;
        public bool _flag = false;

        private void Update()
        {
            if (_flag)
            {
                ArrayInport();
                SortArray(_points);
                _flag = false;
            }
            DrawLine(_points, Color.red);            
            DrawLine(_sortPoints, Color.blue);
        }

        public void Flag() // После завершения генерации объектов данный флаг запускает инпорт и сортировку.
        {
            _flag = true;
        }

        private void ArrayInport()
        {
            _scannerList = GameObject.Find("Scanner").GetComponent<Scanner>();
            if (_scannerList)
            {
                _points = new Vector3[_scannerList.Lenght()];
                _points = _scannerList.PosArray();
                for (int i = 0; i < _points.Length - 1; i++)
                {
                    _pointsDis += Vector3.Distance(_points[i], _points[i + 1]);
                }                
            }
        }
        

        private void SortArray(Vector3[] array)
        {
            ArrayList distanceArr = new ArrayList();
            for (int i = 0; i < array.Length; i++)
            {
                float pos = (array[i] - array[0]).sqrMagnitude;
                distanceArr.Add(pos);
            }

            _sortPoints = new Vector3[array.Length];

            float[] arrayOfDists = new float[array.Length];

            for (int i = 0; i < array.Length; ++i)
            {
                _sortPoints[i] = (Vector3)array[i];
                arrayOfDists[i] = (float)distanceArr[i];
            }

            System.Array.Sort(arrayOfDists, _sortPoints);// Сортировка

            for (int i = 0; i < _sortPoints.Length - 1; i++)
            {
                _sortPointsDis += Vector3.Distance(_sortPoints[i], _sortPoints[i + 1]);
            }            
        }

        private void DrawLine(Vector3[] arr, Color color)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                Debug.DrawLine(arr[i], arr[i + 1], color);
            }
        }
    }
}
//private ArrayList SortPoints(ArrayList sortedList, Vector3 startPos)
//{
//    ArrayList distanceArr = new ArrayList();// ArrayList af distances
//    foreach (Vector3 __pos in sortedList)
//    {
//        float pos = (__pos - startPos).sqrMagnitude; // Set distance
//        distanceArr.Add(pos);
//    }

//    Vector3[] arrayOfPos = new Vector3[sortedList.Count];
//    float[] arrayOfDists = new float[distanceArr.Count];

//    for (int i = 0; i < sortedList.Count; ++i)
//    {// Convert to builtin array
//        arrayOfPos[i] = (Vector3)sortedList[i];
//        arrayOfDists[i] = (float)distanceArr[i];
//    }

//    System.Array.Sort(arrayOfDists, arrayOfPos);// Сортировка

//    return new ArrayList(arrayOfPos);
//}

