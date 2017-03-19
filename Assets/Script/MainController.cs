using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IISCI.Viktor
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private int _case;
        private const int GENERATOR_START = 0;
        private const int GENERATOR_FINISH = 1;
        private const int TRACKBUILDER_START = 2;
        private const int TRACKBUILDER_FINISH = 3;
        private const int MOVE_START = 4;
        private const int MOVE_FINISH = 5;
        private const int PROGRAMM_END_START = 6;
        private const int PROGRAMM_END_FINISH = 7;
        private const int STOP_PROGRAMM = 8;


        private void Start()
        {
            _case = GENERATOR_START;
            StartCoroutine(Main());
        }

        IEnumerator Main()
        {
            switch (_case)
            {
                case GENERATOR_START:
                    GameObject.Find("Generator").GetComponent<Generator>().StartCoroutine(); //Запускаем генерацию объектов.
                    GameObject.Find("Scanner").GetComponent<Scanner>().StartCoroutine(); // Запускаем сканнер объектов.
                    _case = GENERATOR_FINISH;                    
                    break;
                case GENERATOR_FINISH:
                    if (GameObject.Find("Generator").GetComponent<Generator>().Finish()) //Ожидаем завершения генерации.
                    {
                        Debug.Log("Генерация завершился!");
                        _case = TRACKBUILDER_START;
                    }                    
                    break;
                case TRACKBUILDER_START:
                    GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().StartOf(); //Запускаем поиск самого короткого пути.
                        _case = TRACKBUILDER_FINISH;
                    break;
                case TRACKBUILDER_FINISH:
                    if (GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>().Finish()) //Ожидаем завершения поиска пути.
                    {                        
                        _case = MOVE_START;
                    }                    
                    break;                
                case MOVE_START:
                    GameObject.Find("Player").GetComponent<MoveController>().StartCoroutine(); //Запускаем движущийся объект.
                        _case = MOVE_FINISH;
                    break;
                case MOVE_FINISH:
                    if (GameObject.Find("Player").GetComponent<MoveController>().Finish()) //Ожидаем авершения объекта.
                    {                        
                        _case = PROGRAMM_END_START;
                    }
                    break;
                case PROGRAMM_END_START:
                    GameObject.Find("LineRender").GetComponent<LineRender>().StartCoroutine(); //Запускаем отрисовку пути в классе LineRender.
                    GameObject.Find("MainCameraRotPoint").GetComponent<CameraRot>().StartCoroutine(); //Запускаем вращение камеры.
                    GameObject.Find("UIController").GetComponent<UIController>().StartUI();
                    _case = PROGRAMM_END_FINISH;
                    break;
                case PROGRAMM_END_FINISH:
                    if (GameObject.Find("LineRender").GetComponent<LineRender>().Finish()) //Ожидаем завершение отрисовки пути.
                    {
                        GameObject.Find("Scanner").GetComponent<Scanner>().Stop(); //Останавливаем поиск объектов.                        
                        _case = STOP_PROGRAMM;
                    }
                    break;
                case STOP_PROGRAMM:
                    Debug.Log("Программа завершена");
                    StopAllCoroutines();
                    break;                
            }
            yield return new WaitForSeconds(1f);          
        }
    }
}
