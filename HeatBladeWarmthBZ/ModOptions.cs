using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace LyonicDevelopment.HeatBladeWarmthBZ
{
	[Menu("Heat Blade Warmth")]
	public class ModOptions : ConfigFile
	{

		[Slider("Text Size", 15, 70, DefaultValue = 32)]
		public int textSize = 32;

		[Toggle("Show Hint Text")]
		public bool showHintText = true;

	}
}
