using HarmonyLib;
using System;
using UnityEngine;

namespace LyonicDevelopment.HeatBladeWarmthBZ
{
	[HarmonyPatch(typeof(Player))]
	internal class PlayerPatch
	{
		private static BodyTemperature tempMgr;
		private static float heatValue = 60f;
		private static float damageValue = 10f;
		private static float cooldownTime = 130f; //Measured in seconds, value should be above 100 so the player can still freeze to death.
		private static float rechargedTime = 0f;
		private static float animationTimer = 2.5f;
		private static float animationEnd = 0f;
		private static bool waitingForAnim = false;

		[HarmonyPatch(nameof(Player.Start))]
		[HarmonyPostfix]
		public static void Start_Postfix()
		{
			Player.main.gameObject.EnsureComponent<HeatBladeTextComponent>();
			tempMgr = GameObject.FindObjectOfType<BodyTemperature>();
			rechargedTime = 0f;
			animationEnd = 0f;
		}


		[HarmonyPatch(nameof(Player.Update))]
		[HarmonyPostfix]
		public static void Update_Postfix()
		{
			if(HeatBladeTextComponent.textSize != HeatBladeWarmthPlugin.MOD_OPTIONS.textSize)
			{
				HeatBladeTextComponent.textSize = HeatBladeWarmthPlugin.MOD_OPTIONS.textSize;
				HeatBladeTextComponent.hintText.SetSize(HeatBladeTextComponent.textSize);
			}

			if(Inventory.main.GetHeldTool() && Inventory.main.GetHeldTool().GetType() == typeof(HeatBlade) && Time.time >= rechargedTime && tempMgr.isExposed)
			{
				if (HeatBladeTextComponent.textHidden && HeatBladeWarmthPlugin.MOD_OPTIONS.showHintText)
				{
					HeatBladeTextComponent.UnhideText();
				}else if(!HeatBladeTextComponent.textHidden && !HeatBladeWarmthPlugin.MOD_OPTIONS.showHintText)
				{
					HeatBladeTextComponent.HideText();
				}
				
				if (Input.GetKeyDown(KeyCode.G))
				{
					/*
					Plays the first-use animation for the knife, shows the player using the heat blade to warm themselves up.
					Special thanks to Eldritch for providing help with animation playing.
					*/
					Player.main.playerAnimator.SetBool("using_tool_first", true);

					HeatBladeTextComponent.HideText();

					animationEnd = Time.time + animationTimer;
					rechargedTime = Time.time + cooldownTime;
					waitingForAnim = true;
				}
			}
			else if (!HeatBladeTextComponent.textHidden)
			{
				HeatBladeTextComponent.HideText();
			}
		}

		[HarmonyPatch(nameof(Player.LateUpdate))]
		[HarmonyPostfix]
		public static void LateUpdate_Postfix()
		{
			try
			{
				if (waitingForAnim)
				{
					Player.main.pda.SetIgnorePDAInput(true);

					if (Time.time >= animationEnd)
					{
						waitingForAnim = false;
						tempMgr.AddCold(heatValue * -1);
						Player.main.liveMixin.TakeDamage(damageValue, Player.main.transform.position, DamageType.Heat);
						Player.main.liveMixin.damageClip.Play();
					}

				}
				else if (Inventory.main.GetHeldTool() && Inventory.main.GetHeldTool().GetType() == typeof(HeatBlade) && Player.main.pda.ignorePDAInput)
				{
					Player.main.playerAnimator.SetBool("using_tool_first", false);

					Player.main.AddUsedTool(TechType.Knife);
					Player.main.AddUsedTool(TechType.HeatBlade);

					Inventory.main.GetHeldTool().hasFirstUseAnimation = false;

					Player.main.pda.SetIgnorePDAInput(false);
				}
			}
			catch(NullReferenceException)
			{
				/*
				Why do we need this catch here? I have no clue..But the console throws a bunch of NullReferenceExceptions otherwise...What could be null??
				Error only gets thrown when G is pressed, but everything works as it should, and I can't track down what's causing it, so I'm just going to do this to
				suppress the console's error logging for the time being.....Gee, I'm such a great programmer.
				*/
				return;
			}
		}

	}
}
