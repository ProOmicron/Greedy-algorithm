using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class Generator : MonoBehaviour
    {
        #region Parameters
        [SerializeField]
        private float _timeEnd = 10f; //Время до конца спавна объектов.
        [SerializeField]
        private float _timeEndCount; //Текущее время.

        private float _timeSpawn; //Время до появления объекта.
        private float _timeSpawnCount; //Время после появления предыдущего объекта.

        [SerializeField]
        private GameObject[] _objects;
        public GameObject tempObject;

        //Область появления объектов.
        [SerializeField]
        private float _spawnRange = 1;

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
                    tempObject = Instantiate(_objects[Random.Range(0, _objects.Length)], new Vector3(Random.Range(-_spawnRange, _spawnRange), Random.Range(-_spawnRange, _spawnRange), Random.Range(-_spawnRange, _spawnRange)), Quaternion.identity);
                    Debug.Log("Объект " + tempObject.ToString() + " появился на точке" + tempObject.transform.position);
                    //Destroy(tempObject, 1);
                    _timeSpawnCount = 0; //Обнуляем таймер спавна объектов. 
                    _timeSpawn = Random.Range(0.1f, 1f); //Задаём, время следующего спавна объектов.
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
