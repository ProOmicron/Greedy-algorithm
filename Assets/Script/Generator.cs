using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IISCI.Viktor
{
    public class Generator : MonoBehaviour
    {
        #region Parameters
        [SerializeField]
        private GameObject[] _objects;

        private GameObject tempObject;
        private float _timeEnd = 30f; //Время до конца спавна объектов. По умолчанию.
        private float _spawnRadius = 30; //Область появления объектов.

        private bool _finish = false;
        #endregion

        IEnumerator TimeEnd()
        {
            StartCoroutine(GenerateObject());
            yield return new WaitForSeconds(_timeEnd); //Ждём заданное количество секунд. По умолчанию 30 секунд.
            _finish = true;            
            StopAllCoroutines(); //Останавливаем крутину генерации.
        }

        IEnumerator GenerateObject()
        {
            while (true)
            {
                tempObject = Instantiate(_objects[Random.Range(0, _objects.Length)], Random.insideUnitSphere * _spawnRadius, Quaternion.identity);
                Destroy(tempObject, 1f); //Созданный объект уничтожится через 1 секунду.
                //Debug.Log("Объект " + tempObject.ToString() + " сгенерировался по координатам " + tempObject.transform.position);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f)); 
            }
        }

        public void StartCoroutine()
        {
            StartCoroutine(TimeEnd());            
        }

        public void StartCoroutine(float timeEnd) //Перегруженный метод для задания нужного времени завершения генерации.
        {
            _timeEnd = timeEnd;
            StartCoroutine(TimeEnd());            
        }

        public bool Finish()
        {
            return _finish;
        }
    }
}
