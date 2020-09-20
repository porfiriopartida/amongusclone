using System.Collections.Generic;
using LopapaGames.Common.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PrefillColors : MonoBehaviour
    {
        public CharacterColors CharacterColors;
        public GameObject ImagePrefab;
        public List<GameObject> ImagesGameObjects;
        public GameEvent ColorUpdated;
        void Start()
        {
            ImagesGameObjects = new List<GameObject>();
            int idx = 0;
            foreach (var characterColor in CharacterColors.colors)
            {
                //Image img = this.gameObject.AddComponent<Image>();
                GameObject createdGameObject = Instantiate(ImagePrefab, this.transform);
                createdGameObject.GetComponent<Image>().color = characterColor;
                ColorSelectorIndex csi = createdGameObject.GetComponent<ColorSelectorIndex>();
                csi.Idx = idx++;
                List<int> takenColors = SceneStateManager.Instance.GetTakenColors();
                csi.Taken.SetActive(takenColors.Contains(csi.Idx));
                ImagesGameObjects.Add(createdGameObject);
            }
            
            ColorUpdated.Raise();
        }

        public void UpdateColors()
        {
            List<int> takenColors = SceneStateManager.Instance.GetTakenColors();
            
            for (var index = 0; index < ImagesGameObjects.Count; index++)
            {
                var imagesGameObject = ImagesGameObjects[index];
                ColorSelectorIndex colorIdx = imagesGameObject.GetComponent<ColorSelectorIndex>();
                var taken = colorIdx.Taken;
                taken.SetActive(takenColors.Contains(colorIdx.Idx));
            }
        }
    }
}
