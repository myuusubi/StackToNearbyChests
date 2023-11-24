using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace StackToNearbyChests
{
	abstract class Patch
	{
		/// <summary>
		/// Don't use typeof() or it won't work on Mac/Linux
		/// </summary>
		/// <returns></returns>
		public abstract Type GetTargetType();

		/// <summary>
		/// Null if constructor is desired
		/// </summary>
		public abstract string GetTargetMethodName();

		public abstract Type[] GetTargetMethodArguments();

		public void ApplyPatch(Harmony harmonyInstance)
		{
			MethodBase targetMethod = String.IsNullOrEmpty(GetTargetMethodName()) ?
				(MethodBase)GetTargetType().GetConstructor(GetTargetMethodArguments()) :
				targetMethod = GetTargetType().GetMethod(GetTargetMethodName(), GetTargetMethodArguments());

			var prefixMethod = GetType().GetMethod("Prefix");
			var postfixMethod = GetType().GetMethod("Postfix");

			HarmonyMethod prefixPatch = null;
			if (prefixMethod != null) {
				prefixPatch = new HarmonyMethod(prefixMethod);
			}

			HarmonyMethod postfixPatch = null;
			if (postfixMethod != null)
			{
				postfixPatch = new HarmonyMethod(postfixMethod);
			}

			harmonyInstance.Patch(targetMethod, prefixPatch, postfixPatch);
		}

		public static void PatchAll(Harmony harmonyInstance)
		{
			foreach (Type type in (from type in Assembly.GetExecutingAssembly().GetTypes()
								   where type.IsClass && type.BaseType == typeof(Patch)
								   select type))
				((Patch)Activator.CreateInstance(type)).ApplyPatch(harmonyInstance);
		}
	}
}
