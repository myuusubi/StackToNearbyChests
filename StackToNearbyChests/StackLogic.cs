﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Objects;

namespace StackToNearbyChests
{
	static class StackLogic
	{
		internal static void StackToNearbyChests(int radius)
		{

			StardewValley.Farmer farmer = Game1.player;

			foreach (Chest chest in GetChestsAroundFarmer(farmer, radius))
			{
				List<Item> itemsToRemoveFromPlayer = new List<Item>();
				bool movedAtLeastOne = false;

				//Find remaining stack size of CHEST item. check if player has the item, then remove as much as possible
				//need to compare quality
				foreach (Item chestItem in chest.Items)
				{
					if (chestItem != null)
					{
						foreach (Item playerItem in farmer.Items)
						{
							if (playerItem != null)
							{
								int remainingStackSize = chestItem.getRemainingStackSpace();
								if (!(itemsToRemoveFromPlayer.Contains(playerItem)) && playerItem.canStackWith(chestItem))
								{
									movedAtLeastOne = true;
									int amountToRemove = Math.Min(remainingStackSize, playerItem.Stack);
									chestItem.Stack += amountToRemove;

									if (playerItem.Stack > amountToRemove)
									{
										playerItem.Stack -= amountToRemove;
									}
									else
									{
										itemsToRemoveFromPlayer.Add(playerItem);
									}
								}
							}
						}
					}
				}

				foreach (Item removeItem in itemsToRemoveFromPlayer)
					farmer.removeItemFromInventory(removeItem);



				//List of sounds: https://gist.github.com/gasolinewaltz/46b1473415d412e220a21cb84dd9aad6
				if (movedAtLeastOne)
					Game1.playSound(Game1.soundBank.GetCue("pickUpItem").Name);

			}



		}

		private static IEnumerable<Chest> GetChestsAroundFarmer(StardewValley.Farmer farmer, int radius)
		{
			Vector2 farmerLocation = farmer.getStandingPosition();
			farmerLocation = new Vector2(farmerLocation.X / 64, farmerLocation.Y / 64);

			//Normal object chests
			for (int dx = -radius; dx <= radius; dx++)
			{
				for (int dy = -radius; dy <= radius; dy++)
				{
					Vector2 checkLocation = Game1.tileSize * (farmerLocation + new Vector2(dx, dy));
					StardewValley.Object blockObject = farmer.currentLocation.getObjectAt((int)checkLocation.X, (int)checkLocation.Y);

					if (blockObject is Chest)
					{
						Chest chest = blockObject as Chest;
						yield return chest;
					}
				}
			}

			//Fridge
			if (farmer?.currentLocation is FarmHouse farmHouse && farmHouse.upgradeLevel >= 1) //Lvl 1,2,3 is where you have fridge upgrade
			{
				Point fridgeLocation = farmHouse.getKitchenStandingSpot();
				fridgeLocation.X += 2; fridgeLocation.Y += -1; //Fridge spot relative to kitchen spot

				if (Math.Abs(farmerLocation.X - fridgeLocation.X) <= radius && Math.Abs(farmerLocation.Y - fridgeLocation.Y) <= radius)
				{
					if (farmHouse.fridge.Value != null)
						yield return farmHouse.fridge.Value;
					else
						Console.WriteLine("StackToNearbyChests: could not find fridge!");
				}
			}

			//Mills and Junimo Huts
			if (farmer.currentLocation != null) {
				foreach (Building building in farmer.currentLocation.buildings) {
					if (Math.Abs(building.tileX.Value - farmerLocation.X) <= radius && Math.Abs(building.tileY.Value - farmerLocation.Y) <= radius) {
						Chest chest = building.GetBuildingChest("Output");
						if (chest != null) {
							yield return chest;
						}
					}
				}
			}
		}
	}
}
