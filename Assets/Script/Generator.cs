using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class Generator : MonoBehaviour
    {
        #region Parameters
        [SerializeField]
        private float _timeEnd = 30f; //Время до конца спавна объектов.

        private float _timeSpawn; //Время до появления объекта.
        private float _timeSpawnCount; //Время после появления предыдущего объекта.

        [SerializeField]
        private GameObject[] _objects;
        public GameObject tempObject;
        
        [SerializeField] //Область появления объектов.
        private float _spawnRadius = 1;

        private bool _generatorEndFlag = false; //Флаг завершения генерации 
        private TrackBuilder _trackBuilder;

        private bool _finish = false;
        #endregion

        IEnumerator TimeEnd()
        {            
            yield return new WaitForSeconds(_timeEnd); //Ждём заданное количество секунд.
            _finish = true;            
            StopAllCoroutines(); //Останавливаем крутину генерации.
        }

        IEnumerator GenerateObject()
        {
            while (true)
            {
                tempObject = Instantiate(_objects[Random.Range(0, _objects.Length)], Random.insideUnitSphere * _spawnRadius, Quaternion.identity);
                Destroy(tempObject, 1f); //Созданный объект уничтожится через 1 секунду.
                Debug.Log("Объект " + tempObject.ToString() + " сгенерировался по координатам " + tempObject.transform.position);
                yield return new WaitForSeconds(Random.Range(0.1f, 1f));
            }
        }

        public void StartCoroutine()
        {
            StartCoroutine(TimeEnd());
            StartCoroutine(GenerateObject());
        }
        public bool Finish()
        {
            if (_finish)
            {
                StopAllCoroutines();
            }
            return _finish;
        }
    }
}
