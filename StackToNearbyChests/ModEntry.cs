using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StackToNearbyChests
{
	/// <summary>The mod entry class loaded by SMAPI.</summary>
	class ModEntry : Mod
	{
		internal static ModConfig Config { get; private set; }

		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			Config = helper.ReadConfig<ModConfig>();
			ButtonHolder.ButtonIcon = helper.Content.Load<Texture2D>(@"icon.png");

			Patch.PatchAll(new Harmony("me.ilyaki.StackToNearbyChests"));
		}
	}
}
