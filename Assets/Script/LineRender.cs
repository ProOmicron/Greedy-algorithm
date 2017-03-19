using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class LineRender : MonoBehaviour
    {        
        private bool _finish = false;        

        public void StartLineRender()//После завершения движения, данный метод запускает инпорт и сортировку.
        {
            List<Vector3> scannerList = GameObject.Find("TestObject").GetComponent<MoveController>().Export();
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.numPositions = scannerList.Count;
            lineRenderer.SetPositions(scannerList.ToArray());            
            _finish = true;            
        }        

        public bool Finish()//Посылаем сигнал, о том что завершили работу данного класса.
        {            
            return _finish;
        }
    }
}
