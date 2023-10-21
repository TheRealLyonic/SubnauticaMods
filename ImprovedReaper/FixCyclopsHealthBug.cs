using HarmonyLib;
using UnityEngine;

namespace ImprovedReaper
{
	class FixCylopsHealthBug
	{
		[HarmonyPatch(typeof(CyclopsHelmHUDManager))]
		[HarmonyPatch("Update")]

		internal class Patch_Cyclops_Health
		{
			[HarmonyPostfix]
			public static void Postfix(CyclopsHelmHUDManager __instance)
			{
				FixCylopsHealthBug fix = new FixCylopsHealthBug();
				CyclopsHelmHUDManager hudManager = __instance;
				var cyclopsHealth = hudManager.subLiveMixin.health;

				//Health fraction and Helm_Hud hp bar fill code
				float healthFraction = hudManager.subLiveMixin.GetHealthFraction();
				hudManager.hpBar.fillAmount = Mathf.Lerp(hudManager.hpBar.fillAmount, healthFraction, Time.deltaTime * 2f);

				//Noise percent and Helm_Hud noise bar fill code
				float noisePercent = hudManager.noiseManager.GetNoisePercent();
				hudManager.noiseBar.fillAmount = Mathf.Lerp(hudManager.noiseBar.fillAmount, noisePercent, Time.deltaTime);

				//Destroy Cyclops Code
				fix.checkIfDead(hudManager, cyclopsHealth);
				
			}
		}

		public void checkIfDead(CyclopsHelmHUDManager hudManager, float cyclopsHealth)
		{
			if(cyclopsHealth == 0 && !hudManager.subRoot.subDestroyed)
			{
				hudManager.subRoot.subDestroyed = true;

				hudManager.subRoot.voiceNotificationManager.ClearQueue();
				hudManager.subRoot.voiceNotificationManager.PlayVoiceNotification(hudManager.subRoot.abandonShipNotification, false, true);

				hudManager.subRoot.Invoke("PowerDownCyclops", 13f);
				hudManager.subRoot.Invoke("DestroyCyclopsSubRoot", 18f);

				if(Vector3.Distance(hudManager.subRoot.transform.position, Player.main.transform.position) < 20f)
				{
					MainCameraControl.main.ShakeCamera(1.5f, 20f, MainCameraControl.ShakeMode.Linear, 1f);
				}
			}
		}
	}
}