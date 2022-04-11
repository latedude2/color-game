    using UnityEngine;
     
    public class DeleteSavedValues : MonoBehaviour
    {
        /// Add a context menu named "Do Something" in the inspector
        /// of the attached script.
        [ContextMenu("Do Something")]
        void DoSomething()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
