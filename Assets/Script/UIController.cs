using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IISCI.Viktor
{
    public class UIController : MonoBehaviour
    {
        private Text _text;

        public void StartUI() 
        {
            _text = GameObject.Find("Text").GetComponent<Text>();
            _text.text = GameObject.Find("TestObject").GetComponent<MoveController>().MyToString();            
        }        
    }
}
