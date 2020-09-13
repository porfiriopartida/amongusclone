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
                transform.GetComponent<InputController>().enabled = false;
            }
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
