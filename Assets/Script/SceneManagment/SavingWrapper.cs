using UnityEngine;
using RPG.Saving;
using System.Collections;

namespace RPG.SceneManagment
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string SaveFileName = "save2";
        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(SaveFileName);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(SaveFileName);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(SaveFileName);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(SaveFileName);
        }
    } 
}
