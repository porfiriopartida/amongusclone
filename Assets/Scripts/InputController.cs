using System;
using UnityEngine;

namespace DefaultNamespace
{
    
    public class InputController : MonoBehaviour
    {
        
        private MomongoController _momongoController;
        private void Start()
        {
            this._momongoController = GetComponent<MomongoController>();
        }

#if UNITY_IPHONE || UNITY_ANDROID

        private void Update()
        {

            Vector2 direction = Vector2.zero;
            if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x < 150f)
                {
                    direction.x = -1;
                } else if (Input.mousePosition.x > 650f)
                {
                    direction.x = 1;
                }
                
                if (Input.mousePosition.y < 50)
                {
                    direction.y = -1;
                } else if (Input.mousePosition.y > 390)
                {
                    direction.y = 1;
                }
            }
            _momongoController.Move(direction);
        }
#else
        private void Update()
        {
            Vector2 direction = Vector2.zero;
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            if (horizontal != 0)
            {
                direction.x = horizontal;
            }
            
            if (vertical != 0)
            {
                direction.y = vertical;
            }
            
            _momongoController.Move(direction);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _momongoController.Interact();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _momongoController.Report();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                _momongoController.Kill();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _momongoController.Sabotage();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UIMapManager.Instance.ToggleMiniMap();
            }
        }
#endif
    }
}