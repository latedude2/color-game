    using UnityEngine;
     
    public class DeleteSavedValues : MonoBehaviour
    {
        [ContextMenu("Delete saves")]
        void DeleteSaves()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
