using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IISCI.Viktor
{
    public class UIController : MonoBehaviour
    {
        private bool _finish = false;
        private Text _text;

        IEnumerator StartUI()
        {
            _text = GameObject.Find("Text").GetComponent<Text>();
            _text.text = GameObject.Find("Player").GetComponent<MoveController>().MyToString();
            yield return new WaitForEndOfFrame();
        }             

        public void StartOf() 
        {
            StartCoroutine("StartUI");
        }        
    }
}
