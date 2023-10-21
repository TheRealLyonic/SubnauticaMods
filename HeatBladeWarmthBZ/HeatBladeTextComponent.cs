using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.HeatBladeWarmthBZ
{
	public class HeatBladeTextComponent : MonoBehaviour
	{

		/*
		Special thanks to Ramune and his "Simple Coordinates" project, that was used as a reference here for
		how to render text to the screen through Nautilus.
		*/

		public static BasicText hintText;
		public static bool textHidden;
		public static int textSize;

		public void Start()
		{
			textSize = HeatBladeWarmthPlugin.MOD_OPTIONS.textSize;

			hintText = new BasicText();
			UnhideText();

			hintText.SetAlign(TMPro.TextAlignmentOptions.Center);
			hintText.SetLocation(0f, -395f);
			hintText.SetColor(Color.red);
			hintText.SetSize(textSize);
			
			HideText();
		}

		public static void HideText()
		{
			hintText.Hide();
			textHidden = true;
		}

		public static void UnhideText()
		{
			hintText.ShowMessage("Warm Up (G)");
			textHidden = false;
		}

	}
}
