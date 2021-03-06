using UnityEngine;
using UnityEngine.InputSystem;
using RPGCharacterAnimsFREE.Actions;

namespace RPGCharacterAnimsFREE
{
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/index.html")]

	public class RPGCharacterInputSystemController : MonoBehaviour
    {
        RPGCharacterController rpgCharacterController;

		//InputSystem
		public @RPGInputsFREE rpgInputs;

		// Inputs.
		private bool inputJump;
        private bool inputLightHit;
        private bool inputDeath;
        private bool inputAttackL;
        private bool inputAttackR;
		private bool inputBlock;
		private bool inputRoll;
		private bool inputAim;
		private Vector2 inputMovement;
		private bool inputSwitchUp;
		private bool inputSwitchDown;

		// Variables.
		private Vector3 moveInput;
        private bool isJumpHeld;
		private Vector3 currentAim;
		private bool blockToggle;
		private float inputPauseTimeout = 0;
		private bool inputPaused = false;

		private void Awake()
        {
            rpgCharacterController = GetComponent<RPGCharacterController>();
			rpgInputs = new @RPGInputsFREE();
			currentAim = Vector3.zero;
        }

		private void OnEnable()
		{
			rpgInputs.Enable();
		}

		private void OnDisable()
		{
			rpgInputs.Disable();
		}

		private void Update()
		{
			Inputs();
			Moving();
			Damage();
			SwitchWeapons();
			Strafing();
			Rolling();
			Attacking();
		}

		/// <summary>
		/// Pause input for a number of seconds.
		/// </summary>
		/// <param name="timeout">The amount of time in seconds to ignore input</param>
		public void PauseInput(float timeout)
		{
			inputPaused = true;
			inputPauseTimeout = Time.time + timeout;
		}

		/// <summary>
		/// Input abstraction for easier asset updates using outside control schemes.
		/// </summary>
		private void Inputs()
        {
            try {
				inputAttackL = rpgInputs.RPGCharacter.AttackL.WasPressedThisFrame();
				inputAttackR = rpgInputs.RPGCharacter.AttackR.WasPressedThisFrame();
				inputDeath = rpgInputs.RPGCharacter.Death.WasPressedThisFrame();
				inputJump = rpgInputs.RPGCharacter.Jump.WasPressedThisFrame();
				inputLightHit = rpgInputs.RPGCharacter.LightHit.WasPressedThisFrame();
				inputMovement = rpgInputs.RPGCharacter.Move.ReadValue<Vector2>();
				inputRoll = rpgInputs.RPGCharacter.Roll.WasPressedThisFrame();
				inputAim = rpgInputs.RPGCharacter.Aim.IsPressed();
				inputSwitchDown = rpgInputs.RPGCharacter.WeaponDown.WasPressedThisFrame();
				inputSwitchUp = rpgInputs.RPGCharacter.WeaponUp.WasPressedThisFrame();

				// Injury toggle.
				if (Keyboard.current.iKey.wasPressedThisFrame) {
                    if (rpgCharacterController.CanStartAction("Injure")) {
                        rpgCharacterController.StartAction("Injure");
                    } else if (rpgCharacterController.CanEndAction("Injure")) {
                        rpgCharacterController.EndAction("Injure");
                    }
                }
                // Slow time toggle.
                if (Keyboard.current.tKey.wasPressedThisFrame) {
                    if (rpgCharacterController.CanStartAction("SlowTime")) {
                        rpgCharacterController.StartAction("SlowTime", 0.125f);
                    } else if (rpgCharacterController.CanEndAction("SlowTime")) {
                        rpgCharacterController.EndAction("SlowTime");
                    }
                }
                // Pause toggle.
                if (Keyboard.current.pKey.wasPressedThisFrame) {
                    if (rpgCharacterController.CanStartAction("SlowTime")) {
                        rpgCharacterController.StartAction("SlowTime", 0f);
                    } else if (rpgCharacterController.CanEndAction("SlowTime")) {
                        rpgCharacterController.EndAction("SlowTime");
                    }
                }
            } catch (System.Exception) { Debug.LogError("Inputs not found!  Character must have Player Input component."); }
        }

        public bool HasMoveInput()
        {
            return moveInput != Vector3.zero;
        }

		public bool HasAimInput()
		{
			return inputAim;
		}

        public bool HasBlockInput()
        {
            return inputBlock;
        }

