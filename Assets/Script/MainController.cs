using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IISCI.Viktor
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private bool _generatorFinish;
        [SerializeField]
        private bool _scannerFinish;
        [SerializeField]
        private bool _trackBuilderFinish;
        [SerializeField]
        private bool _moveControllerFinish;
        [SerializeField]
        private bool _lineRenderFinish;
        [SerializeField]
        private bool _UIInformatorFinish;
        [SerializeField]
        private int _case = 0;
        

        void Update()
        {
            switch (_case)
            {
                case 0:
                    GameObject.Find("Generator").GetComponent<Generator>().StartOf(); //Запускаем генерацию объектов.
                    GameObject.Find("Scanner").GetComponent<Scanner>().StartOf(); // Запускаем сканнер объектов.
                    _case = 1;                    
                    break;
                case 1:
                    if (GameObject.Find("Generator").GetComponent<Generator>().Finish()) //Ожидаем завершения генерации.
                    {
                        _generatorFinish = true;
                        _case = 2;
                    }                    
                    break;
                case 2:
                    GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().StartOf(); //Запускаем поиск самого короткого пути.
                        _case = 3;
                    break;
                case 3:
                    if (GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().Finish()) //Ожидаем завершения поиска пути.
                    {
                        _trackBuilderFinish = true;
                        _case = 4;
                    }                    
                    break;                
                case 4:
                    GameObject.Find("Player").GetComponent<MoveController>().StartOf(); //Запускаем движущийся объект.
                        _case = 5;
                    break;
                case 5:
                    if (GameObject.Find("Player").GetComponent<MoveController>().Finish()) //Ожидаем авершения объекта.
                    {
                        _moveControllerFinish = true;
                        _case = 6;
                    }
                    break;
                case 6:
                    GameObject.Find("LineRender").GetComponent<LineRender>().StartOf(); //Запускаем отрисовку пути в классе LineRender.
                    GameObject.Find("MainCameraRotPoint").GetComponent<CameraRot>().StartOf(); //Запускаем вращение камеры.
                    GameObject.Find("UIController").GetComponent<UIController>().StartOf();
                    _case = 7;
                    break;
                case 7:
                    if (GameObject.Find("LineRender").GetComponent<LineRender>().Finish()) //Ожидаем завершение отрисовки пути.
                    {
                        GameObject.Find("Scanner").GetComponent<Scanner>().Stop(); //Останавливаем поиск объектов.
                        _lineRenderFinish = true;
                        _case = 8;
                    }
                    break;
                case 8:
                    Debug.Log("Программа завершена");
                    break;                
            }            
        }
    }
}
