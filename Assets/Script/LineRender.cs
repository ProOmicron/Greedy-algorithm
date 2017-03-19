using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class LineRender : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private Scanner _scanner;
        private bool _finish = false;

        public void StartCoroutine() // После завершения генерации путей данный флаг запускает инпорт и сортировку.
        {
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            _lineRenderer.numPositions = InportList().Length;
            _lineRenderer.SetPositions(InportList());            
            _finish = true;
            StopAllCoroutines();
        }

        private Vector3[] InportList()
        {            
            try
            {
                List<Vector3> _scannerList = GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().Export();
                Vector3[] arr = _scannerList.ToArray();
                return arr;
            }
            catch (Exception ex)
            {
                Debug.Log("При инпорте массива произошла ошибка: " + ex);
                throw;
            }            
        }        

        public bool Finish()
        {            
            return _finish;
        }
    }
}
