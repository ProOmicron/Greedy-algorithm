using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Техническое задание:

    В сцене в течение 30 секунд в случайных местах(в заданном диапазоне) появляются
различные объекты на некоторое время, затем они исчезают.

    После того как закончится время - строится кратчайший путь, проходящий через все
объекты и по этому маршруту движется "Тестовый объект" с постоянной скоростью.

    После окончания движения - на экране отображается маршрут движения Тестового объекта
и информация о маршруте, продолжительность, количество пройденных точек, время на прохождение.
*/

namespace IISCI.Viktor
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private float _timeGeneration = 0;
        [SerializeField]
        private float _speedMoveObject = 0;

        #region Consts
        private int _case;
        private const int GENERATOR_START = 1;
        private const int GENERATOR_FINISH = 2;
        private const int TRACKBUILDER_START = 3;
        private const int TRACKBUILDER_FINISH = 4;
        private const int MOVE_START = 5;
        private const int MOVE_FINISH = 6;
        private const int PROGRAMM_END_START = 7;
        private const int PROGRAMM_END_FINISH = 8;
        private const int STOP_PROGRAMM = 9;
        #endregion

        private MoveController _testObject;
        private TrackBuilder _trackBuilder;        
        private UIController _uiController;
        private LineRender _lineRender;
        private Generator _generator;
        private CameraRot _cameraRot;
        private Scanner _scanner;


        private void Start()
        {
            try
            {
                _trackBuilder = GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>();
                _uiController = GameObject.Find("UIController").GetComponent<UIController>();
                _cameraRot = GameObject.Find("MainCameraRotPoint").GetComponent<CameraRot>();
                _testObject = GameObject.Find("TestObject").GetComponent<MoveController>();
                _lineRender = GameObject.Find("LineRender").GetComponent<LineRender>();
                _generator = GameObject.Find("Generator").GetComponent<Generator>();
                _scanner = GameObject.Find("Scanner").GetComponent<Scanner>();
            }
            catch (System.Exception ex)
            {
                Debug.Log("Старт программы произошла с ошибками" + ex.Message);
                throw;
            }            

            StartCoroutine(Main());
        }

        IEnumerator Main()
        {            
            while (true)
            {
                switch (_case)
                {
                    case 0:                        
                        _case = GENERATOR_START;
                        break;

                    case GENERATOR_START:
                        if (_timeGeneration > 0)
                        {
                            _generator.StartCoroutine(_timeGeneration); //Запускаем генерацию объектов, с заданным промежутком врменени. 
                        }
                        else
                        {
                            _generator.StartCoroutine(); //Запускаем генерацию объектов. 
                        }
                        _scanner.StartCoroutine(); // Запускаем сканнер объектов.
                        Debug.Log("Началась генерация объектов");
                        _case = GENERATOR_FINISH;                        
                        break;

                    case GENERATOR_FINISH:                        
                        if (_generator.Finish()) //Ожидаем завершения генерации.
                        {                            
                            Debug.Log("Генерация завершена");
                            _case = TRACKBUILDER_START;
                        }
                        break;

                    case TRACKBUILDER_START:
                        _trackBuilder.StartTrackBuilder(); //Запускаем поиск самого короткого пути.
                        Debug.Log("Запуск поиска короткого пути");
                        _case = TRACKBUILDER_FINISH;
                        break;

                    case TRACKBUILDER_FINISH:
                        if (_trackBuilder.Finish()) //Ожидаем завершения поиска пути.
                        {
                            Debug.Log("Путь найден");
                            _case = MOVE_START;
                        }
                        break;

                    case MOVE_START:
                        if (_speedMoveObject > 0)
                        {
                            _testObject.StartCoroutine(_speedMoveObject); //Запускаем движущийся объект, с заданной скоростью
                        }
                        else
                        {
                            _testObject.StartCoroutine(); //Запускаем движущийся объект, со скростью по умолчанию.
                        }                        
                        Debug.Log("Запуск объекта");
                        _case = MOVE_FINISH;
                        break;
                    case MOVE_FINISH:
                        if (_testObject.Finish()) //Ожидаем авершения объекта.
                        {
                            Debug.Log("Объект завершил движение");
                            _case = PROGRAMM_END_START;
                        }
                        break;

                    case PROGRAMM_END_START:
                        _lineRender.StartLineRender(); //Запускаем отрисовку пути в классе LineRender.
                        _cameraRot.StartCoroutine(); //Запускаем вращение камеры.
                        _uiController.StartUI(); //Запускаем отрисовку информации на экран.
                        Debug.Log("Запуск отрисовки линии и UI контроллера");
                        _case = PROGRAMM_END_FINISH;
                        break;

                    case PROGRAMM_END_FINISH:
                        if (_lineRender.Finish()) //Ожидаем завершение отрисовки пути.
                        {
                            _scanner.Stop(); //Останавливаем поиск объектов.   
                            Debug.Log("Отрисовка линии завершена");
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
}
