using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class Generator : MonoBehaviour
    {
        #region Parameters

        private float _timeEnd = 30f; //Время до конца спавна объектов.
        private float _timeEndCount; //Текущее время.

        private float _timeSpawn; //Время до появления объекта.
        private float _timeSpawnCount; //Время после появления предыдущего объекта.

        [SerializeField]
        private GameObject[] _objects;
        public GameObject tempObject;

        //Область появления объектов.
        [SerializeField]
        private float _spawnX = 1;
        [SerializeField]
        private float _spawnY = 1;
        [SerializeField]
        private float _spawnZ = 1;

        private bool _generatorEndFlag = false; //Флаг завершения генерации 
        private TrackBuilder _trackBuilder;
        
        #endregion

        private void Update()
        {
            if (!_generatorEndFlag)
            {
                GeneratorObject();
            }
        }

        private void GeneratorObject()
        {
            if (_timeEnd >= _timeEndCount) //Отсчитываем время до 30 сек
            {
                if (_timeSpawn <= _timeSpawnCount) //Отсчитываем время до следующего спавна объекта
                {
                    tempObject = Instantiate(_objects[Random.Range(0, _objects.Length)], new Vector3(Random.Range(_spawnX * (-1), _spawnX), Random.Range(_spawnX * (-1), _spawnY), 0), Quaternion.identity);
                    Debug.Log("Объект " + tempObject.ToString() + " появился");
                    Destroy(tempObject, 1);
                    _timeSpawnCount = 0; //Обнуляем таймер спавна объектов. 
                    _timeSpawn = Random.Range(1f, 3f); //Задаём, время следующего спавна объектов.
                }
                else
                {
                    _timeSpawnCount += Time.deltaTime;
                }
                _timeEndCount += Time.deltaTime;
            }
            else
            {
                Debug.Log("Генерация завершился!");
                _generatorEndFlag = true; //Сообщаем, что завершили генерацию объектов.
                _trackBuilder = GameObject.Find("TrackBuilder").GetComponent<TrackBuilder>();
                if (_trackBuilder)
                {
                    _trackBuilder.Flag();
                }                
            }
        }
    }
}