        public void Moving()
        {
            moveInput = new Vector3(inputMovement.x, inputMovement.y, 0f);
            rpgCharacterController.SetMoveInput(moveInput);

            // Set the input on the jump axis every frame.
            Vector3 jumpInput = isJumpHeld ? Vector3.up : Vector3.zero;
            rpgCharacterController.SetJumpInput(jumpInput);

            // If we pressed jump button this frame, jump.
            if (inputJump && rpgCharacterController.CanStartAction("Jump")) {
                rpgCharacterController.StartAction("Jump");
            } else if (inputJump && rpgCharacterController.CanStartAction("DoubleJump")) {
                rpgCharacterController.StartAction("DoubleJump");
            }
        }

		public void Rolling()
		{
			if (!inputRoll) { return; }
			if (!rpgCharacterController.CanStartAction("DiveRoll")) { return; }

			rpgCharacterController.StartAction("DiveRoll", 1);
		}

		private void Strafing()
		{
			if (rpgCharacterController.canStrafe) {
				if (inputAim) {
					if (rpgCharacterController.CanStartAction("Strafe")) { rpgCharacterController.StartAction("Strafe"); }
				} else {
					if (rpgCharacterController.CanEndAction("Strafe")) { rpgCharacterController.EndAction("Strafe"); }
				}
			}
		}

		private void Attacking()
		{
			if (!rpgCharacterController.CanStartAction("Attack")) { return; }
			if (inputAttackL) {
				rpgCharacterController.StartAction("Attack", new Actions.AttackContext("Attack", "Left"));
			} else if (inputAttackR) {
				rpgCharacterController.StartAction("Attack", new Actions.AttackContext("Attack", "Right"));
			}
		}

		private void Damage()
		{
			// Hit.
			if (inputLightHit) { rpgCharacterController.StartAction("GetHit", new HitContext()); }

			// Death.
			if (inputDeath) {
				if (rpgCharacterController.CanStartAction("Death")) {
					rpgCharacterController.StartAction("Death");
				} else if (rpgCharacterController.CanEndAction("Death")) {
					rpgCharacterController.EndAction("Death");
				}
			}
		}

		/// <summary>
		/// Cycle weapons using directional pad input. Up and Down cycle forward and backward through
		/// the list of two handed weapons. Left cycles through the left hand weapons. Right cycles through
		/// the right hand weapons.
		/// </summary>
		private void SwitchWeapons()
        {
			// Bail out if we can't switch weapons.
			if (!rpgCharacterController.CanStartAction("SwitchWeapon")) { return; }

			bool doSwitch = false;
			SwitchWeaponContext context = new SwitchWeaponContext();
			int weaponNumber = 0;

			// Cycle through 2H weapons any input happens on the up-down axis.
			if (inputSwitchUp || inputSwitchDown) {
                int[] twoHandedWeapons = new int[] {
                    (int) Weapon.TwoHandSword,
                };

                // If we're not wielding 2H weapon already, just switch to the first one in the list.
                if (System.Array.IndexOf(twoHandedWeapons, rpgCharacterController.rightWeapon) == -1) {
                    weaponNumber = twoHandedWeapons[0];
                }
                // Otherwise, we should loop through them.
                else {
                    int index = System.Array.IndexOf(twoHandedWeapons, rpgCharacterController.rightWeapon);
                    if (inputSwitchUp) {
                        index = (index - 1 + twoHandedWeapons.Length) % twoHandedWeapons.Length;
                    } else if (inputSwitchDown) {
                        index = (index + 1) % twoHandedWeapons.Length;
                    }
                    weaponNumber = twoHandedWeapons[index];
                }

                // Set up the context and flag that we actually want to perform the switch.
                doSwitch = true;
                context.type = "Switch";
                context.side = "None";
                context.leftWeapon = -1;
                context.rightWeapon = weaponNumber;
            }

            // If we've received input, then "doSwitch" is true, and the context is filled out,
            // so start the SwitchWeapon action.
            if (doSwitch) { rpgCharacterController.StartAction("SwitchWeapon", context); }
        }
    }

	/// <summary>
	/// Extension Method to allow checking InputSystem without Action Callbacks.
	/// </summary>
	public static class InputActionExtensions
	{
		public static bool IsPressed(this InputAction inputAction)
		{
			return inputAction.ReadValue<float>() > 0f;
		}

		public static bool WasPressedThisFrame(this InputAction inputAction)
		{
			return inputAction.triggered && inputAction.ReadValue<float>() > 0f;
		}

		public static bool WasReleasedThisFrame(this InputAction inputAction)
		{
			return inputAction.triggered && inputAction.ReadValue<float>() == 0f;
		}
	}
}