using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PrefillColors : MonoBehaviour
    {
        public CharacterColors characterColors;

        public GameObject ImagePrefab;
        void Start()
        {
            foreach (var characterColor in characterColors.colors)
            {
                //Image img = this.gameObject.AddComponent<Image>();
                GameObject createdGameObject = Instantiate(ImagePrefab, this.transform);
                createdGameObject.GetComponent<Image>().color = characterColor;
            }
        }
    }
}
