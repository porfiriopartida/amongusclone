using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace PUN
{
    public class PlayerSetup : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        Text playerNameText;

        public GameObject KillRangeCollider;

        public GameObject PlayerNamePanel;

        public InputController InputController;

        public Canvas Hud;
        private void Start()
        {
            Camera mainCamera = Camera.main;

            //Inject color
            GetComponent<SpriteRenderer>().color = SceneStateManager.Instance.GetColor(photonView.Owner);
            
            if (photonView.IsMine)
            {
                MomongoController momongoController = transform.GetComponent<MomongoController>();
                momongoController.ShowMask();
                SceneStateManager.Instance.MomongoController = momongoController;
                InputController.enabled = true;
                transform.GetComponent<InputController>().enabled = true;
                if (mainCamera != null)
                {
                    mainCamera.gameObject.GetComponent<CameraFollow>().SetTarget(transform);
                }
                
                Hud.gameObject.SetActive(true);
                Hud.worldCamera = mainCamera;
                Hud.planeDistance = 50;
            }
            else
            {
                InputController.enabled = false;
                Hud.gameObject.SetActive(false);
                transform.GetComponent<InputController>().enabled = false;
            }

            this.gameObject.name = photonView.Owner.NickName;
            this.gameObject.transform.parent = SceneStateManager.Instance.Spawn.transform;
            SetPlayerUI();

        }
        public void SetPlayerUI()
        {
            if (playerNameText.text != null && photonView.Owner != null)
            {
                playerNameText.text = photonView.Owner.NickName;
            }

            bool isLocalImpostor = SceneStateManager.Instance.IsImpostor(PhotonNetwork.LocalPlayer);
            bool isThisImpostor = SceneStateManager.Instance.IsImpostor(photonView.Owner);
            if (isLocalImpostor)
            {
                if (isThisImpostor)
                {
                    //TODO: If we want to make this configurable, add isMine check so impostor doesn't know.
                    playerNameText.color = Color.red;
                    KillRangeCollider.SetActive(false);
                }
                else
                {
                    KillRangeCollider.SetActive(true);
                }
            }
        }
    }
}
