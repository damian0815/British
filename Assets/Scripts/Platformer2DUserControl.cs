using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof (PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }


    private void Update()
    {
        if (!DistributeUI.Instance.Visible && !MarketResearchUI.Instance.Visible) {
            UpdateInput();
        }
    }

    private void UpdateInput()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (CrossPlatformInputManager.GetButtonDown("PickUp")) {
            if (m_Character.CarryingObject == null) {
                m_Character.AttemptPickUp();
            } else {
                m_Character.Drop();
            }
        }

    }


    private void FixedUpdate()
    {
        if (!DistributeUI.Instance.Visible && !MarketResearchUI.Instance.Visible) {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }


}
