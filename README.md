StackToNearbyChests
===
A fork of the StackToNearbyChests mod, updated for the latest version of Stardew Valley.

The mod, as the name implies, allows the player to click a button in their inventory that
will automatically sort their items into the corresponding nearby chests. This is helpful
for multiplayer playthroughs, where it's harder to keep a good organization system.

#### Our Changes

As per the terms of GPL 3.0, we have to state our changes. They are as follows:
- Changed `using Harmony` to `using HarmonyLib` for the new Harmony version
- We use the specific `helper.ModContent` helper instead of `helper.Content`
- Updated the `icon.png` file to our own artwork, which better fits the game's style
- Updated the logic for finding nearby chests to account for changes to Stardew Valley
