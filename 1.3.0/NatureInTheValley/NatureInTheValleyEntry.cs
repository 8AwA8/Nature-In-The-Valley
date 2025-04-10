using System;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Audio;
using StardewValley.BellsAndWhistles;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Shops;
using StardewValley.GameData.Tools;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.Triggers;
using xTile.Dimensions;
using xTile.Layers;

namespace NatureInTheValley
{
	// Token: 0x02000004 RID: 4
	internal sealed class NatureInTheValleyEntry : Mod
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002FE8 File Offset: 0x000011E8
		public override void Entry(IModHelper helper)
		{
			this.helper = helper;
			NatInValleyConfig natInValleyConfig = helper.ReadConfig<NatInValleyConfig>();
			this.config = natInValleyConfig;
			NatureInTheValleyEntry.staticConfig = natInValleyConfig;
			NatureInTheValleyEntry.staticHelper = helper;
			if (!this.config.useOnlyContentPacks)
			{
				foreach (KeyValuePair<string, string> keyValuePair in helper.ModContent.Load<Dictionary<string, string>>("MainData.json"))
				{
					this.creatureData.TryAdd(keyValuePair.Key, new List<string>(keyValuePair.Value.Split('/', StringSplitOptions.None)));
				}
			}
			IList<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			IList<Dictionary<string, RewardModel>> list2 = new List<Dictionary<string, RewardModel>>();
			NatureInTheValleyEntry.customRewards.Clear();
			foreach (IContentPack contentPack in base.Helper.ContentPacks.GetOwned())
			{
				if (contentPack.HasFile("content.json"))
				{
					list.Add(contentPack.ReadJsonFile<Dictionary<string, string>>("content.json"));
				}
				if (contentPack.HasFile("reward.json"))
				{
					list2.Add(contentPack.ReadJsonFile<Dictionary<string, RewardModel>>("reward.json"));
				}
			}
			foreach (Dictionary<string, string> dictionary in list)
			{
				foreach (KeyValuePair<string, string> keyValuePair2 in dictionary)
				{
					if (!this.creatureData.ContainsKey(keyValuePair2.Key))
					{
						this.creatureData.TryAdd(keyValuePair2.Key, new List<string>(keyValuePair2.Value.Split('/', StringSplitOptions.None)));
					}
					else
					{
						this.creatureData[keyValuePair2.Key] = new List<string>(keyValuePair2.Value.Split('/', StringSplitOptions.None));
					}
				}
			}
			foreach (Dictionary<string, RewardModel> dictionary2 in list2)
			{
				foreach (KeyValuePair<string, RewardModel> keyValuePair3 in dictionary2)
				{
					if (!NatureInTheValleyEntry.customRewards.ContainsKey(keyValuePair3.Key))
					{
						NatureInTheValleyEntry.customRewards.TryAdd(keyValuePair3.Key, keyValuePair3.Value);
					}
				}
			}
			NatureInTheValleyEntry.staticCreatureData = this.creatureData;
			helper.Events.Player.Warped += this.OnWarp;
			helper.Events.GameLoop.TimeChanged += this.TimeChange;
			helper.Events.Input.ButtonPressed += this.Pressed;
			helper.Events.GameLoop.OneSecondUpdateTicked += this.OneSecond;
			helper.Events.GameLoop.GameLaunched += this.OnLaunch;
			helper.Events.Display.MenuChanged += this.MenuChanged;
			helper.Events.World.LargeTerrainFeatureListChanged += this.RemovedLargeTerrain;
			helper.Events.Content.AssetsInvalidated += this.OnAssetsInvalidated;
			helper.Events.GameLoop.DayStarted += this.DayStarted;
			helper.Events.Display.RenderedWorld += this.renderPostWorld;
			helper.Events.Content.AssetRequested += this.OnAssetRequested;
			NatureInTheValleyEntry.netTexture = helper.ModContent.Load<Texture2D>("PNGs\\NormalNet");
			Harmony harmony = new Harmony("Nature.NatureInTheValley");
			GameLocation.RegisterTileAction("NatInValley_ReturnHome", new Func<GameLocation, string[], Farmer, Point, bool>(this.ReturnHome));
			GameLocation.RegisterTileAction("NatWarpInsect", new Func<GameLocation, string[], Farmer, Point, bool>(this.StartBossFadeHandle));
			GameLocation.RegisterTileAction("NIVOpenDonate", new Func<GameLocation, string[], Farmer, Point, bool>(this.OpenDonationWindow));
			helper.ConsoleCommands.Add("NatCreat_Summon", "Summons a creature.\n\nUsage: NatCreat_Summon <value>\n- value: the name.", new Action<string, string[]>(this.Command));
			helper.ConsoleCommands.Add("NatCreat_FillMuseum", "Summons a creature.\n\nUsage: NatCreat_Summon <value>\n- value: the name.", new Action<string, string[]>(this.CommandTwo));
			helper.ConsoleCommands.Add("NatCreat_EmptyMuseum", "Summons a creature.\n\nUsage: NatCreat_Summon <value>\n- value: the name.", new Action<string, string[]>(this.CommandThree));
			helper.ConsoleCommands.Add("NatCreat_List", "Summons a creature.\n\nUsage: NatCreat_Summon <value>\n- value: the name.", new Action<string, string[]>(this.CommandFour));
			helper.ConsoleCommands.Add("NatCreat_Possible", "Summons a creature.\n\nUsage: NatCreat_Summon <value>\n- value: the name.", new Action<string, string[]>(this.CommandFive));
			harmony.Patch(AccessTools.Method(typeof(GameLocation), "draw", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "CreatureDrawer", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(Game1), "_update", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "Ticking", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(GameLocation), "damageMonster", new Type[]
			{
				typeof(Microsoft.Xna.Framework.Rectangle),
				typeof(int),
				typeof(int),
				typeof(bool),
				typeof(float),
				typeof(int),
				typeof(float),
				typeof(float),
				typeof(bool),
				typeof(Farmer),
				typeof(bool)
			}, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "damageCreeps", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(GameLocation), "answerDialogue", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "PostTextFunction", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(BusStop), "checkAction", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "CoverCheckAction", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(Tree), "performTreeFall", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "TreeFell", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(InventoryPage), "receiveLeftClick", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "creatureMakerDrop", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(Furniture), "checkForAction", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "actionCover", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(FishingRod), "canThisBeAttached", new Type[]
			{
				typeof(StardewValley.Object),
				typeof(int)
			}, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "baitFix", null), null, null, null);
			if (helper.ModRegistry.IsLoaded("malic.cp.jadeNPC"))
			{
				harmony.Patch(AccessTools.Method(typeof(NPC), "checkAction", null, null), null, new HarmonyMethod(typeof(NatureInTheValleyEntry), "NPC_checkAction", null), null, null);
			}
			harmony.Patch(AccessTools.Method(typeof(StardewValley.Object), "getDescription", null, null), null, new HarmonyMethod(typeof(NatureInTheValleyEntry), "descriptCover", null), null, null);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000037F8 File Offset: 0x000019F8
		public NatureInTheValleyEntry()
		{
			this.locationalData = new Dictionary<string, List<string>>();
			this.creatureData = new Dictionary<string, List<string>>();
			this.desertLocationNames = new HashSet<string>
			{
				"Desert",
				"SkullCave",
				"DesertFestival"
			};
			this.forestLocationNames = new HashSet<string>
			{
				"SecretWoods",
				"Forest",
				"Backwoods",
				"Woods",
				"Mountain",
				"Farm_Forest",
				"NIVOuterInsec"
			};
			this.waterLocationNames = new HashSet<string>
			{
				"Beach",
				"BeachNightMarket",
				"IslandWest",
				"IslandSouth",
				"IslandSouthEast",
				"IslandSouthEastCave",
				"Farm_Beach"
			};
			this.underLocationNames = new HashSet<string>
			{
				"FarmCave",
				"Mine",
				"UndergroundMine",
				"BugLand",
				"WitchWarpCave",
				"SkullCave",
				"WitchSwamp",
				"MasteryCave",
				"IslandSoutheastCave"
			};
			this.treePos = new List<Vector2>();
			this.waterPos = new List<Vector2>();
			this.layersToSave = new List<Layer>();
			this.bushPos = new List<Vector2>();
			this.stumpPos = new List<Vector2>();
			this.dailyMod = 1f;
			this.possibleDailyMods = new List<float>
			{
				0.8f,
				0.8f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1.25f,
				1.25f,
				1.5f,
				1.5f
			};
			this.locationCap = 1;
			this.spawnChance = 1.0;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00003A84 File Offset: 0x00001C84
		public void OnWarp(object sender, WarpedEventArgs eventArgs)
		{
			if (eventArgs.NewLocation != null && eventArgs.NewLocation.Name != null)
			{
				if (eventArgs.NewLocation.Name == "BusStop")
				{
					this.setUpPositionalWarp(eventArgs.NewLocation);
				}
				if (eventArgs.NewLocation.Name == "NIVInnerInsec")
				{
					this.setUpInsectarium(eventArgs.NewLocation);
				}
				this.treePos = this.GetTrees(eventArgs.NewLocation);
				this.waterPos = this.GetWater(eventArgs.NewLocation);
				this.bushPos = this.GetBushes(eventArgs.NewLocation);
				this.stumpPos = this.GetStumps(eventArgs.NewLocation);
				if (eventArgs.NewLocation.map.GetLayer("Back") != null)
				{
					this.Tiles = eventArgs.NewLocation.map.GetLayer("Back").Tiles.Array.Length;
				}
				else
				{
					this.Tiles = 10;
				}
				this.locationCap = (int)((double)((float)(2 + this.Tiles / 1800)) * (double)Game1.numberOfPlayers() * (double)this.config.maxcreaturelLimitMultiplier * (double)this.dailyMod);
				this.spawnChance = 0.22 * (double)this.config.spawnRateMultiplier * this.func(this.Tiles) * 1.45 * (double)this.dailyMod;
				this.locationalData = this.CreaturesForArea(eventArgs.NewLocation);
				this.MakeCreatueSpawnList();
				this.spawnedInLoc = 0;
				eventArgs.NewLocation.resourceClumps.OnValueRemoved -= new NetCollection<ResourceClump>.ContentsChangeEvent(this.RemovedResource);
				eventArgs.NewLocation.resourceClumps.OnValueRemoved += new NetCollection<ResourceClump>.ContentsChangeEvent(this.RemovedResource);
			}
			if (eventArgs.OldLocation != null)
			{
				eventArgs.OldLocation.resourceClumps.OnValueRemoved -= new NetCollection<ResourceClump>.ContentsChangeEvent(this.RemovedResource);
				this.ClearUnclaimedCreatures(eventArgs.NewLocation);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00003C7C File Offset: 0x00001E7C
		private static void CreatureDrawer(GameLocation __instance, SpriteBatch b)
		{
			foreach (NatCreature natCreature in NatureInTheValleyEntry.creatures)
			{
				if (natCreature.GetLocation() != null && natCreature.GetLocation().Name == __instance.Name)
				{
					natCreature.Draw(b);
					natCreature.DrawShadow(b);
				}
			}
			Farmer player = Game1.player;
			if (player.ActiveItem == null || !(player.ActiveItem is Tool))
			{
				return;
			}
			if (player.ActiveItem is NatInValeyNet)
			{
				Texture2D texture = NatureInTheValleyEntry.netTexture;
				if ((player.ActiveItem as NatInValeyNet).isHeld)
				{
					NatureInTheValleyEntry.AnimatedBump.Value = 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(1f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 1:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-50f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.Hair, false));
						return;
					case 3:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-22f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 10)
				{
					PerScreen<int> animatedBump = NatureInTheValleyEntry.AnimatedBump;
					int value = animatedBump.Value;
					animatedBump.Value = value + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(0f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-85f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 20)
				{
					PerScreen<int> animatedBump2 = NatureInTheValleyEntry.AnimatedBump;
					int value2 = animatedBump2.Value;
					animatedBump2.Value = value2 + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(30f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture, Game1.GlobalToLocal(player.Position + new Vector2(-100f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
			}
			else if (player.ActiveItem is NatInValeyGoldenNet)
			{
				Texture2D texture2 = NatInValeyGoldenNet.texture;
				if ((player.ActiveItem as NatInValeyGoldenNet).isHeld)
				{
					NatureInTheValleyEntry.AnimatedBump.Value = 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(1f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 1:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-50f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.Hair, false));
						return;
					case 3:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-22f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 10)
				{
					PerScreen<int> animatedBump3 = NatureInTheValleyEntry.AnimatedBump;
					int value3 = animatedBump3.Value;
					animatedBump3.Value = value3 + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(0f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-85f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 20)
				{
					PerScreen<int> animatedBump4 = NatureInTheValleyEntry.AnimatedBump;
					int value4 = animatedBump4.Value;
					animatedBump4.Value = value4 + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(30f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture2, Game1.GlobalToLocal(player.Position + new Vector2(-100f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
			}
			else if (player.ActiveItem is NatInValeyJadeNet)
			{
				Texture2D texture3 = NatInValeyJadeNet.texture;
				if ((player.ActiveItem as NatInValeyJadeNet).isHeld)
				{
					NatureInTheValleyEntry.AnimatedBump.Value = 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(1f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 1:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-50f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.Hair, false));
						return;
					case 3:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-22f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 10)
				{
					PerScreen<int> animatedBump5 = NatureInTheValleyEntry.AnimatedBump;
					int value5 = animatedBump5.Value;
					animatedBump5.Value = value5 + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(0f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-85f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 20)
				{
					PerScreen<int> animatedBump6 = NatureInTheValleyEntry.AnimatedBump;
					int value6 = animatedBump6.Value;
					animatedBump6.Value = value6 + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(30f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture3, Game1.GlobalToLocal(player.Position + new Vector2(-100f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
			}
			else if (player.ActiveItem is NatInValeySaphNet)
			{
				Texture2D texture4 = NatInValeySaphNet.texture;
				if ((player.ActiveItem as NatInValeySaphNet).isHeld)
				{
					NatureInTheValleyEntry.AnimatedBump.Value = 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(1f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 1:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-50f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.Hair, false));
						return;
					case 3:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-22f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 10)
				{
					PerScreen<int> animatedBump7 = NatureInTheValleyEntry.AnimatedBump;
					int value7 = animatedBump7.Value;
					animatedBump7.Value = value7 + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(0f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-85f, -130f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.2f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
				else if (NatureInTheValleyEntry.AnimatedBump.Value > 0 && NatureInTheValleyEntry.AnimatedBump.Value < 20)
				{
					PerScreen<int> animatedBump8 = NatureInTheValleyEntry.AnimatedBump;
					int value8 = animatedBump8.Value;
					animatedBump8.Value = value8 + 1;
					switch (player.FacingDirection)
					{
					case 0:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-20f, -120f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 96, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.ToolUp, false));
						return;
					case 1:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(30f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 64, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					case 2:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-59f, -105f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer() - 1E-06f, FarmerRenderer.FarmerSpriteLayers.ToolDown, false));
						return;
					case 3:
						b.Draw(texture4, Game1.GlobalToLocal(player.Position + new Vector2(-100f, -110f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(64, 128, 32, 32)), Color.White, 0f, Vector2.Zero, 4.4f, SpriteEffects.None, FarmerRenderer.GetLayerDepth(player.getDrawLayer(), FarmerRenderer.FarmerSpriteLayers.Tool, false));
						return;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002050 File Offset: 0x00000250
		static NatureInTheValleyEntry()
		{
			NatureInTheValleyEntry.customRewards = new Dictionary<string, RewardModel>();
			NatureInTheValleyEntry.AnimatedBump = new PerScreen<int>();
			NatureInTheValleyEntry.staticCreatureData = new Dictionary<string, List<string>>();
			NatureInTheValleyEntry.creatures = new List<NatCreature>();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000052DC File Offset: 0x000034DC
		public void TimeChange(object sender, TimeChangedEventArgs e)
		{
			if (Game1.currentLocation == null || Game1.currentLocation.Name == "NIVInnerInsec")
			{
				return;
			}
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (NatureInTheValleyEntry.creatures[i].TimeChange())
				{
					NatureInTheValleyEntry.creatures.RemoveAt(i);
					i--;
				}
			}
			this.Tiles = Game1.currentLocation.map.Layers[0].Tiles.Array.Length;
			this.locationCap = (int)((double)((float)(2 + this.Tiles / 1800)) * (double)Game1.numberOfPlayers() * (double)this.config.maxcreaturelLimitMultiplier * (double)this.dailyMod);
			this.spawnChance = 0.22 * (double)this.config.spawnRateMultiplier * this.func(this.Tiles) * 1.45 * (double)this.dailyMod;
			this.locationalData = this.CreaturesForArea(Game1.currentLocation);
			this.MakeCreatueSpawnList();
			this.spawnedInLoc = 0;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000053F4 File Offset: 0x000035F4
		public void Instantiate(string name, Vector2 tile, GameLocation location)
		{
			NatureInTheValleyEntry.creatures.Add(new NatCreature(tile * 64f, location, name, this.ModelData[name], this.config.creatureSizeMultiplier));
			Item item = ItemRegistry.Create("NatInValley.Creature." + name, 1, 0, false);
			item.modData["NAT_NIV_NAME"] = name;
			item.modData["NAT_NIV_TILEX"] = string.Format("{0}", tile.X);
			item.modData["NAT_NIV_TILEY"] = string.Format("{0}", tile.Y);
			TriggerActionManager.Raise("NAT_NIV_Spawned", new object[]
			{
				name,
				tile.X,
				tile.Y
			}, null, null, item, null);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002084 File Offset: 0x00000284
		public void SpawnCreatureInLocation(GameLocation location)
		{
			this.TrySpawnFromArea(location);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000054D8 File Offset: 0x000036D8
		public Dictionary<string, List<string>> CreaturesForArea(GameLocation location)
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			string currentSeason = Game1.currentSeason;
			int timeOfDay = Game1.timeOfDay;
			bool flag = location.IsRainingHere();
			bool flag2 = location.IsLightningHere();
			bool flag3 = location.IsSnowingHere();
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				List<string> value = keyValuePair.Value;
				bool flag4 = false;
				foreach (string code in value[11].Split(",", StringSplitOptions.None))
				{
					if (this.validLocation(location, code))
					{
						flag4 = true;
						break;
					}
				}
				bool flag5 = true;
				bool flag6 = (timeOfDay <= int.Parse(value[13]) && timeOfDay >= int.Parse(value[12])) || (this.ModelData[keyValuePair.Key].secondStartTime <= timeOfDay && this.ModelData[keyValuePair.Key].secondEndTime >= timeOfDay);
				if (value.Count > 28 && value[28] != "true" && value[28] != "false" && value[28] != " ")
				{
					if (!GameStateQuery.CheckConditions(value[28], null, null, null, null, null, null))
					{
						flag5 = false;
					}
				}
				else if ((value[0] == "3" && (float)this.donatedCount / (float)this.creatureData.Count < 0.25f) || (value[0] == "4" && (float)this.donatedCount / (float)this.creatureData.Count < 0.5f))
				{
					flag5 = false;
				}
				if (flag5 && flag4 && new List<string>(value[9].Split(",", StringSplitOptions.None)).Contains(currentSeason) && flag6 && ((!flag && !flag2 && !flag3) || value[10] != "1") && (flag || flag2 || flag3 || value[10] != "2") && (flag2 || value[10] != "3") && (flag3 || value[10] != "4"))
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return dictionary;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00005790 File Offset: 0x00003990
		public List<Vector2> GetTrees(GameLocation location)
		{
			List<Vector2> list = new List<Vector2>();
			using (NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>.ValuesCollection.Enumerator enumerator = location.terrainFeatures.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Tree && (enumerator.Current as Tree).growthStage.Value >= 5 && !(enumerator.Current as Tree).stump.Value && (enumerator.Current as Tree).isActionable())
					{
						list.Add(enumerator.Current.Tile);
					}
				}
			}
			return list;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00005848 File Offset: 0x00003A48
		public List<Vector2> GetBushes(GameLocation location)
		{
			List<Vector2> list = new List<Vector2>();
			foreach (LargeTerrainFeature largeTerrainFeature in location.largeTerrainFeatures)
			{
				if (largeTerrainFeature is Bush)
				{
					list.Add(largeTerrainFeature.Tile + new Vector2(-0.06f, 0.75f));
				}
			}
			return list;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000058C4 File Offset: 0x00003AC4
		public List<Vector2> GetStumps(GameLocation location)
		{
			List<Vector2> list = new List<Vector2>();
			foreach (ResourceClump resourceClump in location.resourceClumps)
			{
				if (resourceClump.parentSheetIndex.Value == 600)
				{
					list.Add(resourceClump.Tile + new Vector2(0.25f, 1.4f));
				}
			}
			return list;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000594C File Offset: 0x00003B4C
		public List<Vector2> GetWater(GameLocation location)
		{
			List<Vector2> list = new List<Vector2>();
			if (location.waterTiles == null || location.waterTiles.waterTiles == null)
			{
				return list;
			}
			for (int i = 0; i < location.map.Layers[0].LayerWidth; i++)
			{
				for (int j = 0; j < location.map.Layers[0].LayerHeight; j++)
				{
					if (location.waterTiles.waterTiles[i, j].isWater && location.waterTiles.waterTiles[i, j].isVisible && location.isOpenWater(i, j))
					{
						list.Add(new Vector2((float)i, (float)j));
					}
				}
			}
			return list;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00005A08 File Offset: 0x00003C08
		private bool CheckForBugAtTile(GameLocation location, Vector2 pos, float distance)
		{
			return NatureInTheValleyEntry.creatures.Any((NatCreature creature) => Vector2.Distance(pos, creature.Position) <= distance);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00005A40 File Offset: 0x00003C40
		public void RemovedResource(object sender)
		{
			this.stumpPos = this.GetStumps(Game1.currentLocation);
			List<Vector2> list = this.stumpPos;
			foreach (NatCreature natCreature in NatureInTheValleyEntry.creatures)
			{
				if (natCreature.LocalLocationCode == 4 && !natCreature.isStatic && natCreature.GetLocation().Name == Game1.currentLocation.Name)
				{
					Vector2 value = natCreature.Position / 64f;
					bool flag = false;
					foreach (Vector2 value2 in list)
					{
						if (Vector2.Distance(value, value2) < 1.5f)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						natCreature.isRunning = true;
					}
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00005B44 File Offset: 0x00003D44
		private void setUpPositionalWarp(GameLocation a)
		{
			if (!base.Helper.ModRegistry.IsLoaded("hootless.BusLocations") || !base.Helper.ModRegistry.IsLoaded("Nature.NIVBL"))
			{
				if (base.Helper.ModRegistry.IsLoaded("Pathoschild.CentralStation") || base.Helper.ModRegistry.IsLoaded("Cherry.TrainStationn"))
				{
					return;
				}
				a.setMapTile(28, 10, 1032, "Front", "outdoors", null, false);
				a.setMapTile(28, 11, 1057, "Buildings", "outdoors", null, false);
				if (!a.overlayObjects.ContainsKey(new Vector2(28f, 12f)))
				{
					if (a.overlayObjects.ContainsKey(new Vector2(28f, 11f)))
					{
						a.overlayObjects.Remove(new Vector2(28f, 11f));
					}
					a.overlayObjects.Add(new Vector2(28f, 11f), new StardewValley.Object("NatInValley.Creature.AtlasMoth", 1, false, -1, 0));
				}
				a.setTileProperty(28, 11, "Buildings", "Action", "None");
				a.setTileProperty(28, 11, "Buildings", "Action", "NatWarpInsect");
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00005C98 File Offset: 0x00003E98
		private bool StartBossFadeHandle(GameLocation location, string[] args, Farmer player, Point point)
		{
			if (Game1.player.Money < 100)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_NotEnoughMoneyForTicket"));
			}
			else
			{
				location.createQuestionDialogue(this.helper.Translation.Get("BusStop_InsectariumTravel"), location.createYesNoResponses(), "NatInsectarium");
			}
			return true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00005CF8 File Offset: 0x00003EF8
		private static void PostTextFunction(GameLocation __instance, Response answer)
		{
			if (__instance.lastQuestionKey == null || __instance.afterQuestion != null)
			{
				return;
			}
			if (ArgUtility.SplitBySpaceAndGet(__instance.lastQuestionKey, 0, null) + "_" + answer.responseKey == "NatInsectarium_Yes" && Game1.player.Money >= 100)
			{
				Game1.player.Money -= 75;
				Game1.warpFarmer("NIVOuterInsec", 24, 39, 0);
				return;
			}
			if (__instance.lastQuestionKey.Contains("TeraRelease_") && answer.responseKey == "Yes")
			{
				Vector2 vector = new Vector2(float.Parse(__instance.lastQuestionKey.Split("_", StringSplitOptions.None)[1]), float.Parse(__instance.lastQuestionKey.Split("_", StringSplitOptions.None)[2]));
				__instance.furniture.Remove(Game1.currentLocation.GetFurnitureAt(vector));
				List<string> list = NatureInTheValleyEntry.staticCreatureData[__instance.lastQuestionKey.Split("_", StringSplitOptions.None)[3]];
				NatCreature natCreature = new NatCreature(vector * 64f, __instance, __instance.lastQuestionKey.Split("_", StringSplitOptions.None)[3], int.Parse(list[0]), list[1] == "true", float.Parse(list[2]), int.Parse(list[3]), float.Parse(list[4]), list[5] == "true", list[6] == "true", int.Parse(list[7]), list[8] == "true", int.Parse(list[12]), int.Parse(list[13]), int.Parse(list[14]), list[15], int.Parse(list[16]), int.Parse(list[17]), int.Parse(list[18]), true, (list.Count > 26) ? int.Parse(list[26]) : 32, (list.Count > 27) ? int.Parse(list[27]) : 32, list[list.Count - 1] == "true", list.Count > 31 && list[30] == "true", list.Count > 32 && list[31] == "true", list.Count > 33 && list[32] == "true", (list.Count > 34) ? int.Parse(list[33]) : 25, (list.Count > 35) ? int.Parse(list[34]) : 0, (list.Count > 37) ? list[36] : "", NatureInTheValleyEntry.staticConfig.creatureSizeMultiplier, (list.Count > 38) ? new List<string>(list[37].Split(",", StringSplitOptions.None)) : new List<string>(), (list.Count > 39) ? float.Parse(list[38]) : 0.1f, list.Count > 45 && list[44] == "true", (list.Count > 46) ? float.Parse(list[45]) : 500f, (list.Count > 47) ? int.Parse(list[46]) : 1150, list.Count > 48 && list[47] == "true", (list.Count > 53) ? new List<string>(list[52].Split(",", StringSplitOptions.None)) : new List<string>());
				natCreature.released = true;
				NatureInTheValleyEntry.creatures.Add(natCreature);
				return;
			}
			if (__instance.lastQuestionKey == "NatDesk")
			{
				if (answer.responseKey == "NIVFResponse")
				{
					if (Constants.TargetPlatform == GamePlatform.Android)
					{
						Game1.activeClickableMenu = new MobileCreatureDonationMenu();
						return;
					}
					Game1.activeClickableMenu = new CreatureDonationMenu();
					return;
				}
				else
				{
					if (answer.responseKey == "NIVSResponse")
					{
						Game1.activeClickableMenu = new ClickIntoCreatureInfoMenu();
					}
					if (answer.responseKey == "NIVTResponse")
					{
						Game1.activeClickableMenu = new CreatureTerrariumMenu();
					}
					if (answer.responseKey == "NIVRResponse")
					{
						Game1.activeClickableMenu = new ItemGrabMenu(NatureInTheValleyEntry.getRewardsForPlayer(Game1.player), false, true, new InventoryMenu.highlightThisItem(NatureInTheValleyEntry.HighlightCollectableRewards), null, "Rewards", new ItemGrabMenu.behaviorOnItemSelect(NatureInTheValleyEntry.OnRewardCollected), false, true, false, false, false, 0, null, -1, null, ItemExitBehavior.ReturnToPlayer, true);
					}
					if (answer.responseKey == "NIVShopResponse")
					{
						Utility.TryOpenShopMenu("NIVInsectariumShop", "IvyInsectarium", true);
					}
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000208D File Offset: 0x0000028D
		public void Command(string common, string[] args)
		{
			this.Instantiate(args[0], Game1.player.Tile, Game1.player.currentLocation);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000061F4 File Offset: 0x000043F4
		public void OnLaunch(object sender, GameLaunchedEventArgs eventArgs)
		{
			ISpaceCoreAPI api = this.helper.ModRegistry.GetApi<ISpaceCoreAPI>("spacechase0.SpaceCore");
			api.RegisterSerializerType(typeof(NatInValeyNet));
			api.RegisterSerializerType(typeof(NatInValeyGoldenNet));
			api.RegisterSerializerType(typeof(NatInValeyJadeNet));
			api.RegisterSerializerType(typeof(NatInValeySaphNet));
			api.RegisterSerializerType(typeof(Terrarium));
			foreach (KeyValuePair<string, CreatureModel> keyValuePair in Game1.content.Load<Dictionary<string, CreatureModel>>("Nature.NITV/Creatures"))
			{
				List<string> value2 = new List<string>
				{
					keyValuePair.Value.rarity.ToString(),
					keyValuePair.Value.grounded.ToString().ToLower(),
					keyValuePair.Value.speed.ToString(),
					keyValuePair.Value.pauseTime.ToString(),
					keyValuePair.Value.scale.ToString(),
					keyValuePair.Value.doesRun.ToString().ToLower(),
					keyValuePair.Value.isMover.ToString().ToLower(),
					keyValuePair.Value.range.ToString(),
					keyValuePair.Value.dangerous.ToString().ToLower(),
					keyValuePair.Value.seasons.Join(null, ","),
					keyValuePair.Value.weatherCode,
					keyValuePair.Value.locations.Join(null, ","),
					keyValuePair.Value.minTime.ToString(),
					keyValuePair.Value.maxTime.ToString(),
					keyValuePair.Value.frames.ToString(),
					keyValuePair.Value.spritePath,
					keyValuePair.Value.xShadow.ToString(),
					keyValuePair.Value.localSpawnCode,
					keyValuePair.Value.yShadow.ToString(),
					keyValuePair.Value.price.ToString(),
					keyValuePair.Value.spriteIndex.ToString(),
					keyValuePair.Value.xDef.ToString(),
					keyValuePair.Value.yDef.ToString(),
					keyValuePair.Value.itemTexture.ToString(),
					keyValuePair.Value.displayName,
					keyValuePair.Value.displayDescription,
					keyValuePair.Value.xSpriteSize.ToString(),
					keyValuePair.Value.ySpriteSize.ToString(),
					keyValuePair.Value.GSQ,
					keyValuePair.Value.packSize.ToString(),
					keyValuePair.Value.rotaryAnims.ToString().ToLower(),
					keyValuePair.Value.complexIdling.ToString().ToLower(),
					keyValuePair.Value.friendlyFollower.ToString().ToLower(),
					keyValuePair.Value.dangerDamage.ToString(),
					keyValuePair.Value.health.ToString(),
					keyValuePair.Value.forceSword.ToString().ToLower(),
					keyValuePair.Value.cueName,
					keyValuePair.Value.variantList.Join(null, ","),
					keyValuePair.Value.variantChance.ToString(),
					keyValuePair.Value.alternativeDrop,
					keyValuePair.Value.onlyAlternativeDrop.ToString().ToLower(),
					keyValuePair.Value.alternativeDropChance.ToString(),
					keyValuePair.Value.isTerrariumable.ToString().ToLower(),
					keyValuePair.Value.isDonatable.ToString().ToLower(),
					keyValuePair.Value.semiAquatic.ToString().ToLower(),
					keyValuePair.Value.soundRange.ToString(),
					keyValuePair.Value.soundFrequency.ToString(),
					keyValuePair.Value.useBetterCreatureBounds.ToString().ToLower(),
					keyValuePair.Value.displayedLocation,
					keyValuePair.Value.displayedLocalLocation,
					keyValuePair.Value.secondStartTime.ToString(),
					keyValuePair.Value.secondEndTime.ToString(),
					keyValuePair.Value.variantGSQs.Join(null, ","),
					keyValuePair.Value.extraDescription,
					keyValuePair.Value.compelxAnims.ToString().ToLower()
				};
				if (this.creatureData.ContainsKey(keyValuePair.Key))
				{
					this.creatureData.Remove(keyValuePair.Key);
				}
				this.creatureData.Add(keyValuePair.Key, value2);
			}
			NatureInTheValleyEntry.staticCreatureData = this.creatureData;
			this.MakeModelDataFromData();
			this.helper.GameContent.InvalidateCache("Data/Furniture");
			this.helper.GameContent.InvalidateCache("Data/Objects");
			foreach (KeyValuePair<string, RewardModel> keyValuePair2 in Game1.content.Load<Dictionary<string, RewardModel>>("Nature.NITV/Rewards"))
			{
				if (NatureInTheValleyEntry.customRewards.ContainsKey(keyValuePair2.Key))
				{
					NatureInTheValleyEntry.customRewards.Remove(keyValuePair2.Key);
				}
				NatureInTheValleyEntry.customRewards.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			IGenericModConfigMenuApi api2 = base.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
			IToolbarIconsApi api3 = base.Helper.ModRegistry.GetApi<IToolbarIconsApi>("furyx639.ToolbarIcons");
			if (api3 != null)
			{
				api3.AddToolbarIcon("Nature.NIV.Icon", "Mods/NatureInTheValley/Icon", new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16)), this.helper.Translation.Get("DecoratorTT"));
				api3.ToolbarIconPressed += delegate(object o, string s)
				{
					if (!s.Equals("Nature.NIV.Icon"))
					{
						return;
					}
					if (Context.IsPlayerFree)
					{
						Game1.activeClickableMenu = new ClickIntoCreatureInfoMenu();
					}
				};
			}
			IInformant api4 = base.Helper.ModRegistry.GetApi<IInformant>("Slothsoft.Informant");
			if (api4 != null)
			{
				api4.AddItemDecorator("NatureInTheValleyDonate", new Func<string>(this.DecoratorName), new Func<string>(this.DecoratorDesc), new Func<Item, Texture2D>(this.Decorate));
			}
			GameStateQuery.Register("NAT_NIV_DonatedTotalPercent", new GameStateQueryDelegate(NatureInTheValleyEntry.DonatedTotalGSQ));
			GameStateQuery.Register("NAT_NIV_DonatedSpecific", new GameStateQueryDelegate(NatureInTheValleyEntry.DonatedSpecificGSQ));
			GameStateQuery.Register("NAT_NIV_CaughtSpecific", new GameStateQueryDelegate(NatureInTheValleyEntry.CaughtSpecificGSQ));
			GameStateQuery.Register("NAT_NIV_Exists", new GameStateQueryDelegate(NatureInTheValleyEntry.ExistsGSQ));
			GameStateQuery.Register("NAT_NIV_SpawnedSpecific", new GameStateQueryDelegate(NatureInTheValleyEntry.SpawnedSpecificGSQ));
			GameStateQuery.Register("NAT_NIV_TriggerNameCheck", new GameStateQueryDelegate(NatureInTheValleyEntry.TriggerSpawnByName));
			GameStateQuery.Register("NAT_NIV_TriggerNaturalCheck", new GameStateQueryDelegate(NatureInTheValleyEntry.TriggerNaturalByName));
			GameStateQuery.Register("NAT_NIV_TriggerArbitraryCheck", new GameStateQueryDelegate(NatureInTheValleyEntry.TriggerArbitraryByName));
			GameStateQuery.Register("NAT_NIV_TriggerTileCheck", new GameStateQueryDelegate(NatureInTheValleyEntry.TriggerSpawnByTile));
			TriggerActionManager.RegisterAction("NAT_NIV_ClearCreatures", new TriggerActionDelegate(NatureInTheValleyEntry.ActionClearCreatures));
			TriggerActionManager.RegisterAction("NAT_NIV_InstantiateSpecificCreature", new TriggerActionDelegate(NatureInTheValleyEntry.ActionSpawnSpecific));
			TriggerActionManager.RegisterAction("NAT_NIV_InstantiateSpecificNearTriggered", new TriggerActionDelegate(NatureInTheValleyEntry.ActionSpawnSpecificNearTriggered));
			TriggerActionManager.RegisterAction("NAT_NIV_TryCatch", new TriggerActionDelegate(NatureInTheValleyEntry.ActionTryCatch));
			TriggerActionManager.RegisterAction("NAT_NIV_RemoveSpecific", new TriggerActionDelegate(NatureInTheValleyEntry.ClearSpecific));
			TriggerActionManager.RegisterTrigger("NAT_NIV_Spawned");
			TriggerActionManager.RegisterTrigger("NAT_NIV_Caught");
			if (api2 == null)
			{
				return;
			}
			api2.Register(base.ModManifest, delegate
			{
				this.config = new NatInValleyConfig();
			}, delegate
			{
				base.Helper.WriteConfig<NatInValleyConfig>(this.config);
			}, false);
			api2.AddNumberOption(base.ModManifest, () => this.config.spawnRateMultiplier, delegate(float value)
			{
				this.config.spawnRateMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingMult"), () => this.helper.Translation.Get("MenuingMultInfo"), new float?(0.1f), new float?(10f), new float?(0.1f), null, null);
			api2.AddNumberOption(base.ModManifest, () => this.config.maxcreaturelLimitMultiplier, delegate(float value)
			{
				this.config.maxcreaturelLimitMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingLim"), () => this.helper.Translation.Get("MenuingLimInfo"), new float?(0.1f), new float?(10f), new float?(0.1f), null, null);
			api2.AddNumberOption(base.ModManifest, () => this.config.creaturePriceMultiplier, delegate(float value)
			{
				this.config.creaturePriceMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingPrice"), () => this.helper.Translation.Get("MenuingPriceInfo"), new float?(0.1f), new float?(10f), new float?(0.1f), null, null);
			api2.AddBoolOption(base.ModManifest, () => this.config.addCreaturesToShippingCollection, delegate(bool value)
			{
				this.config.addCreaturesToShippingCollection = value;
			}, () => this.helper.Translation.Get("MenuingShip"), () => this.helper.Translation.Get("MenuingShipInfo"), null);
			api2.AddBoolOption(base.ModManifest, () => this.config.useOnlyContentPacks, delegate(bool value)
			{
				this.config.useOnlyContentPacks = value;
				NatureInTheValleyEntry.staticConfig.useOnlyContentPacks = value;
			}, () => this.helper.Translation.Get("MenuingPack"), () => "", null);
			api2.AddBoolOption(base.ModManifest, () => this.config.useTerrariumWallpapers, delegate(bool value)
			{
				this.config.useTerrariumWallpapers = value;
				NatureInTheValleyEntry.staticConfig.useTerrariumWallpapers = value;
			}, () => this.helper.Translation.Get("MenuingWall"), () => "", null);
			api2.AddBoolOption(base.ModManifest, () => this.config.includeChargeSound, delegate(bool value)
			{
				this.config.includeChargeSound = value;
			}, () => this.helper.Translation.Get("MenuingSound"), () => "", null);
			api2.AddBoolOption(base.ModManifest, () => this.config.boolDisableShop, delegate(bool value)
			{
				this.config.boolDisableShop = value;
			}, () => this.helper.Translation.Get("MenuingMail"), () => this.helper.Translation.Get("MenuingMailInfo"), null);
			api2.AddNumberOption(base.ModManifest, () => this.config.catchingDifficultyMultiplier, delegate(float value)
			{
				this.config.catchingDifficultyMultiplier = value;
				NatureInTheValleyEntry.staticConfig.catchingDifficultyMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingNet"), () => this.helper.Translation.Get("MenuingNetInfo"), new float?(0.7f), new float?(3f), new float?(0.1f), null, null);
			api2.AddNumberOption(base.ModManifest, () => this.config.creatureRangeMultiplier, delegate(float value)
			{
				this.config.creatureRangeMultiplier = value;
				NatureInTheValleyEntry.staticConfig.creatureRangeMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingRange"), () => this.helper.Translation.Get("MenuingRangeInfo"), new float?(0f), new float?(3f), new float?(0.1f), null, null);
			api2.AddNumberOption(base.ModManifest, () => this.config.creatureSizeMultiplier, delegate(float value)
			{
				this.config.creatureSizeMultiplier = value;
				NatureInTheValleyEntry.staticConfig.creatureSizeMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingSize"), () => this.helper.Translation.Get("MenuingSizeInfo"), new float?(0.5f), new float?(2f), new float?(0.1f), null, null);
			api2.AddNumberOption(base.ModManifest, () => this.config.creatureDamageModifier, delegate(float value)
			{
				this.config.creatureDamageModifier = value;
				NatureInTheValleyEntry.staticConfig.creatureDamageModifier = value;
			}, () => this.helper.Translation.Get("MenuingDamage"), () => this.helper.Translation.Get("MenuingDamageInfo"), new float?(0f), new float?(3f), new float?(0.1f), null, null);
			api2.AddKeybindList(base.ModManifest, () => this.config.KeyForEncy, delegate(KeybindList value)
			{
				this.config.KeyForEncy = value;
			}, () => this.helper.Translation.Get("MenuingKey"), () => this.helper.Translation.Get("MenuingKeyInfo"), null);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00006FF4 File Offset: 0x000051F4
		public static void TryCatch(Farmer farmer)
		{
			Vector2 value = default(Vector2);
			switch (farmer.FacingDirection)
			{
			case 0:
				value = farmer.Position + new Vector2(0f, -96f);
				break;
			case 1:
				value = farmer.Position + new Vector2(96f, 0f);
				break;
			case 2:
				value = farmer.Position + new Vector2(0f, 96f);
				break;
			case 3:
				value = farmer.Position + new Vector2(-96f, 0f);
				break;
			}
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (farmer.currentLocation.Name == "NIVInnerInsec" || (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 36 && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][35] == "true"))
				{
					return;
				}
				if (NatureInTheValleyEntry.creatures[i].GetLocation().Name == farmer.currentLocation.Name && (double)Vector2.Distance(NatureInTheValleyEntry.creatures[i].GetEffectivePosition() + (NatureInTheValleyEntry.creatures[i].IsGrounded ? Vector2.Zero : new Vector2(0f, -30f)), value) < (double)((NatureInTheValleyEntry.creatures[i].IsGrounded ? 80f : 105f) * NatureInTheValleyEntry.staticConfig.catchingDifficultyMultiplier) * Math.Sqrt((double)NatureInTheValleyEntry.creatures[i].scale))
				{
					if (!NatureInTheValleyEntry.creatures[i].released)
					{
						string text = "";
						if (!Game1.player.modData.TryGetValue("NIVCaught" + NatureInTheValleyEntry.creatures[i].name, out text))
						{
							NatureInTheValleyEntry.sparklingText.Value = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\1_6_Strings:FirstCatch"), new Color(200, 255, 220), Color.White, true, 0.3, 2500, -1, 500, 1f);
							Game1.player.modData["NIVCaught" + NatureInTheValleyEntry.creatures[i].name] = "true";
							Game1.player.gainExperience(2, 11 + (int)Math.Pow(4.0, (double)NatureInTheValleyEntry.creatures[i].Rarity));
							Game1.playSound("discoverMineral", null);
						}
						else
						{
							Game1.playSound("jingle1", null);
							Game1.player.gainExperience(2, 3 + (int)Math.Pow(4.0, (double)NatureInTheValleyEntry.creatures[i].Rarity));
						}
						if (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 40 && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][39] != "" && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 42 && Game1.random.NextDouble() <= (double)float.Parse(NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][41]))
						{
							Item item = ItemRegistry.Create(NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][39], 1, 0, false);
							Game1.player.addItemByMenuIfNecessary(item, null, false);
						}
					}
					if (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count <= 41 || NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][40] == "false")
					{
						Game1.player.addItemByMenuIfNecessary(ItemRegistry.Create("NatInValley.Creature." + NatureInTheValleyEntry.creatures[i].name, 1, 0, false), null, false);
					}
					Item item2 = ItemRegistry.Create("NatInValley.Creature." + NatureInTheValleyEntry.creatures[i].name, 1, 0, false);
					item2.modData["NAT_NIV_NAME"] = NatureInTheValleyEntry.creatures[i].name;
					TriggerActionManager.Raise("NAT_NIV_Caught", null, null, null, item2, null);
					NatureInTheValleyEntry.creatures.RemoveAt(i);
					i--;
				}
				else if (NatureInTheValleyEntry.creatures[i].GetLocation() == farmer.currentLocation && Vector2.Distance(NatureInTheValleyEntry.creatures[i].GetEffectivePosition() + new Vector2(16f, 16f) + (NatureInTheValleyEntry.creatures[i].IsGrounded ? Vector2.Zero : new Vector2(0f, -32f)), value) < 180f && !NatureInTheValleyEntry.creatures[i].isRunning && !NatureInTheValleyEntry.creatures[i].Dangerous && !NatureInTheValleyEntry.creatures[i].isStatic)
				{
					NatureInTheValleyEntry.creatures[i].isRunning = true;
				}
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000075B4 File Offset: 0x000057B4
		private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
		{
			if (e.NameWithoutLocale.IsEquivalentTo("Mods/NatureInTheValley/Icon", false))
			{
				e.LoadFromModFile<Texture2D>("PNGs/Icon.png", AssetLoadPriority.Low);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Mods/NatureInTheValley/TerrariumItem", false))
			{
				e.LoadFromModFile<Texture2D>("PNGs/TerrariumItem.png", AssetLoadPriority.Low);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Nature.NITV/Creatures", false))
			{
				e.LoadFrom(() => new Dictionary<string, CreatureModel>(), AssetLoadPriority.Exclusive, null);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Nature.NITV/Rewards", false))
			{
				e.LoadFrom(() => new Dictionary<string, RewardModel>(), AssetLoadPriority.Exclusive, null);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Data/Objects", false))
			{
				e.Edit(delegate(IAssetData asset)
				{
					IDictionary<string, ObjectData> data = asset.AsDictionary<string, ObjectData>().Data;
					Dictionary<string, ObjectData> dictionary = new Dictionary<string, ObjectData>();
					foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
					{
						if (keyValuePair.Value.Count <= 24)
						{
							ObjectData objectData = new ObjectData();
							objectData.Name = keyValuePair.Key;
							objectData.DisplayName = this.helper.Translation.Get(keyValuePair.Key + ".Name");
							objectData.Description = this.helper.Translation.Get("Rarity." + keyValuePair.Value[0]) + "\n\n" + this.helper.Translation.Get(keyValuePair.Key + ".Description");
							objectData.Type = "Basic";
							objectData.Category = -81;
							objectData.Price = (int)((float)int.Parse(keyValuePair.Value[19]) * 1f / 2.25f * this.config.creaturePriceMultiplier);
							objectData.Texture = "Mods\\NatureInTheValley\\Creatures\\Items";
							objectData.SpriteIndex = int.Parse(keyValuePair.Value[20]);
							objectData.CanBeGivenAsGift = true;
							objectData.ExcludeFromRandomSale = true;
							objectData.ExcludeFromShippingCollection = !this.config.addCreaturesToShippingCollection;
							dictionary.Add("NatInValley.Creature." + keyValuePair.Key, objectData);
						}
						else
						{
							ObjectData objectData2 = new ObjectData();
							objectData2.Name = keyValuePair.Key;
							if (this.helper.Translation.Get(keyValuePair.Key + ".Name").HasValue())
							{
								objectData2.DisplayName = this.helper.Translation.Get(keyValuePair.Key + ".Name");
								objectData2.Description = this.helper.Translation.Get("Rarity." + keyValuePair.Value[0]) + "\n\n" + this.helper.Translation.Get(keyValuePair.Key + ".Description");
							}
							else
							{
								objectData2.DisplayName = keyValuePair.Value[24];
								objectData2.Description = this.helper.Translation.Get("Rarity." + keyValuePair.Value[0]) + "\n\n" + keyValuePair.Value[25];
							}
							objectData2.Type = "Basic";
							objectData2.Category = -81;
							objectData2.Price = (int)((float)int.Parse(keyValuePair.Value[19]) * 1f / 2.25f * this.config.creaturePriceMultiplier);
							objectData2.Texture = keyValuePair.Value[23];
							objectData2.SpriteIndex = int.Parse(keyValuePair.Value[20]);
							objectData2.CanBeGivenAsGift = true;
							objectData2.ExcludeFromRandomSale = true;
							objectData2.ExcludeFromShippingCollection = !this.config.addCreaturesToShippingCollection;
							dictionary.Add("NatInValley.Creature." + keyValuePair.Key, objectData2);
						}
					}
					data.TryAddMany(dictionary);
				}, AssetEditPriority.Default, null);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Data/Tools", false))
			{
				e.Edit(delegate(IAssetData asset)
				{
					asset.AsDictionary<string, ToolData>().Data.TryAddMany(new Dictionary<string, ToolData>
					{
						{
							"NIVNet",
							new ToolData
							{
								Name = "NatValleyNet",
								DisplayName = this.helper.Translation.Get("NetName"),
								Description = this.helper.Translation.Get("NetDescript"),
								ClassName = "NatureInTheValley.NatInValeyNet, NatureInTheValley, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null",
								Texture = "Mods\\NatureInTheValley\\Creatures\\Items",
								SpriteIndex = 0
							}
						},
						{
							"NIVJadeNet",
							new ToolData
							{
								Name = "NatValleyJadeNet",
								DisplayName = this.helper.Translation.Get("NetName"),
								Description = this.helper.Translation.Get("NetDescript"),
								ClassName = "NatureInTheValley.NatInValeyJadeNet, NatureInTheValley, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null",
								Texture = "Mods\\NatureInTheValley\\Creatures\\Items",
								SpriteIndex = 0
							}
						},
						{
							"NIVGoldNet",
							new ToolData
							{
								Name = "NatValleyGoldNet",
								DisplayName = this.helper.Translation.Get("NetName"),
								Description = this.helper.Translation.Get("NetDescript"),
								ClassName = "NatureInTheValley.NatInValeyGoldenNet, NatureInTheValley, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null",
								Texture = "Mods\\NatureInTheValley\\Creatures\\Items",
								SpriteIndex = 0
							}
						},
						{
							"NIVSaphNet",
							new ToolData
							{
								Name = "NatValleySaphNet",
								DisplayName = this.helper.Translation.Get("NetSaphName"),
								Description = this.helper.Translation.Get("NetSaphDescript"),
								ClassName = "NatureInTheValley.NatInValeySaphNet, NatureInTheValley, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null",
								Texture = "Mods\\NatureInTheValley\\Creatures\\Items",
								SpriteIndex = 0
							}
						}
					});
				}, AssetEditPriority.Default, null);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Data/Furniture", false))
			{
				e.Edit(delegate(IAssetData asset)
				{
					IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
					{
						dictionary.Add("Tera.NatInValley.Creature." + keyValuePair.Key, string.Concat(new string[]
						{
							"Tera.NatInValley.Creature.",
							keyValuePair.Key,
							"/other/2 2/2 1/1/",
							((int)((float)int.Parse(keyValuePair.Value[19]) * 1f / 1.5f * this.config.creaturePriceMultiplier)).ToString(),
							"/2/Terrarium/0/Mods\\NatureInTheValley\\TerrariumItem/true/"
						}));
					}
					data.TryAddMany(dictionary);
				}, AssetEditPriority.Default, null);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Data/Mail", false))
			{
				e.Edit(delegate(IAssetData asset)
				{
					IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					if (Game1.player != null && Game1.player.mailReceived.Contains("NIV_InsectariumBonus"))
					{
						dictionary.Add("NIV_InsectariumBonus", string.Concat(new string[]
						{
							this.helper.Translation.Get("IvyMail1"),
							"%item money ",
							((int)((double)this.donatedCount * 4.2 * 28.0)).ToString(),
							this.helper.Translation.Get("IvyMailTitle")
						}));
					}
					else
					{
						dictionary.Add("NIV_InsectariumBonus", string.Concat(new string[]
						{
							this.helper.Translation.Get("IvyMail2"),
							"%item money ",
							((int)((double)this.donatedCount * 4.2 * 28.0)).ToString(),
							this.helper.Translation.Get("IvyMailTitle")
						}));
					}
					if (data.ContainsKey("NIV_InsectariumBonus"))
					{
						data.Remove("NIV_InsectariumBonus");
					}
					data.TryAddMany(dictionary);
				}, AssetEditPriority.Default, null);
				return;
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Data/NPCGiftTastes", false))
			{
				e.Edit(delegate(IAssetData asset)
				{
					IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
					List<string> list = data["Sebastian"].Split("/", StringSplitOptions.None).ToList<string>();
					List<string> list2 = list[1].Split(" ", StringSplitOptions.None).ToList<string>();
					foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
					{
						if ((!list2.Contains("NatInValley.Creature." + keyValuePair.Key) && keyValuePair.Key.ToLower().Contains("frog")) || keyValuePair.Key.ToLower().Contains("toad"))
						{
							list2.Add("NatInValley.Creature." + keyValuePair.Key);
						}
					}
					list[1] = list2.Join(null, " ");
					data["Sebastian"] = list.Join(null, "/");
					List<string> list3 = data["Vincent"].Split("/", StringSplitOptions.None).ToList<string>();
					List<string> list4 = list3[3].Split(" ", StringSplitOptions.None).ToList<string>();
					foreach (KeyValuePair<string, List<string>> keyValuePair2 in this.creatureData)
					{
						if (!list4.Contains("NatInValley.Creature." + keyValuePair2.Key))
						{
							list4.Add("NatInValley.Creature." + keyValuePair2.Key);
						}
					}
					list3[3] = list4.Join(null, " ");
					data["Vincent"] = list3.Join(null, "/");
				}, AssetEditPriority.Default, null);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00007750 File Offset: 0x00005950
		public void MenuChanged(object sender, MenuChangedEventArgs e)
		{
			IClickableMenu newMenu = e.NewMenu;
			if (!(newMenu is ShopMenu) || ((newMenu as ShopMenu).ShopId != "SeedShop" && (newMenu as ShopMenu).ShopId != "Joja") || (newMenu as ShopMenu).forSale.Contains(new NatInValeyNet()))
			{
				return;
			}
			Dictionary<ISalable, ItemStockInformation> itemPriceAndStock = (newMenu as ShopMenu).itemPriceAndStock;
			List<ISalable> forSale = (newMenu as ShopMenu).forSale;
			NatInValeyNet natInValeyNet = new NatInValeyNet();
			if ((newMenu as ShopMenu).ShopId == "SeedShop")
			{
				forSale.Insert(13, natInValeyNet);
			}
			if ((newMenu as ShopMenu).ShopId == "Joja")
			{
				forSale.Add(natInValeyNet);
			}
			itemPriceAndStock.Add(natInValeyNet, new ItemStockInformation(250, int.MaxValue, null, null, LimitedStockMode.Global, null, null, null, null));
			(newMenu as ShopMenu).itemPriceAndStock = itemPriceAndStock;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000020AC File Offset: 0x000002AC
		private bool ReturnHome(GameLocation location, string[] args, Farmer player, Point point)
		{
			Game1.warpFarmer("BusStop", 22, 11, 0);
			return true;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000784C File Offset: 0x00005A4C
		public bool OpenDonationWindow(GameLocation location, string[] args, Farmer player, Point point)
		{
			location.createQuestionDialogue(this.helper.Translation.Get("InsectariumDesk"), new Response[]
			{
				new Response("NIVFResponse", this.helper.Translation.Get("InsectariumC1")),
				new Response("NIVSResponse", this.helper.Translation.Get("InsectariumC2")),
				new Response("NIVRResponse", this.helper.Translation.Get("InsectariumC4")),
				new Response("NIVTResponse", this.helper.Translation.Get("InsectariumC5")),
				new Response("NIVShopResponse", this.helper.Translation.Get("InsectariumC6")),
				new Response("NIVNResponse", this.helper.Translation.Get("InsectariumC3"))
			}, "NatDesk");
			return true;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00007970 File Offset: 0x00005B70
		public void setUpInsectarium(GameLocation location)
		{
			int num = 0;
			int num2 = 0;
			if (this.CreaturesInLocationR(location) < 1)
			{
				using (Dictionary<string, List<string>>.Enumerator enumerator = this.creatureData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, List<string>> keyValuePair = enumerator.Current;
						if (keyValuePair.Value.Count <= 44 || keyValuePair.Value[43] == "true")
						{
							num2++;
						}
						string text;
						if (location.modData.TryGetValue("NatureInTheValley/Donated/" + keyValuePair.Key, out text))
						{
							num++;
							List<string> value = keyValuePair.Value;
							NatureInTheValleyEntry.creatures.Add(new NatCreature(new Vector2(float.Parse(value[21]), float.Parse(value[22])) * 64f, location, keyValuePair.Key, int.Parse(value[0]), value[1] == "true", float.Parse(value[2]), int.Parse(value[3]), float.Parse(value[4]), value[5] == "true", value[6] == "true", int.Parse(value[7]), value[8] == "true", int.Parse(value[12]), int.Parse(value[13]), int.Parse(value[14]), value[15], int.Parse(value[16]), int.Parse(value[17]), int.Parse(value[18]), true, (value.Count > 26) ? int.Parse(value[26]) : 32, (value.Count > 27) ? int.Parse(value[27]) : 32, value[value.Count - 1] == "true", value.Count > 31 && value[30] == "true", value.Count > 32 && value[31] == "true", value.Count > 33 && value[32] == "true", (value.Count > 34) ? int.Parse(value[33]) : 25, (value.Count > 35) ? int.Parse(value[34]) : 0, (value.Count > 37) ? value[36] : "", this.config.creatureSizeMultiplier, (value.Count > 38) ? new List<string>(value[37].Split(",", StringSplitOptions.None)) : new List<string>(), (value.Count > 39) ? float.Parse(value[38]) : 0.1f, value.Count > 45 && value[44] == "true", (value.Count > 46) ? float.Parse(value[45]) : 500f, (value.Count > 47) ? int.Parse(value[46]) : 1150, value.Count > 48 && value[47] == "true", (value.Count > 53) ? new List<string>(value[52].Split(",", StringSplitOptions.None)) : new List<string>()));
						}
					}
					goto IL_3E3;
				}
			}
			num = this.CreaturesInLocationR(location);
			IL_3E3:
			this.donatedCount = Math.Max(this.donatedCount, num);
			if (this.layersToSave.Count < 1 && location.map.GetLayer("AlwaysFront72") != null)
			{
				this.layersToSave = new List<Layer>
				{
					location.map.GetLayer("AlwaysFront72"),
					location.map.GetLayer("Front72"),
					location.map.GetLayer("Buildings72"),
					location.map.GetLayer("Front60"),
					location.map.GetLayer("Buildings601"),
					location.map.GetLayer("Buildings602"),
					location.map.GetLayer("AlwaysFront28"),
					location.map.GetLayer("Front48"),
					location.map.GetLayer("Buildings48"),
					location.map.GetLayer("Back48"),
					location.map.GetLayer("AlwaysFront24"),
					location.map.GetLayer("Front24"),
					location.map.GetLayer("Buildings24"),
					location.map.GetLayer("Back3"),
					location.map.GetLayer("Back4"),
					location.map.GetLayer("Front36"),
					location.map.GetLayer("Buildings1834"),
					location.map.GetLayer("Buildings4340"),
					location.map.GetLayer("Buildings4614"),
					location.map.GetLayer("Buildings1618"),
					location.map.GetLayer("Buildings6436"),
					location.map.GetLayer("Buildings6815"),
					location.map.GetLayer("Buildings4845"),
					location.map.GetLayer("Buildings3114"),
					location.map.GetLayer("Buildings3725"),
					location.map.GetLayer("Buildings4625"),
					location.map.GetLayer("Buildings5740"),
					location.map.GetLayer("Buildings3821"),
					location.map.GetLayer("Buildings7120"),
					location.map.GetLayer("Buildings1141"),
					location.map.GetLayer("Buildings7335"),
					location.map.GetLayer("Buildings2245"),
					location.map.GetLayer("Buildings6528"),
					location.map.GetLayer("Buildings4120"),
					location.map.GetLayer("Buildings937"),
					location.map.GetLayer("Buildings2042"),
					location.map.GetLayer("Buildings6413"),
					location.map.GetLayer("Buildings6742"),
					location.map.GetLayer("Buildings5624"),
					location.map.GetLayer("Front1834"),
					location.map.GetLayer("Front4340"),
					location.map.GetLayer("Front4614"),
					location.map.GetLayer("Front1618"),
					location.map.GetLayer("Front6436"),
					location.map.GetLayer("Front6815"),
					location.map.GetLayer("Front4845"),
					location.map.GetLayer("Front3114"),
					location.map.GetLayer("Front3725"),
					location.map.GetLayer("Front4625"),
					location.map.GetLayer("Front5740"),
					location.map.GetLayer("Front3821"),
					location.map.GetLayer("Front7120"),
					location.map.GetLayer("Front1141"),
					location.map.GetLayer("Front7335"),
					location.map.GetLayer("Front2245"),
					location.map.GetLayer("Front6528"),
					location.map.GetLayer("Front4120"),
					location.map.GetLayer("Front937"),
					location.map.GetLayer("Front2042"),
					location.map.GetLayer("Front6413"),
					location.map.GetLayer("Front6742"),
					location.map.GetLayer("Front5642")
				};
			}
			if (this.layersToSave.Count > 1)
			{
				foreach (Layer layer in this.layersToSave)
				{
					if (!location.map.Layers.Contains(layer))
					{
						try
						{
							location.map.AddLayer(layer);
						}
						catch (Exception)
						{
						}
					}
				}
			}
			this.setupTempCharacters(location, this.donatedCount, num2);
			if (num < 72)
			{
				location.map.RemoveLayer(location.map.GetLayer("AlwaysFront72"));
				location.map.RemoveLayer(location.map.GetLayer("Front72"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings72"));
				location.removeTileProperty(38, 36, "Back", "Passable");
				location.removeTileProperty(42, 36, "Back", "Passable");
				location.removeTileProperty(43, 36, "Back", "Passable");
			}
			else
			{
				location.setTileProperty(38, 36, "Back", "Passable", "T");
				location.setTileProperty(42, 36, "Back", "Passable", "T");
				location.setTileProperty(43, 36, "Back", "Passable", "T");
			}
			if (num < 60)
			{
				location.map.RemoveLayer(location.map.GetLayer("Buildings601"));
				location.map.RemoveLayer(location.map.GetLayer("Front60"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings602"));
				location.removeTileProperty(31, 46, "Back", "Passable");
				location.removeTileProperty(35, 46, "Back", "Passable");
				location.removeTileProperty(33, 44, "Back", "Passable");
				location.removeTileProperty(33, 48, "Back", "Passable");
				location.removeTileProperty(47, 47, "Back", "Passable");
				location.removeTileProperty(48, 48, "Back", "Passable");
				location.removeTileProperty(49, 46, "Back", "Passable");
				location.removeTileProperty(50, 47, "Back", "Passable");
				location.removeTileProperty(32, 37, "Back", "Passable");
				location.removeTileProperty(33, 37, "Back", "Passable");
				location.removeTileProperty(34, 37, "Back", "Passable");
				location.removeTileProperty(47, 37, "Back", "Passable");
				location.removeTileProperty(48, 37, "Back", "Passable");
				location.removeTileProperty(49, 37, "Back", "Passable");
				location.removeTileProperty(48, 46, "Back", "Passable");
				location.removeTileProperty(49, 46, "Back", "Passable");
				location.removeTileProperty(48, 47, "Back", "Passable");
				location.removeTileProperty(49, 47, "Back", "Passable");
				location.removeTileProperty(32, 44, "Back", "Passable");
				location.removeTileProperty(34, 44, "Back", "Passable");
				location.removeTileProperty(32, 45, "Back", "Passable");
				location.removeTileProperty(33, 45, "Back", "Passable");
				location.removeTileProperty(34, 45, "Back", "Passable");
				location.removeTileProperty(32, 46, "Back", "Passable");
				location.removeTileProperty(33, 46, "Back", "Passable");
				location.removeTileProperty(34, 46, "Back", "Passable");
				location.removeTileProperty(32, 47, "Back", "Passable");
				location.removeTileProperty(33, 47, "Back", "Passable");
				location.removeTileProperty(34, 47, "Back", "Passable");
			}
			else
			{
				location.setTileProperty(31, 46, "Back", "Passable", "T");
				location.setTileProperty(35, 46, "Back", "Passable", "T");
				location.setTileProperty(33, 44, "Back", "Passable", "T");
				location.setTileProperty(33, 48, "Back", "Passable", "T");
				location.setTileProperty(47, 47, "Back", "Passable", "T");
				location.setTileProperty(48, 48, "Back", "Passable", "T");
				location.setTileProperty(49, 46, "Back", "Passable", "T");
				location.setTileProperty(50, 47, "Back", "Passable", "T");
				location.setTileProperty(32, 37, "Back", "Passable", "T");
				location.setTileProperty(33, 37, "Back", "Passable", "T");
				location.setTileProperty(34, 37, "Back", "Passable", "T");
				location.setTileProperty(47, 37, "Back", "Passable", "T");
				location.setTileProperty(48, 37, "Back", "Passable", "T");
				location.setTileProperty(49, 37, "Back", "Passable", "T");
				location.setTileProperty(48, 46, "Back", "Passable", "T");
				location.setTileProperty(49, 46, "Back", "Passable", "T");
				location.setTileProperty(48, 47, "Back", "Passable", "T");
				location.setTileProperty(49, 47, "Back", "Passable", "T");
				location.setTileProperty(32, 44, "Back", "Passable", "T");
				location.setTileProperty(34, 44, "Back", "Passable", "T");
				location.setTileProperty(32, 45, "Back", "Passable", "T");
				location.setTileProperty(33, 45, "Back", "Passable", "T");
				location.setTileProperty(34, 45, "Back", "Passable", "T");
				location.setTileProperty(32, 46, "Back", "Passable", "T");
				location.setTileProperty(33, 46, "Back", "Passable", "T");
				location.setTileProperty(34, 46, "Back", "Passable", "T");
				location.setTileProperty(32, 47, "Back", "Passable", "T");
				location.setTileProperty(33, 47, "Back", "Passable", "T");
				location.setTileProperty(34, 47, "Back", "Passable", "T");
			}
			if (num < 48)
			{
				location.map.RemoveLayer(location.map.GetLayer("Back48"));
				location.map.RemoveLayer(location.map.GetLayer("AlwaysFront28"));
				location.map.RemoveLayer(location.map.GetLayer("Front48"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings48"));
				location.removeTileProperty(42, 45, "Back", "Passable");
			}
			else
			{
				location.setTileProperty(42, 45, "Back", "Passable", "T");
			}
			if (num < 36)
			{
				location.map.RemoveLayer(location.map.GetLayer("Front36"));
			}
			if (num < 24)
			{
				location.map.RemoveLayer(location.map.GetLayer("AlwaysFront24"));
				location.map.RemoveLayer(location.map.GetLayer("Front24"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings24"));
				location.removeTileProperty(51, 40, "Back", "Passable");
				location.removeTileProperty(51, 43, "Back", "Passable");
				location.removeTileProperty(43, 43, "Back", "Passable");
				location.removeTileProperty(38, 43, "Back", "Passable");
				location.removeTileProperty(30, 43, "Back", "Passable");
				location.removeTileProperty(30, 40, "Back", "Passable");
			}
			else
			{
				location.setTileProperty(51, 40, "Back", "Passable", "T");
				location.setTileProperty(51, 43, "Back", "Passable", "T");
				location.setTileProperty(43, 43, "Back", "Passable", "T");
				location.setTileProperty(38, 43, "Back", "Passable", "T");
				location.setTileProperty(30, 43, "Back", "Passable", "T");
				location.setTileProperty(30, 40, "Back", "Passable", "T");
			}
			if (num < 12)
			{
				location.map.RemoveLayer(location.map.GetLayer("Back3"));
				location.map.RemoveLayer(location.map.GetLayer("Back4"));
			}
			location.reloadMap();
			if (!Game1.player.hasOrWillReceiveMail("InsectariumEntryEvent"))
			{
				Game1.player.mailReceived.Add("InsectariumEntryEvent");
				Event evt = new Event(string.Concat(new string[]
				{
					"continue/-1000 -1000/farmer 41 50 0 IvyInsectarium 41 39 2/skippable/viewport 41 37 true/pause 650/viewport move 0 2 2000/move farmer 0 -5 0/pause 200/jump IvyInsectarium 6/pause 200/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv1"),
					"\"/pause 200/quickQuestion #",
					this.helper.Translation.Get("IvyEv2"),
					"#",
					this.helper.Translation.Get("IvyEv3"),
					"#",
					this.helper.Translation.Get("IvyEv4"),
					"(break)speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv5"),
					"\"(break)speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv6"),
					"\"(break)speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv7"),
					"\"/pause 300/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv8"),
					"\"/end position 41 45"
				}), Game1.player);
				location.startEvent(evt);
				return;
			}
			if (num >= num2 * 2 / 3 && !Game1.player.hasOrWillReceiveMail("InsectariumSecondEvent"))
			{
				Game1.player.mailReceived.Add("InsectariumSecondEvent");
				Event evt2 = new Event(string.Concat(new string[]
				{
					"playful/-1000 -1000/farmer 41 41 0 IvyInsectarium 41 39 2 Penny -1000 -1000 0 Jas -999 -999 0 Vincent -998 -988 0/skippable/viewport 41 42 true/pause 650/pause 200/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.1"),
					"\"/jump IvyInsectarium 2/playSound doorClose/faceDirection farmer 2/warp Jas 40 51 true/warp Vincent 41 51 false/move Jas 0 -7 2 true/move Vincent 0 -7 2 false/playSound doorClose/warp Penny 41 51/move Penny 0 -5 0/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.2"),
					"\"/pause 200/message \"",
					this.helper.Translation.Get("IvyEv.2.3"),
					"\"/pause 100/faceDirection Jas 0/faceDirection Vincent 0/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv.2.4"),
					"\"/pause 100/faceDirection Jas 2/faceDirection Vincent 2/speak Vincent \"",
					this.helper.Translation.Get("IvyEv.2.5"),
					"\"/speak Jas \"",
					this.helper.Translation.Get("IvyEv.2.6"),
					"\"/speak Vincent \"",
					this.helper.Translation.Get("IvyEv.2.7"),
					"\"/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.8"),
					"\"/globalFade 0.015 false/viewport -1000 -1000 true/warp Penny 70 39 true/faceDirection Penny 2/warp Jas 71 41 true/faceDirection Jas 1/warp Vincent 69 40 true/faceDirection Vincent 3/viewport 67 40/globalFadeToClear 0.015/shake Vincent 350/emote Jas 56/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.9"),
					"\"/faceDirection Vincent 0/faceDirection Jas 0/jump Vincent 6/jump Vincent 6/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.10"),
					"\"/pause 200/speak Vincent \"",
					this.helper.Translation.Get("IvyEv.2.11"),
					"\"/pause 200/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.12"),
					"\"/pause 200/speak Jas \"",
					this.helper.Translation.Get("IvyEv.2.13"),
					"\"/pause 200/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.14"),
					"\"/emote Jas 56/pause 400/globalFade 0.015 false/viewport -1000 -1000 true/warp Penny 18 36 true/faceDirection Penny 2/warp Jas 15 38 true/faceDirection Jas 2/warp Vincent 19 39 true/faceDirection Vincent 2/viewport 17 40/globalFadeToClear 0.015/jump Vincent 5/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.15"),
					"\"/pause 200/faceDirection Vincent 3/speak Vincent \"",
					this.helper.Translation.Get("IvyEv.2.16"),
					"\"/pause 200/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.17"),
					"\"/jump Vincent 3/shake Jas 200/pause 200/speak Jas \"",
					this.helper.Translation.Get("IvyEv.2.18"),
					"\"/pause 200/globalFade 0.015 false/viewport -1000 -1000 true/warp Penny 18 15 true/faceDirection Penny 2/warp Jas 17 17 true/faceDirection Jas 0/warp Vincent 19 17 true/faceDirection Vincent 0/viewport 15 18/globalFadeToClear 0.015/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.19"),
					"\"/pause 200/message \"",
					this.helper.Translation.Get("IvyEv.2.20"),
					"\"/pause 200/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.21"),
					"\"/pause 200/speak Jas \"",
					this.helper.Translation.Get("IvyEv.2.22"),
					"\"/pause 200/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.23"),
					"\"/pause 200/globalFade 0.015 false/viewport -1000 -1000 true/warp Penny 58 16 true/faceDirection Penny 2/warp Jas 57 18 true/faceDirection Jas 0/warp Vincent 59 18 true/faceDirection Vincent 0/viewport 55 16/globalFadeToClear 0.015/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.25"),
					"\"/pause 200/speak Vincent \"",
					this.helper.Translation.Get("IvyEv.2.26"),
					"\"/pause 1000/faceDirection Vincent 3/emote Vincent 40/pause 500/speak Jas \"",
					this.helper.Translation.Get("IvyEv.2.27"),
					"\"/emote Jase 56/pause 500/speak Penny \"",
					this.helper.Translation.Get("IvyEv.2.28"),
					"\"/pause 100/end position 41 45"
				}), Game1.player);
				location.startEvent(evt2);
				return;
			}
			if (num >= num2 && !Game1.player.hasOrWillReceiveMail("InsectariumFinalEvent"))
			{
				Game1.player.mailReceived.Add("InsectariumFinalEvent");
				Event evt3 = new Event(string.Concat(new string[]
				{
					"continue/-1000 -1000/farmer 46 18 0 IvyInsectarium 46 15 2/skippable/viewport 42 14 true/pause 650/viewport move 1 1 4000/pause 200/emote IvyInsectarium 56/pause 200/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv9"),
					"\"/pause 400/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv10"),
					"\"/pause 400/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv11"),
					"\"/pause 400/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv12"),
					"\"/pause 400/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv13"),
					"\"/pause 400/speak IvyInsectarium \"",
					this.helper.Translation.Get("IvyEv14"),
					"\"/viewport move 2 1 3000/pause 2500/end position 41 45"
				}), Game1.player);
				location.startEvent(evt3);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000093B0 File Offset: 0x000075B0
		public void CommandTwo(string common, string[] args)
		{
			GameLocation locationFromName = Game1.getLocationFromName("NIVInnerInsec");
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				locationFromName.modData["NatureInTheValley/Donated/" + keyValuePair.Key] = "true";
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00009428 File Offset: 0x00007628
		public void CommandThree(string common, string[] args)
		{
			GameLocation locationFromName = Game1.getLocationFromName("NIVInnerInsec");
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				locationFromName.modData.Remove("NatureInTheValley/Donated/" + keyValuePair.Key);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000020BE File Offset: 0x000002BE
		public static bool HighlightCollectableRewards(Item item)
		{
			return Game1.player.couldInventoryAcceptThisItem(item);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000949C File Offset: 0x0000769C
		public static List<Item> getRewardsForPlayer(Farmer who)
		{
			List<Item> list = new List<Item>();
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, List<string>> keyValuePair in NatureInTheValleyEntry.staticCreatureData)
			{
				string text;
				if (Game1.currentLocation.modData.TryGetValue("NatureInTheValley/Donated/" + keyValuePair.Key, out text))
				{
					num++;
				}
				if (keyValuePair.Value.Count <= 44 || keyValuePair.Value[43] == "true")
				{
					num2++;
				}
			}
			foreach (KeyValuePair<string, RewardModel> keyValuePair2 in NatureInTheValleyEntry.customRewards)
			{
				if (num >= keyValuePair2.Value.TotalDonated && !who.hasOrWillReceiveMail("InsecReward." + keyValuePair2.Value.ItemId))
				{
					bool flag = true;
					if (keyValuePair2.Value.creatureRequirements != null)
					{
						foreach (string str in keyValuePair2.Value.creatureRequirements)
						{
							string text2;
							if (!Game1.currentLocation.modData.TryGetValue("NatureInTheValley/Donated/" + str, out text2))
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						list.Add(ItemRegistry.Create(keyValuePair2.Value.ItemId, keyValuePair2.Value.ItemCount, 0, false));
					}
				}
			}
			if (num >= num2 && !who.hasOrWillReceiveMail("InsecReward.(T)NIVGoldNet"))
			{
				list.Add(new NatInValeyGoldenNet());
			}
			if (num >= num2 / 2 && !who.hasOrWillReceiveMail("InsecReward.(T)NIVSaphNet"))
			{
				list.Add(new NatInValeySaphNet());
			}
			if (NatureInTheValleyEntry.staticConfig.useOnlyContentPacks)
			{
				return list;
			}
			if (num >= 1 && !who.hasOrWillReceiveMail("InsecReward.(O)SkillBook_2"))
			{
				list.Add(ItemRegistry.Create("SkillBook_2", 1, 0, false));
			}
			if (num >= 4 && !who.hasOrWillReceiveMail("InsecReward.(O)PrizeTicket"))
			{
				list.Add(ItemRegistry.Create("PrizeTicket", 1, 0, false));
			}
			if (num >= 8 && !who.hasOrWillReceiveMail("InsecReward.(O)TreasureTotem"))
			{
				list.Add(ItemRegistry.Create("TreasureTotem", 2, 0, false));
			}
			if (num >= 13 && !who.hasOrWillReceiveMail("InsecReward.(O)StardropTea"))
			{
				list.Add(ItemRegistry.Create("StardropTea", 1, 0, false));
			}
			if (num >= 19 && !who.hasOrWillReceiveMail("InsecReward.(O)GoldCoin"))
			{
				list.Add(ItemRegistry.Create("GoldCoin", 40, 0, false));
			}
			if (num >= 26 && !who.hasOrWillReceiveMail("InsecReward.(O)745"))
			{
				list.Add(ItemRegistry.Create("745", 15, 0, false));
			}
			if (num >= 33 && !who.hasOrWillReceiveMail("InsecReward.(O)724"))
			{
				list.Add(ItemRegistry.Create("724", 2, 0, false));
			}
			if (num >= 40 && !who.hasOrWillReceiveMail("InsecReward.(O)725"))
			{
				list.Add(ItemRegistry.Create("725", 2, 0, false));
			}
			if (num >= 40 && !who.hasOrWillReceiveMail("InsecReward.(O)726"))
			{
				list.Add(ItemRegistry.Create("726", 2, 0, false));
			}
			if (num >= 47 && !who.hasOrWillReceiveMail("InsecReward.(O)681"))
			{
				list.Add(ItemRegistry.Create("681", 2, 0, false));
			}
			if (num >= 54 && !who.hasOrWillReceiveMail("InsecReward.(O)805"))
			{
				list.Add(ItemRegistry.Create("805", 15, 0, false));
			}
			if (num >= 61 && !who.hasOrWillReceiveMail("InsecReward.(O)907"))
			{
				list.Add(ItemRegistry.Create("907", 1, 0, false));
			}
			if (num >= 68 && !who.hasOrWillReceiveMail("InsecReward.(O)688"))
			{
				list.Add(ItemRegistry.Create("688", 2, 0, false));
			}
			if (num >= 68 && !who.hasOrWillReceiveMail("InsecReward.(O)689"))
			{
				list.Add(ItemRegistry.Create("689", 2, 0, false));
			}
			if (num >= 68 && !who.hasOrWillReceiveMail("InsecReward.(O)690"))
			{
				list.Add(ItemRegistry.Create("690", 2, 0, false));
			}
			if (num >= 75 && !who.hasOrWillReceiveMail("InsecReward.(S)NIVSurveyerShirt"))
			{
				list.Add(ItemRegistry.Create("NIVSurveyerShirt", 1, 0, false));
			}
			if (num >= 82 && !who.hasOrWillReceiveMail("InsecReward.(H)NIVSurveyerhat"))
			{
				list.Add(ItemRegistry.Create("NIVSurveyerhat", 1, 0, false));
			}
			return list;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000020CB File Offset: 0x000002CB
		public static void OnRewardCollected(Item item, Farmer who)
		{
			if (item == null)
			{
				return;
			}
			if (who.hasOrWillReceiveMail("InsecReward." + item.QualifiedItemId))
			{
				return;
			}
			who.mailReceived.Add("InsecReward." + item.QualifiedItemId);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000991C File Offset: 0x00007B1C
		public void DayStarted(object sender, DayStartedEventArgs eventArgs)
		{
			NatureInTheValleyEntry.creatures.Clear();
			this.dailyMod = this.possibleDailyMods[Game1.random.Next(this.possibleDailyMods.Count - 1)];
			if (this.helper.ModRegistry.IsLoaded("malic.cp.jadeNPC"))
			{
				if (Game1.seasonIndex == 1 && Game1.player.getFriendshipLevelForNPC("Jade") >= 2 && !Game1.player.hasQuest("NIVjadeQuest1") && !Game1.player.hasQuest("NIVjadeQuest4") && !Game1.player.hasOrWillReceiveMail("NiTVQ1"))
				{
					Game1.addMail("NiTVQ1", true, false);
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest1"));
				}
				if (Game1.seasonIndex == 2 && Game1.player.getFriendshipLevelForNPC("Jade") >= 4 && !Game1.player.hasQuest("NIVjadeQuest2") && !Game1.player.hasQuest("NIVjadeQuest1") && !Game1.player.hasOrWillReceiveMail("NiTVQ2"))
				{
					Game1.addMail("NiTVQ2", true, false);
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest2"));
				}
				if (Game1.seasonIndex == 3 && Game1.player.getFriendshipLevelForNPC("Jade") >= 6 && !Game1.player.hasQuest("NIVjadeQuest3") && !Game1.player.hasQuest("NIVjadeQuest2") && !Game1.player.hasOrWillReceiveMail("NiTVQ3"))
				{
					Game1.addMail("NiTVQ3", true, false);
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest3"));
				}
				if (Game1.seasonIndex == 4 && Game1.player.getFriendshipLevelForNPC("Jade") >= 8 && !Game1.player.hasQuest("NIVjadeQuest4") && !Game1.player.hasQuest("NIVjadeQuest3") && !Game1.player.hasOrWillReceiveMail("NiTVQ4"))
				{
					Game1.addMail("NiTVQ4", true, false);
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest4"));
				}
			}
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				string text;
				if (Game1.getLocationFromName("NIVInnerInsec").modData.TryGetValue("NatureInTheValley/Donated/" + keyValuePair.Key, out text))
				{
					num++;
				}
				if (keyValuePair.Value.Count <= 44 || keyValuePair.Value[43] == "true")
				{
					num2++;
				}
			}
			this.donatedCount = num;
			if (Game1.dayOfMonth != 1 || this.config.boolDisableShop)
			{
				return;
			}
			if (this.donatedCount > num2 / 3)
			{
				this.helper.GameContent.InvalidateCache("Data/Mail");
				if (Game1.player.mailReceived.Contains("NIV_InsectariumBonus"))
				{
					Game1.player.mailReceived.Remove("NIV_InsectariumBonus");
				}
				Game1.player.mailbox.Add("NIV_InsectariumBonus");
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002106 File Offset: 0x00000306
		private double func(int x)
		{
			return Math.Max((double)x / 6000.0, 1.0);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00009C58 File Offset: 0x00007E58
		public void CommandFour(string common, string[] args)
		{
			foreach (NatCreature natCreature in NatureInTheValleyEntry.creatures)
			{
				base.Monitor.Log(natCreature.name, LogLevel.Warn);
				base.Monitor.Log(natCreature.Position.ToString(), LogLevel.Warn);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00009CD4 File Offset: 0x00007ED4
		public void OneSecond(object sender, OneSecondUpdateTickedEventArgs e)
		{
			if (!Game1.shouldTimePass(false) || Game1.currentLocation == null || Game1.currentLocation.Name == "NIVInnerInsec")
			{
				return;
			}
			if (NatureInTheValleyEntry.creatures.Count < this.locationCap && Game1.random.NextDouble() < this.spawnChance)
			{
				this.TrySpawnFromArea(Game1.currentLocation);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00009D38 File Offset: 0x00007F38
		public void RemovedLargeTerrain(object sender, LargeTerrainFeatureListChangedEventArgs e)
		{
			if (e.Removed == null || !(e.Removed is Bush) || !e.IsCurrentLocation)
			{
				return;
			}
			this.bushPos = this.GetBushes(Game1.currentLocation);
			List<Vector2> list = this.bushPos;
			foreach (NatCreature natCreature in NatureInTheValleyEntry.creatures)
			{
				if (natCreature.LocalLocationCode == 2 && !natCreature.isStatic && natCreature.GetLocation() == Game1.currentLocation)
				{
					Vector2 value = natCreature.Position / 64f;
					bool flag = false;
					foreach (Vector2 value2 in list)
					{
						if (Vector2.Distance(value, value2) < 2.5f)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						natCreature.isRunning = true;
					}
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002122 File Offset: 0x00000322
		public int CreaturesInLocation()
		{
			return NatureInTheValleyEntry.creatures.Count;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00009E48 File Offset: 0x00008048
		public int CreaturesInLocationR(GameLocation l)
		{
			int num = 0;
			using (List<NatCreature>.Enumerator enumerator = NatureInTheValleyEntry.creatures.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetLocation().Name == l.Name)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00009EB0 File Offset: 0x000080B0
		public static bool CoverCheckAction(BusStop __instance, Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who, out bool __result)
		{
			if ((!NatureInTheValleyEntry.staticHelper.ModRegistry.IsLoaded("hootless.BusLocations") || !NatureInTheValleyEntry.staticHelper.ModRegistry.IsLoaded("Nature.NIVBL")) && tileLocation.X == 28 && tileLocation.Y == 11)
			{
				__instance.performAction("NatWarpInsect", who, tileLocation);
				__result = true;
				return false;
			}
			__result = false;
			return true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000212E File Offset: 0x0000032E
		private void Pressed(object sender, ButtonPressedEventArgs eventArgs)
		{
			if (this.config.KeyForEncy.IsDown() && Context.IsPlayerFree)
			{
				Game1.activeClickableMenu = new ClickIntoCreatureInfoMenu();
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00009F18 File Offset: 0x00008118
		public static void TreeFell(Tree __instance, Tool t, int explosion, Vector2 tileLocation)
		{
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				NatCreature natCreature = NatureInTheValleyEntry.creatures[i];
				if (Vector2.Distance(natCreature.Position / 64f, __instance.Tile) < 2f && natCreature.GetLocation().Name == __instance.Location.Name && !natCreature.isStatic)
				{
					natCreature.isRunning = true;
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00009F94 File Offset: 0x00008194
		public static void NPC_checkAction(NPC __instance)
		{
			if (__instance.Name == "Jade" && Game1.player.getFriendshipLevelForNPC("Jade") >= 2250 && !Game1.player.hasOrWillReceiveMail("NIVJadeNet") && Game1.player.couldInventoryAcceptThisItem(new NatInValeyJadeNet()))
			{
				Game1.player.mailReceived.Add("NIVJadeNet");
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("Jade"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("JadeMessageNet")));
				Game1.player.addItemToInventory(new NatInValeyJadeNet());
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000A04C File Offset: 0x0000824C
		public bool validLocation(GameLocation location, string code)
		{
			if (code == "1")
			{
				int num = 0;
				if (this.treePos != null)
				{
					num = this.treePos.Count;
				}
				return this.forestLocationNames.Contains(location.Name) || num > 60;
			}
			if (code == "4")
			{
				return this.desertLocationNames.Contains(location.Name);
			}
			if (code == "5")
			{
				return location.IsOutdoors || this.underLocationNames.Contains(location.Name) || (location is MineShaft && (float)(location as MineShaft).mineLevel % 10f == 0f);
			}
			if (code == "3")
			{
				return this.underLocationNames.Contains(location.Name) || (location is MineShaft && (float)(location as MineShaft).mineLevel % 10f == 0f);
			}
			if (code == "0")
			{
				return (location.IsOutdoors || this.forestLocationNames.Contains(location.Name)) && !this.desertLocationNames.Contains(location.Name) && !this.waterLocationNames.Contains(location.Name);
			}
			if (code == "100")
			{
				return location.IsOutdoors || this.forestLocationNames.Contains(location.Name) || this.waterLocationNames.Contains(location.Name) || this.underLocationNames.Contains(location.Name) || (location is MineShaft && (float)(location as MineShaft).mineLevel % 10f == 0f) || this.desertLocationNames.Contains(location.Name);
			}
			if (code == "222")
			{
				return location is VolcanoDungeon;
			}
			if (code == "2")
			{
				return this.waterLocationNames.Contains(location.Name) || ((this.waterPos != null) ? ((float)this.waterPos.Count) : 0f) / (float)Math.Max(this.Tiles, 100) >= 0.4f;
			}
			return location.Name == code;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000A29C File Offset: 0x0000849C
		private void renderPostWorld(object sender, RenderedWorldEventArgs eventArgs)
		{
			if (NatureInTheValleyEntry.sparklingText.Value != null)
			{
				NatureInTheValleyEntry.sparklingText.Value.draw(eventArgs.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, Game1.player.Position + new Vector2(-50f, -192f)));
				this.count++;
				if (this.count > 100)
				{
					this.count = 0;
					NatureInTheValleyEntry.sparklingText.Value = null;
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000A31C File Offset: 0x0000851C
		public static bool DonatedTotalGSQ(string[] query, GameStateQueryContext context)
		{
			int num = 0;
			int num2 = 0;
			GameLocation locationFromName = Game1.getLocationFromName("NIVInnerInsec");
			foreach (KeyValuePair<string, List<string>> keyValuePair in NatureInTheValleyEntry.staticCreatureData)
			{
				string text;
				if (locationFromName.modData.TryGetValue("NatureInTheValley/Donated/" + keyValuePair.Key, out text))
				{
					num++;
				}
				if (keyValuePair.Value.Count <= 44 || keyValuePair.Value[43] == "true")
				{
					num2++;
				}
			}
			return float.Parse(query[1]) <= (float)num / (float)num2;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000A3DC File Offset: 0x000085DC
		public static bool DonatedSpecificGSQ(string[] query, GameStateQueryContext context)
		{
			string text;
			return Game1.getLocationFromName("NIVInnerInsec").modData.TryGetValue("NatureInTheValley/Donated/" + query[1], out text);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000A40C File Offset: 0x0000860C
		public static bool CaughtSpecificGSQ(string[] query, GameStateQueryContext context)
		{
			string text;
			return Game1.player.modData.TryGetValue("NIVCaught" + query[1], out text);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000A438 File Offset: 0x00008638
		public static bool SpawnedSpecificGSQ(string[] query, GameStateQueryContext context)
		{
			using (List<NatCreature>.Enumerator enumerator = NatureInTheValleyEntry.creatures.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name == query[1])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000A498 File Offset: 0x00008698
		public static bool ActionClearCreatures(string[] args, TriggerActionContext context, out string error)
		{
			string text;
			if (!ArgUtility.TryGet(args, 0, out text, out error, false, null))
			{
				return false;
			}
			NatureInTheValleyEntry.creatures.Clear();
			return true;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000A4C0 File Offset: 0x000086C0
		public static bool ActionSpawnSpecific(string[] args, TriggerActionContext context, out string error)
		{
			string text;
			if (!ArgUtility.TryGet(args, 0, out text, out error, false, null))
			{
				return false;
			}
			List<string> list = NatureInTheValleyEntry.staticCreatureData[args[1]];
			NatureInTheValleyEntry.creatures.Add(new NatCreature(new Vector2(float.Parse(args[2]) * 64f, float.Parse(args[3]) * 64f), Game1.currentLocation, args[1], int.Parse(list[0]), list[1] == "true", float.Parse(list[2]), int.Parse(list[3]), float.Parse(list[4]), list[5] == "true", list[6] == "true", int.Parse(list[7]), list[8] == "true", int.Parse(list[12]), int.Parse(list[13]), int.Parse(list[14]), list[15], int.Parse(list[16]), int.Parse(list[17]), int.Parse(list[18]), false, (list.Count > 26) ? int.Parse(list[26]) : 32, (list.Count > 27) ? int.Parse(list[27]) : 32, list[list.Count - 1] == "true", list.Count > 31 && list[30] == "true", list.Count > 32 && list[31] == "true", list.Count > 33 && list[32] == "true", (list.Count > 34) ? int.Parse(list[33]) : 25, (list.Count > 35) ? int.Parse(list[34]) : 0, (list.Count > 37) ? list[36] : "", NatureInTheValleyEntry.staticConfig.creatureSizeMultiplier, (list.Count > 38) ? new List<string>(list[37].Split(",", StringSplitOptions.None)) : new List<string>(), (list.Count > 39) ? float.Parse(list[38]) : 0.1f, list.Count > 45 && list[44] == "true", (list.Count > 46) ? float.Parse(list[45]) : 500f, (list.Count > 47) ? int.Parse(list[46]) : 1150, list.Count > 48 && list[47] == "true", (list.Count > 53) ? new List<string>(list[52].Split(",", StringSplitOptions.None)) : new List<string>()));
			Item item = ItemRegistry.Create("NatInValley.Creature." + args[1], 1, 0, false);
			item.modData["NAT_NIV_NAME"] = args[1];
			item.modData["NAT_NIV_TILEX"] = args[2];
			item.modData["NAT_NIV_TILEY"] = args[3];
			item.modData["NAT_NIV_UNNATURAL"] = "true";
			TriggerActionManager.Raise("NAT_NIV_Spawned", new object[]
			{
				args[1],
				args[2],
				args[3]
			}, null, null, item, null);
			return true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000A874 File Offset: 0x00008A74
		public void CommandFive(string common, string[] args)
		{
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.locationalData)
			{
				base.Monitor.Log(keyValuePair.Key, LogLevel.Warn);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000A8D4 File Offset: 0x00008AD4
		public string HandleRarityChance(Dictionary<string, List<string>> data)
		{
			if (data.Count == 0)
			{
				return "";
			}
			for (int i = 0; i < 30; i++)
			{
				string result;
				List<string> list;
				Utility.TryGetRandom<string, List<string>>(data, out result, out list, null);
				string a = list[0];
				if (a == "0")
				{
					return result;
				}
				if (a == "1")
				{
					if (Game1.random.NextDouble() < 0.75)
					{
						return result;
					}
				}
				else if (a == "2")
				{
					if (Game1.random.NextDouble() < 0.17)
					{
						return result;
					}
				}
				else if (a == "3")
				{
					if (Game1.random.NextDouble() < 0.065)
					{
						return result;
					}
				}
				else if (a == "4" && Game1.random.NextDouble() < 0.023)
				{
					return result;
				}
			}
			return "";
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000A9BC File Offset: 0x00008BBC
		private Vector2 ValidPosition(GameLocation l)
		{
			for (int i = 0; i < 10; i++)
			{
				Vector2 vector = new Vector2((float)Game1.random.Next(Game1.currentLocation.map.Layers[0].TileWidth), (float)Game1.random.Next(Game1.currentLocation.map.Layers[0].TileHeight));
				if (l.CanSpawnCharacterHere(vector))
				{
					return vector;
				}
			}
			return Vector2.Zero;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000AA38 File Offset: 0x00008C38
		public void TrySpawnFromArea(GameLocation location)
		{
			string text;
			if (this.CreatureSpawnList.Count <= this.spawnedInLoc)
			{
				text = this.HandleRarityChance(this.locationalData);
				if (text == "")
				{
					return;
				}
			}
			else
			{
				text = this.CreatureSpawnList[this.spawnedInLoc];
			}
			this.spawnedInLoc++;
			CreatureModel creatureModel = this.ModelData[text];
			string localSpawnCode = creatureModel.localSpawnCode;
			if (localSpawnCode == "0")
			{
				Vector2 value = this.ValidPosition(location);
				if (value != Vector2.Zero)
				{
					this.Instantiate(text, value + new Vector2(-0.25f, 0.25f), location);
					for (int i = 0; i < creatureModel.packSize - 1; i++)
					{
						Vector2 vector = value + new Vector2(-0.25f, 0.25f) + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
						if (this.isValidPosition(location, vector))
						{
							this.Instantiate(text, vector, location);
						}
					}
				}
				return;
			}
			if (localSpawnCode == "1")
			{
				List<Vector2> list = (this.treePos != null) ? this.treePos : this.GetTrees(location);
				if (list.Count < 2)
				{
					return;
				}
				Vector2 vector2 = list[(int)(Game1.random.NextDouble() * (double)(list.Count - 1))] + new Vector2(-0.25f, 0.55f);
				if (this.CheckForBugAtTile(location, vector2 * 64f, 64f))
				{
					return;
				}
				this.Instantiate(text, vector2, location);
				for (int j = 0; j < creatureModel.packSize - 1; j++)
				{
					Vector2 vector3 = vector2 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
					if (this.isValidPosition(location, vector3))
					{
						this.Instantiate(text, vector3, location);
					}
				}
				return;
			}
			else if (localSpawnCode == "2")
			{
				List<Vector2> list2 = (this.bushPos != null) ? this.bushPos : this.GetBushes(location);
				if (list2.Count < 2)
				{
					return;
				}
				Vector2 vector4 = list2[Game1.random.Next(list2.Count - 1)];
				if (this.CheckForBugAtTile(location, vector4 * 64f, 64f))
				{
					return;
				}
				this.Instantiate(text, vector4, location);
				for (int k = 0; k < creatureModel.packSize - 1; k++)
				{
					Vector2 vector5 = vector4 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
					if (this.isValidPosition(location, vector5))
					{
						this.Instantiate(text, vector5, location);
					}
				}
				return;
			}
			else if (localSpawnCode == "4")
			{
				List<Vector2> list3 = (this.stumpPos != null) ? this.stumpPos : this.GetStumps(location);
				if (list3.Count < 2)
				{
					return;
				}
				Vector2 vector6 = list3[Game1.random.Next(list3.Count - 1)];
				if (this.CheckForBugAtTile(location, vector6 * 64f, 64f))
				{
					return;
				}
				this.Instantiate(text, vector6, location);
				for (int l = 0; l < creatureModel.packSize - 1; l++)
				{
					Vector2 vector7 = vector6 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
					if (this.isValidPosition(location, vector7))
					{
						this.Instantiate(text, vector7, location);
					}
				}
				return;
			}
			else
			{
				List<Vector2> list4 = (this.waterPos != null) ? this.waterPos : this.GetWater(location);
				if (list4.Count < 2)
				{
					return;
				}
				Vector2 vector8 = list4[Game1.random.Next(list4.Count - 1)];
				this.Instantiate(text, vector8, location);
				for (int m = 0; m < creatureModel.packSize - 1; m++)
				{
					Vector2 vector9 = vector8 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
					if (this.isValidPosition(location, vector9))
					{
						this.Instantiate(text, vector9, location);
					}
				}
				return;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002153 File Offset: 0x00000353
		private bool isValidPosition(GameLocation l, Vector2 vector)
		{
			return l.CanSpawnCharacterHere(vector);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000215C File Offset: 0x0000035C
		public string DecoratorName()
		{
			return this.helper.Translation.Get("DecoratorTT");
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002178 File Offset: 0x00000378
		public string DecoratorDesc()
		{
			return this.helper.Translation.Get("DecoratorDesc");
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000AE74 File Offset: 0x00009074
		public Texture2D Decorate(Item input)
		{
			if (input.Category != -81 || !input.ItemId.Contains("NatInValley.Creature."))
			{
				return null;
			}
			string text;
			if (!Game1.getLocationFromName("NIVInnerInsec").modData.TryGetValue("NatureInTheValley/Donated/" + input.Name, out text))
			{
				return this.helper.ModContent.Load<Texture2D>("PNGs/Icon.png");
			}
			return null;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000AEE0 File Offset: 0x000090E0
		public void OnAssetsInvalidated(object sender, AssetsInvalidatedEventArgs e)
		{
			using (IEnumerator<IAssetName> enumerator = e.NamesWithoutLocale.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsEquivalentTo("Nature.NITV/Creatures", false))
					{
						foreach (KeyValuePair<string, CreatureModel> keyValuePair in Game1.content.Load<Dictionary<string, CreatureModel>>("Nature.NITV/Creatures"))
						{
							List<string> value = new List<string>
							{
								keyValuePair.Value.rarity.ToString(),
								keyValuePair.Value.grounded.ToString().ToLower(),
								keyValuePair.Value.speed.ToString(),
								keyValuePair.Value.pauseTime.ToString(),
								keyValuePair.Value.scale.ToString(),
								keyValuePair.Value.doesRun.ToString().ToLower(),
								keyValuePair.Value.isMover.ToString().ToLower(),
								keyValuePair.Value.range.ToString(),
								keyValuePair.Value.dangerous.ToString().ToLower(),
								keyValuePair.Value.seasons.Join(null, ","),
								keyValuePair.Value.weatherCode,
								keyValuePair.Value.locations.Join(null, ","),
								keyValuePair.Value.minTime.ToString(),
								keyValuePair.Value.maxTime.ToString(),
								keyValuePair.Value.frames.ToString(),
								keyValuePair.Value.spritePath,
								keyValuePair.Value.xShadow.ToString(),
								keyValuePair.Value.localSpawnCode,
								keyValuePair.Value.yShadow.ToString(),
								keyValuePair.Value.price.ToString(),
								keyValuePair.Value.spriteIndex.ToString(),
								keyValuePair.Value.xDef.ToString(),
								keyValuePair.Value.yDef.ToString(),
								keyValuePair.Value.itemTexture.ToString(),
								keyValuePair.Value.displayName,
								keyValuePair.Value.displayDescription,
								keyValuePair.Value.xSpriteSize.ToString(),
								keyValuePair.Value.ySpriteSize.ToString(),
								keyValuePair.Value.GSQ,
								keyValuePair.Value.packSize.ToString(),
								keyValuePair.Value.rotaryAnims.ToString().ToLower(),
								keyValuePair.Value.complexIdling.ToString().ToLower(),
								keyValuePair.Value.friendlyFollower.ToString().ToLower(),
								keyValuePair.Value.dangerDamage.ToString(),
								keyValuePair.Value.health.ToString(),
								keyValuePair.Value.forceSword.ToString().ToLower(),
								keyValuePair.Value.cueName,
								keyValuePair.Value.variantList.Join(null, ","),
								keyValuePair.Value.variantChance.ToString(),
								keyValuePair.Value.alternativeDrop,
								keyValuePair.Value.onlyAlternativeDrop.ToString().ToLower(),
								keyValuePair.Value.alternativeDropChance.ToString(),
								keyValuePair.Value.isTerrariumable.ToString().ToLower(),
								keyValuePair.Value.isDonatable.ToString().ToLower(),
								keyValuePair.Value.semiAquatic.ToString().ToLower(),
								keyValuePair.Value.soundRange.ToString(),
								keyValuePair.Value.soundFrequency.ToString(),
								keyValuePair.Value.useBetterCreatureBounds.ToString().ToLower(),
								keyValuePair.Value.displayedLocation,
								keyValuePair.Value.displayedLocalLocation,
								keyValuePair.Value.secondStartTime.ToString(),
								keyValuePair.Value.secondEndTime.ToString(),
								keyValuePair.Value.variantGSQs.Join(null, ","),
								keyValuePair.Value.extraDescription,
								keyValuePair.Value.compelxAnims.ToString().ToLower()
							};
							if (this.creatureData.ContainsKey(keyValuePair.Key))
							{
								this.creatureData.Remove(keyValuePair.Key);
							}
							this.creatureData.Add(keyValuePair.Key, value);
						}
						this.MakeModelDataFromData();
						NatureInTheValleyEntry.staticCreatureData = this.creatureData;
						this.helper.GameContent.InvalidateCache("Data/Furniture");
						this.helper.GameContent.InvalidateCache("Data/Objects");
					}
					if (enumerator.Current.IsEquivalentTo("Nature.NITV/Rewards", false))
					{
						foreach (KeyValuePair<string, RewardModel> keyValuePair2 in Game1.content.Load<Dictionary<string, RewardModel>>("Nature.NITV/Rewards"))
						{
							if (NatureInTheValleyEntry.customRewards.ContainsKey(keyValuePair2.Key))
							{
								NatureInTheValleyEntry.customRewards.Remove(keyValuePair2.Key);
							}
							NatureInTheValleyEntry.customRewards.Add(keyValuePair2.Key, keyValuePair2.Value);
						}
					}
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000B670 File Offset: 0x00009870
		public static bool ActionTryCatch(string[] args, TriggerActionContext context, out string error)
		{
			string text;
			if (!ArgUtility.TryGet(args, 0, out text, out error, false, null))
			{
				return false;
			}
			Vector2 value = new Vector2(float.Parse(args[1]), float.Parse(args[2]));
			Farmer player = Game1.player;
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (player.currentLocation.Name == "NIVInnerInsec" || (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 36 && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][35] == "true"))
				{
					return true;
				}
				if (NatureInTheValleyEntry.creatures[i].GetLocation().Name == player.currentLocation.Name && (double)Vector2.Distance(NatureInTheValleyEntry.creatures[i].GetEffectivePosition() + (NatureInTheValleyEntry.creatures[i].IsGrounded ? Vector2.Zero : new Vector2(0f, -30f)), value) < (double)((NatureInTheValleyEntry.creatures[i].IsGrounded ? 80f : 105f) * NatureInTheValleyEntry.staticConfig.catchingDifficultyMultiplier) * Math.Sqrt((double)NatureInTheValleyEntry.creatures[i].scale))
				{
					if (!NatureInTheValleyEntry.creatures[i].released)
					{
						string text2 = "";
						if (!Game1.player.modData.TryGetValue("NIVCaught" + NatureInTheValleyEntry.creatures[i].name, out text2))
						{
							NatureInTheValleyEntry.sparklingText.Value = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\1_6_Strings:FirstCatch"), new Color(200, 255, 220), Color.White, true, 0.3, 2500, -1, 500, 1f);
							Game1.player.modData["NIVCaught" + NatureInTheValleyEntry.creatures[i].name] = "true";
							Game1.player.gainExperience(2, 11 + (int)Math.Pow(4.0, (double)NatureInTheValleyEntry.creatures[i].Rarity));
							Game1.playSound("discoverMineral", null);
						}
						else
						{
							Game1.playSound("jingle1", null);
							Game1.player.gainExperience(2, 3 + (int)Math.Pow(4.0, (double)NatureInTheValleyEntry.creatures[i].Rarity));
						}
						if (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 40 && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][39] != "" && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 42 && Game1.random.NextDouble() <= (double)float.Parse(NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][41]))
						{
							Item item = ItemRegistry.Create(NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][39], 1, 0, false);
							Game1.player.addItemByMenuIfNecessary(item, null, false);
						}
					}
					if (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count <= 41 || NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][40] == "false")
					{
						Game1.player.addItemByMenuIfNecessary(ItemRegistry.Create("NatInValley.Creature." + NatureInTheValleyEntry.creatures[i].name, 1, 0, false), null, false);
					}
					Item item2 = ItemRegistry.Create("NatInValley.Creature." + NatureInTheValleyEntry.creatures[i].name, 1, 0, false);
					item2.modData["NAT_NIV_NAME"] = NatureInTheValleyEntry.creatures[i].name;
					TriggerActionManager.Raise("NAT_NIV_Caught", null, null, null, item2, null);
					NatureInTheValleyEntry.creatures.RemoveAt(i);
					i--;
				}
				else if (NatureInTheValleyEntry.creatures[i].GetLocation() == player.currentLocation && Vector2.Distance(NatureInTheValleyEntry.creatures[i].GetEffectivePosition() + new Vector2(16f, 16f) + (NatureInTheValleyEntry.creatures[i].IsGrounded ? Vector2.Zero : new Vector2(0f, -32f)), value) < 180f && !NatureInTheValleyEntry.creatures[i].isRunning && !NatureInTheValleyEntry.creatures[i].Dangerous && !NatureInTheValleyEntry.creatures[i].isStatic)
				{
					NatureInTheValleyEntry.creatures[i].isRunning = true;
				}
			}
			return true;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002194 File Offset: 0x00000394
		public static bool ExistsGSQ(string[] query, GameStateQueryContext context)
		{
			return NatureInTheValleyEntry.staticCreatureData.ContainsKey(query[1]);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000BBC8 File Offset: 0x00009DC8
		private static bool baitFix(FishingRod __instance, StardewValley.Object o, int slot, ref bool __result)
		{
			if (o.Category == -81 && slot == 0 && __instance.CanUseBait() && o.ItemId.Contains("NatInValley.Creature.") && (o.ItemId.ToLower().Contains("worm") || o.ItemId.ToLower().Contains("fly") || o.ItemId.ToLower().Contains("caterpillar") || o.ItemId.ToLower().Contains("centipede") || o.ItemId.ToLower().Contains("millipede")))
			{
				__result = true;
				return false;
			}
			return true;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000BC80 File Offset: 0x00009E80
		public static bool ClearSpecific(string[] args, TriggerActionContext context, out string error)
		{
			string text;
			if (!ArgUtility.TryGet(args, 0, out text, out error, false, null))
			{
				return false;
			}
			int num = 0;
			int num2 = (args.Length > 2) ? int.Parse(args[2]) : 1;
			for (int i = 1; i <= NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (NatureInTheValleyEntry.creatures[NatureInTheValleyEntry.creatures.Count - i].name == args[1] && num < num2 && !NatureInTheValleyEntry.creatures[i].isStatic)
				{
					num++;
					NatureInTheValleyEntry.creatures.Remove(NatureInTheValleyEntry.creatures[NatureInTheValleyEntry.creatures.Count - i]);
					i--;
				}
			}
			return true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000021A3 File Offset: 0x000003A3
		public static bool TriggerSpawnByName(string[] query, GameStateQueryContext context)
		{
			return query[2] == context.TargetItem.modData["NAT_NIV_NAME"];
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000BD2C File Offset: 0x00009F2C
		public static bool ActionSpawnSpecificNearTriggered(string[] args, TriggerActionContext context, out string error)
		{
			int num = int.Parse(args[2]);
			Vector2 vector = new Vector2((float.Parse(context.TriggerArgs[1].ToString()) + (float)Game1.random.Next(-num, num)) * 64f, (float.Parse(context.TriggerArgs[2].ToString()) + (float)Game1.random.Next(-num, num)) * 64f);
			string text;
			if (!ArgUtility.TryGet(args, 0, out text, out error, false, null))
			{
				return false;
			}
			List<string> list = NatureInTheValleyEntry.staticCreatureData[args[1]];
			NatureInTheValleyEntry.creatures.Add(new NatCreature(vector, Game1.currentLocation, args[1], int.Parse(list[0]), list[1] == "true", float.Parse(list[2]), int.Parse(list[3]), float.Parse(list[4]), list[5] == "true", list[6] == "true", int.Parse(list[7]), list[8] == "true", int.Parse(list[12]), int.Parse(list[13]), int.Parse(list[14]), list[15], int.Parse(list[16]), int.Parse(list[17]), int.Parse(list[18]), false, (list.Count > 26) ? int.Parse(list[26]) : 32, (list.Count > 27) ? int.Parse(list[27]) : 32, list[list.Count - 1] == "true", list.Count > 31 && list[30] == "true", list.Count > 32 && list[31] == "true", list.Count > 33 && list[32] == "true", (list.Count > 34) ? int.Parse(list[33]) : 25, (list.Count > 35) ? int.Parse(list[34]) : 0, (list.Count > 37) ? list[36] : "", NatureInTheValleyEntry.staticConfig.creatureSizeMultiplier, (list.Count > 38) ? new List<string>(list[37].Split(",", StringSplitOptions.None)) : new List<string>(), (list.Count > 39) ? float.Parse(list[38]) : 0.1f, list.Count > 45 && list[44] == "true", (list.Count > 46) ? float.Parse(list[45]) : 500f, (list.Count > 47) ? int.Parse(list[46]) : 1150, list.Count > 48 && list[47] == "true", (list.Count > 53) ? new List<string>(list[52].Split(",", StringSplitOptions.None)) : new List<string>()));
			Item item = ItemRegistry.Create("NatInValley.Creature." + args[1], 1, 0, false);
			item.modData["NAT_NIV_NAME"] = args[1];
			item.modData["NAT_NIV_TILEX"] = vector.X.ToString();
			item.modData["NAT_NIV_TILEY"] = vector.ToString();
			TriggerActionManager.Raise("NAT_NIV_Spawned", new object[]
			{
				args[1],
				vector.X,
				vector.Y
			}, null, null, item, null);
			return true;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000C130 File Offset: 0x0000A330
		public static bool TriggerSpawnByTile(string[] query, GameStateQueryContext context)
		{
			return Vector2.Distance(new Vector2(float.Parse(query[1]), float.Parse(query[2])), new Vector2(float.Parse(context.TargetItem.modData["NAT_NIV_TILEX"]), float.Parse(context.TargetItem.modData["NAT_NIV_TILEY"]))) < 1.5f;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000C198 File Offset: 0x0000A398
		private void MakeCreatueSpawnList()
		{
			this.CreatureSpawnList.Clear();
			for (int i = 0; i < 30; i++)
			{
				string text = this.HandleRarityChance(this.locationalData);
				if (text != "")
				{
					this.CreatureSpawnList.Add(text);
				}
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000021C2 File Offset: 0x000003C2
		public static bool TriggerNaturalByName(string[] query, GameStateQueryContext context)
		{
			return !context.TargetItem.modData.ContainsKey("NAT_NIV_UNNATURAL");
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
		private void MakeModelDataFromData()
		{
			this.ModelData.Clear();
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				CreatureModel creatureModel = new CreatureModel();
				creatureModel.rarity = int.Parse(keyValuePair.Value[0]);
				creatureModel.grounded = (keyValuePair.Value[1] == "true");
				creatureModel.speed = float.Parse(keyValuePair.Value[2]);
				creatureModel.pauseTime = int.Parse(keyValuePair.Value[3]);
				creatureModel.scale = float.Parse(keyValuePair.Value[4]);
				creatureModel.doesRun = (keyValuePair.Value[5] == "true");
				creatureModel.isMover = (keyValuePair.Value[6] == "true");
				creatureModel.range = int.Parse(keyValuePair.Value[7]);
				creatureModel.dangerous = (keyValuePair.Value[8] == "true");
				creatureModel.seasons = new List<string>(keyValuePair.Value[9].Split(",", StringSplitOptions.None));
				creatureModel.weatherCode = keyValuePair.Value[10];
				creatureModel.locations = new List<string>(keyValuePair.Value[11].Split(",", StringSplitOptions.None));
				creatureModel.minTime = int.Parse(keyValuePair.Value[12]);
				creatureModel.maxTime = int.Parse(keyValuePair.Value[13]);
				creatureModel.frames = int.Parse(keyValuePair.Value[14]);
				creatureModel.spritePath = keyValuePair.Value[15];
				creatureModel.xShadow = int.Parse(keyValuePair.Value[16]);
				creatureModel.localSpawnCode = keyValuePair.Value[17];
				creatureModel.yShadow = int.Parse(keyValuePair.Value[18]);
				creatureModel.price = int.Parse(keyValuePair.Value[19]);
				creatureModel.spriteIndex = int.Parse(keyValuePair.Value[20]);
				creatureModel.xDef = float.Parse(keyValuePair.Value[21]);
				creatureModel.yDef = float.Parse(keyValuePair.Value[22]);
				creatureModel.itemTexture = ((keyValuePair.Value.Count > 23) ? keyValuePair.Value[23] : "Mods\\NatureInTheValley\\Creatures\\Items");
				creatureModel.displayName = ((keyValuePair.Value.Count > 24) ? keyValuePair.Value[24] : this.helper.Translation.Get(keyValuePair.Key + ".Name"));
				creatureModel.displayDescription = ((keyValuePair.Value.Count > 25) ? keyValuePair.Value[25] : this.helper.Translation.Get(keyValuePair.Key + ".Description"));
				creatureModel.xSpriteSize = ((keyValuePair.Value.Count > 26) ? int.Parse(keyValuePair.Value[26]) : 32);
				creatureModel.ySpriteSize = ((keyValuePair.Value.Count > 27) ? int.Parse(keyValuePair.Value[27]) : 32);
				creatureModel.GSQ = ((keyValuePair.Value.Count > 29) ? keyValuePair.Value[28] : " ");
				creatureModel.packSize = ((keyValuePair.Value.Count > 30) ? int.Parse(keyValuePair.Value[29]) : 1);
				creatureModel.rotaryAnims = (keyValuePair.Value.Count > 31 && keyValuePair.Value[30] == "true");
				creatureModel.complexIdling = (keyValuePair.Value.Count > 32 && keyValuePair.Value[31] == "true");
				creatureModel.friendlyFollower = (keyValuePair.Value.Count > 33 && keyValuePair.Value[32] == "true");
				creatureModel.dangerDamage = ((keyValuePair.Value.Count > 34) ? int.Parse(keyValuePair.Value[33]) : 25);
				creatureModel.health = ((keyValuePair.Value.Count > 35) ? int.Parse(keyValuePair.Value[34]) : 0);
				creatureModel.forceSword = (keyValuePair.Value.Count > 36 && keyValuePair.Value[35] == "true");
				creatureModel.cueName = ((keyValuePair.Value.Count > 37) ? keyValuePair.Value[36] : "");
				creatureModel.variantList = ((keyValuePair.Value.Count > 38) ? new List<string>(keyValuePair.Value[37].Split(",", StringSplitOptions.None)) : new List<string>());
				creatureModel.variantChance = ((keyValuePair.Value.Count > 39) ? float.Parse(keyValuePair.Value[38]) : 0.1f);
				creatureModel.alternativeDrop = ((keyValuePair.Value.Count > 40) ? keyValuePair.Value[39] : "");
				creatureModel.onlyAlternativeDrop = (keyValuePair.Value.Count > 41 && keyValuePair.Value[40] == "true");
				creatureModel.alternativeDropChance = ((keyValuePair.Value.Count > 42) ? float.Parse(keyValuePair.Value[41]) : 1f);
				creatureModel.isTerrariumable = (keyValuePair.Value.Count <= 43 || keyValuePair.Value[42] == "true");
				creatureModel.isDonatable = (keyValuePair.Value.Count <= 44 || keyValuePair.Value[43] == "true");
				creatureModel.semiAquatic = (keyValuePair.Value.Count > 45 && keyValuePair.Value[44] == "true");
				creatureModel.soundRange = ((keyValuePair.Value.Count > 46) ? float.Parse(keyValuePair.Value[45]) : 500f);
				creatureModel.soundFrequency = ((keyValuePair.Value.Count > 47) ? int.Parse(keyValuePair.Value[46]) : 1150);
				creatureModel.useBetterCreatureBounds = (keyValuePair.Value.Count > 48 && keyValuePair.Value[47] == "true");
				creatureModel.displayedLocation = ((keyValuePair.Value.Count > 49) ? keyValuePair.Value[48] : "");
				creatureModel.displayedLocalLocation = ((keyValuePair.Value.Count > 50) ? keyValuePair.Value[49] : "");
				creatureModel.secondStartTime = ((keyValuePair.Value.Count > 51) ? int.Parse(keyValuePair.Value[50]) : 0);
				creatureModel.secondEndTime = ((keyValuePair.Value.Count > 52) ? int.Parse(keyValuePair.Value[51]) : 0);
				creatureModel.variantGSQs = ((keyValuePair.Value.Count > 53) ? new List<string>(keyValuePair.Value[52].Split(",", StringSplitOptions.None)) : new List<string>());
				creatureModel.extraDescription = ((keyValuePair.Value.Count > 54) ? keyValuePair.Value[53] : "");
				creatureModel.compelxAnims = (keyValuePair.Value[keyValuePair.Value.Count - 1] == "true");
				this.ModelData.Add(keyValuePair.Key, creatureModel);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000CACC File Offset: 0x0000ACCC
		private static void damageCreeps(GameLocation __instance, Microsoft.Xna.Framework.Rectangle areaOfEffect, int minDamage, int maxDamage, bool isBomb, float knockBackModifier, int addedPrecision, float critChance, float critMultiplier, bool triggerMonsterInvincibleTimer, Farmer who)
		{
			if (minDamage < 10)
			{
				return;
			}
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (!NatureInTheValleyEntry.creatures[i].isStatic && NatureInTheValleyEntry.creatures[i].Dangerous && Vector2.Distance(NatureInTheValleyEntry.creatures[i].GetEffectivePosition(), areaOfEffect.Center.ToVector2()) < 70f)
				{
					bool flag = false;
					if (who != null && Game1.random.NextDouble() < (double)critChance + (double)who.LuckLevel * ((double)critChance / 40.0))
					{
						flag = true;
						__instance.playSound("crit", null, null, SoundContext.Default);
					}
					int num = Game1.random.Next(minDamage, maxDamage + 1);
					int num2 = Math.Max(1, (flag ? ((int)((double)num * (double)critMultiplier)) : num) + ((who != null) ? (who.Attack * 3) : 0));
					if (who != null && who.professions.Contains(24))
					{
						num2 = (int)Math.Ceiling((double)num2 * 1.1000000238418579);
					}
					if (who != null && who.professions.Contains(26))
					{
						num2 = (int)Math.Ceiling((double)num2 * 1.1499999761581421);
					}
					if (who != null && flag && who.professions.Contains(29))
					{
						num2 = (int)((double)num2 * 2.0);
					}
					if (NatureInTheValleyEntry.creatures[i].maxHealth == 0)
					{
						NatureInTheValleyEntry.creatures[i].isRunning = true;
					}
					else
					{
						__instance.debris.Add(new Debris(num2, NatureInTheValleyEntry.creatures[i].GetEffectivePosition(), flag ? Color.Yellow : new Color(255, 130, 0), flag ? ((float)(1.0 + (double)num2 / 300.0)) : 1f, null));
						num2 /= 2;
						NatureInTheValleyEntry.creatures[i].health -= num2;
						if (NatureInTheValleyEntry.creatures[i].health < 0)
						{
							if (!NatureInTheValleyEntry.creatures[i].released)
							{
								string text = "";
								if (!Game1.player.modData.TryGetValue("NIVCaught" + NatureInTheValleyEntry.creatures[i].name, out text))
								{
									NatureInTheValleyEntry.sparklingText.Value = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\1_6_Strings:FirstCatch"), new Color(200, 255, 220), Color.White, true, 0.3, 2500, -1, 500, 1f);
									Game1.player.modData["NIVCaught" + NatureInTheValleyEntry.creatures[i].name] = "true";
									Game1.player.gainExperience(2, 11 + (int)Math.Pow(4.0, (double)NatureInTheValleyEntry.creatures[i].Rarity));
									Game1.playSound("discoverMineral", null);
								}
								else
								{
									Game1.playSound("jingle1", null);
									Game1.player.gainExperience(2, 3 + (int)Math.Pow(4.0, (double)NatureInTheValleyEntry.creatures[i].Rarity));
								}
								if (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 40 && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][39] != "" && NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count > 42 && Game1.random.NextDouble() <= (double)float.Parse(NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][41]))
								{
									Item item = ItemRegistry.Create(NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][39], 1, 0, false);
									Game1.player.addItemByMenuIfNecessary(item, null, false);
								}
							}
							if (NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name].Count <= 41 || NatureInTheValleyEntry.staticCreatureData[NatureInTheValleyEntry.creatures[i].name][40] == "false")
							{
								Game1.player.addItemByMenuIfNecessary(ItemRegistry.Create("NatInValley.Creature." + NatureInTheValleyEntry.creatures[i].name, 1, 0, false), null, false);
							}
							Item item2 = ItemRegistry.Create("NatInValley.Creature." + NatureInTheValleyEntry.creatures[i].name, 1, 0, false);
							item2.modData["NAT_NIV_NAME"] = NatureInTheValleyEntry.creatures[i].name;
							TriggerActionManager.Raise("NAT_NIV_Caught", null, null, null, item2, null);
							NatureInTheValleyEntry.creatures.RemoveAt(i);
							i--;
						}
						else
						{
							Vector2 position = NatureInTheValleyEntry.creatures[i].Position + (NatureInTheValleyEntry.creatures[i].Position - who.Position) / Vector2.Distance(NatureInTheValleyEntry.creatures[i].Position, who.Position) * 16f;
							if (NatureInTheValleyEntry.creatures[i].ValidPosition(position))
							{
								NatureInTheValleyEntry.creatures[i].Position = position;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000D0B4 File Offset: 0x0000B2B4
		public void ClearUnclaimedCreatures(GameLocation newlocation)
		{
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (NatureInTheValleyEntry.creatures[i].GetLocation().Name != newlocation.Name && !NatureInTheValleyEntry.creatures[i].released)
				{
					NatureInTheValleyEntry.creatures.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000D11C File Offset: 0x0000B31C
		public static bool TriggerArbitraryByName(string[] query, GameStateQueryContext context)
		{
			List<string> list = NatureInTheValleyEntry.staticCreatureData[context.TargetItem.modData["NAT_NIV_NAME"]];
			return list.Count > int.Parse(query[2]) && list[int.Parse(query[2])].Replace("'", ",") == query[3];
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000D180 File Offset: 0x0000B380
		public static bool creatureMakerDrop(InventoryPage __instance, int x, int y)
		{
			if (!__instance.isWithinBounds(x, y) && Game1.player.CursorSlotItem != null && Game1.player.CursorSlotItem.canBeTrashed() && Game1.player.CursorSlotItem.Category == -81 && Game1.player.CursorSlotItem.ItemId.Contains("NatInValley.Creature.") && NatureInTheValleyEntry.staticCreatureData.ContainsKey(Game1.player.CursorSlotItem.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]) && Game1.currentLocation.Name != "NIVInnerInsec")
			{
				Game1.playSound("throwDownITem", null);
				Item cursorSlotItem = Game1.player.CursorSlotItem;
				for (int i = 0; i < cursorSlotItem.Stack; i++)
				{
					List<string> list = NatureInTheValleyEntry.staticCreatureData[cursorSlotItem.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]];
					NatCreature natCreature = new NatCreature(Game1.player.Position, Game1.currentLocation, cursorSlotItem.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1], int.Parse(list[0]), list[1] == "true", float.Parse(list[2]), int.Parse(list[3]), float.Parse(list[4]), list[5] == "true", list[6] == "true", int.Parse(list[7]), list[8] == "true", int.Parse(list[12]), int.Parse(list[13]), int.Parse(list[14]), list[15], int.Parse(list[16]), int.Parse(list[17]), int.Parse(list[18]), true, (list.Count > 26) ? int.Parse(list[26]) : 32, (list.Count > 27) ? int.Parse(list[27]) : 32, list[list.Count - 1] == "true", list.Count > 31 && list[30] == "true", list.Count > 32 && list[31] == "true", list.Count > 33 && list[32] == "true", (list.Count > 34) ? int.Parse(list[33]) : 25, (list.Count > 35) ? int.Parse(list[34]) : 0, (list.Count > 37) ? list[36] : "", NatureInTheValleyEntry.staticConfig.creatureSizeMultiplier, (list.Count > 38) ? new List<string>(list[37].Split(",", StringSplitOptions.None)) : new List<string>(), (list.Count > 39) ? float.Parse(list[38]) : 0.1f, list.Count > 45 && list[44] == "true", (list.Count > 46) ? float.Parse(list[45]) : 500f, (list.Count > 47) ? int.Parse(list[46]) : 1150, list.Count > 48 && list[47] == "true", (list.Count > 53) ? new List<string>(list[52].Split(",", StringSplitOptions.None)) : new List<string>());
					natCreature.released = true;
					NatureInTheValleyEntry.creatures.Add(natCreature);
				}
				Game1.player.CursorSlotItem = null;
				return false;
			}
			return true;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000D598 File Offset: 0x0000B798
		private static void Ticking(GameTime gameTime)
		{
			if (!Game1.shouldTimePass(false) || Game1.currentLocation == null)
			{
				return;
			}
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (NatureInTheValleyEntry.creatures[i].GetLocation().Name == Game1.currentLocation.Name && Vector2.Distance(NatureInTheValleyEntry.creatures[i].Position, Game1.player.Position) < 2000f)
				{
					NatureInTheValleyEntry.creatures[i].Update(gameTime);
					NatureInTheValleyEntry.creatures[i].UpdateAnim(gameTime);
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000D638 File Offset: 0x0000B838
		private static bool actionCover(StardewValley.Object __instance, Farmer who, ref bool __result, bool justCheckingForActivity = false)
		{
			if (!__instance.ItemId.Contains("Tera.NatInValley.Creature.") || __instance.Location == null)
			{
				return true;
			}
			if (__instance.isTemporarilyInvisible || justCheckingForActivity)
			{
				__result = true;
				return false;
			}
			__result = true;
			who.currentLocation.createQuestionDialogue(NatureInTheValleyEntry.staticHelper.Translation.Get("ReleaseQuestion"), who.currentLocation.createYesNoResponses(), string.Concat(new object[]
			{
				"TeraRelease_",
				__instance.TileLocation.X,
				"_",
				__instance.TileLocation.Y,
				"_",
				__instance.ItemId.Split("Tera.NatInValley.Creature.", StringSplitOptions.None)[1]
			}));
			return false;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000D704 File Offset: 0x0000B904
		public static string descriptCover(string __result, StardewValley.Object __instance)
		{
			if (!__instance.ItemId.Contains("NatInValley.Creature.") || Game1.getLocationFromName("NIVInnerInsec").modData.ContainsKey("NatureInTheValley/Donated/" + __instance.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]))
			{
				return __result;
			}
			return __result + "\n" + NatureInTheValleyEntry.staticHelper.Translation.Get("DonatableDesc");
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000D77C File Offset: 0x0000B97C
		public void setupTempCharacters(GameLocation location, int donated, int total)
		{
			if ((float)(donated / total) >= 0.05f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 == 0)
				{
					location.setTileProperty(46, 14, "Back", "Passable", "T");
					location.setTileProperty(47, 14, "Back", "Passable", "T");
					location.setMapTile(46, 14, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.1\"", false);
					location.setMapTile(47, 14, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.2\"", false);
					if (location.hasTileAt(16, 18, "Buildings", null))
					{
						location.removeTileProperty(16, 18, "Back", "Passable");
						location.removeMapTile(16, 18, "Buildings");
					}
					if (location.hasTileAt(17, 18, "Buildings", null))
					{
						location.removeTileProperty(17, 18, "Back", "Passable");
						location.removeMapTile(17, 18, "Buildings");
					}
					location.map.RemoveLayer(location.map.GetLayer("Front1618"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings1618"));
				}
				else
				{
					location.setTileProperty(16, 18, "Back", "Passable", "T");
					location.setTileProperty(17, 18, "Back", "Passable", "T");
					location.setMapTile(16, 18, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.1\"", false);
					location.setMapTile(17, 18, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.2\"", false);
					if (location.hasTileAt(46, 14, "Buildings", null))
					{
						location.removeTileProperty(46, 14, "Back", "Passable");
						location.removeMapTile(46, 14, "Buildings");
					}
					if (location.hasTileAt(47, 14, "Buildings", null))
					{
						location.removeTileProperty(47, 14, "Back", "Passable");
						location.removeMapTile(47, 14, "Buildings");
					}
					location.map.RemoveLayer(location.map.GetLayer("Front4614"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings4614"));
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front4614"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings4614"));
				location.map.RemoveLayer(location.map.GetLayer("Front1618"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings1618"));
				if (location.hasTileAt(46, 14, "Buildings", null))
				{
					location.removeTileProperty(46, 14, "Back", "Passable");
					location.removeMapTile(46, 14, "Buildings");
				}
				if (location.hasTileAt(47, 14, "Buildings", null))
				{
					location.removeTileProperty(47, 14, "Back", "Passable");
					location.removeMapTile(47, 14, "Buildings");
				}
				if (location.hasTileAt(16, 18, "Buildings", null))
				{
					location.removeTileProperty(16, 18, "Back", "Passable");
					location.removeMapTile(16, 18, "Buildings");
				}
				if (location.hasTileAt(17, 18, "Buildings", null))
				{
					location.removeTileProperty(17, 18, "Back", "Passable");
					location.removeMapTile(17, 18, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.1f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 != 0)
				{
					location.setTileProperty(64, 36, "Back", "Passable", "T");
					location.setMapTile(64, 36, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.3\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front6815"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings6815"));
					if (location.hasTileAt(68, 15, "Buildings", null))
					{
						location.removeTileProperty(68, 15, "Back", "Passable");
						location.removeMapTile(68, 15, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(68, 15, "Back", "Passable", "T");
					location.setMapTile(68, 15, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.3\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front6436"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings6436"));
					if (location.hasTileAt(64, 36, "Buildings", null))
					{
						location.removeTileProperty(64, 36, "Back", "Passable");
						location.removeMapTile(64, 36, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front6815"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings6815"));
				location.map.RemoveLayer(location.map.GetLayer("Front6436"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings6436"));
				if (location.hasTileAt(68, 15, "Buildings", null))
				{
					location.removeTileProperty(68, 15, "Back", "Passable");
					location.removeMapTile(68, 15, "Buildings");
				}
				if (location.hasTileAt(64, 36, "Buildings", null))
				{
					location.removeTileProperty(64, 36, "Back", "Passable");
					location.removeMapTile(64, 36, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.15f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 == 0)
				{
					location.setTileProperty(48, 45, "Back", "Passable", "T");
					location.setMapTile(48, 45, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.4\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front3114"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings3114"));
					if (location.hasTileAt(31, 14, "Buildings", null))
					{
						location.removeTileProperty(31, 14, "Back", "Passable");
						location.removeMapTile(31, 14, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(31, 14, "Back", "Passable", "T");
					location.setMapTile(31, 14, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.4\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front4845"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings4845"));
					if (location.hasTileAt(48, 45, "Buildings", null))
					{
						location.removeTileProperty(48, 45, "Back", "Passable");
						location.removeMapTile(48, 45, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front3114"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings3114"));
				location.map.RemoveLayer(location.map.GetLayer("Front4845"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings4845"));
				if (location.hasTileAt(31, 14, "Buildings", null))
				{
					location.removeTileProperty(31, 14, "Back", "Passable");
					location.removeMapTile(31, 14, "Buildings");
				}
				if (location.hasTileAt(48, 45, "Buildings", null))
				{
					location.removeTileProperty(48, 45, "Back", "Passable");
					location.removeMapTile(48, 45, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.15f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 == 0)
				{
					location.setTileProperty(37, 25, "Back", "Passable", "T");
					location.setMapTile(37, 25, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.5\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front4625"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings4625"));
					if (location.hasTileAt(46, 25, "Buildings", null))
					{
						location.removeTileProperty(46, 25, "Back", "Passable");
						location.removeMapTile(46, 25, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(46, 25, "Back", "Passable", "T");
					location.setMapTile(46, 25, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.5\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front3725"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings3725"));
					if (location.hasTileAt(37, 25, "Buildings", null))
					{
						location.removeTileProperty(37, 25, "Back", "Passable");
						location.removeMapTile(37, 25, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front4625"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings4625"));
				location.map.RemoveLayer(location.map.GetLayer("Front3725"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings3725"));
				if (location.hasTileAt(46, 25, "Buildings", null))
				{
					location.removeTileProperty(46, 25, "Back", "Passable");
					location.removeMapTile(46, 25, "Buildings");
				}
				if (location.hasTileAt(37, 25, "Buildings", null))
				{
					location.removeTileProperty(37, 25, "Back", "Passable");
					location.removeMapTile(37, 25, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.2f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 == 0)
				{
					location.setTileProperty(57, 40, "Back", "Passable", "T");
					location.setMapTile(57, 40, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.6\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front3821"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings3821"));
					if (location.hasTileAt(38, 21, "Buildings", null))
					{
						location.removeTileProperty(38, 21, "Back", "Passable");
						location.removeMapTile(38, 21, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(38, 21, "Back", "Passable", "T");
					location.setMapTile(38, 21, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.6\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front5740"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings5740"));
					if (location.hasTileAt(57, 40, "Buildings", null))
					{
						location.removeTileProperty(57, 40, "Back", "Passable");
						location.removeMapTile(57, 40, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front3821"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings3821"));
				location.map.RemoveLayer(location.map.GetLayer("Front5740"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings5740"));
				if (location.hasTileAt(38, 21, "Buildings", null))
				{
					location.removeTileProperty(38, 21, "Back", "Passable");
					location.removeMapTile(38, 21, "Buildings");
				}
				if (location.hasTileAt(57, 40, "Buildings", null))
				{
					location.removeTileProperty(57, 40, "Back", "Passable");
					location.removeMapTile(57, 40, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.25f && Game1.random.NextDouble() > 0.20000000298023224)
			{
				location.setTileProperty(71, 20, "Back", "Passable", "T");
				location.setMapTile(71, 20, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.7\"", false);
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front7120"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings7120"));
				if (location.hasTileAt(71, 20, "Buildings", null))
				{
					location.removeTileProperty(71, 20, "Back", "Passable");
					location.removeMapTile(71, 20, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.3f && Game1.random.NextDouble() > 0.25)
			{
				if (Game1.dayOfMonth % 2 == 0)
				{
					location.setTileProperty(12, 39, "Back", "Passable", "T");
					location.setMapTile(12, 39, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.8\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front7335"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings7335"));
					if (location.hasTileAt(73, 35, "Buildings", null))
					{
						location.removeTileProperty(73, 35, "Back", "Passable");
						location.removeMapTile(73, 35, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(73, 35, "Back", "Passable", "T");
					location.setMapTile(73, 35, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.8\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front1141"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings1141"));
					if (location.hasTileAt(12, 39, "Buildings", null))
					{
						location.removeTileProperty(12, 39, "Back", "Passable");
						location.removeMapTile(12, 39, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front7335"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings7335"));
				location.map.RemoveLayer(location.map.GetLayer("Front1141"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings1141"));
				if (location.hasTileAt(73, 35, "Buildings", null))
				{
					location.removeTileProperty(73, 35, "Back", "Passable");
					location.removeMapTile(73, 35, "Buildings");
				}
				if (location.hasTileAt(12, 39, "Buildings", null))
				{
					location.removeTileProperty(12, 39, "Back", "Passable");
					location.removeMapTile(12, 39, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.35f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 == 0)
				{
					location.setTileProperty(23, 43, "Back", "Passable", "T");
					location.setMapTile(23, 43, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.9\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front6528"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings6528"));
					if (location.hasTileAt(65, 28, "Buildings", null))
					{
						location.removeTileProperty(65, 28, "Back", "Passable");
						location.removeMapTile(65, 28, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(65, 28, "Back", "Passable", "T");
					location.setMapTile(65, 28, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.9\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front2245"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings2245"));
					if (location.hasTileAt(23, 43, "Buildings", null))
					{
						location.removeTileProperty(23, 43, "Back", "Passable");
						location.removeMapTile(23, 43, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front6528"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings6528"));
				location.map.RemoveLayer(location.map.GetLayer("Front2245"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings2245"));
				if (location.hasTileAt(65, 28, "Buildings", null))
				{
					location.removeTileProperty(65, 28, "Back", "Passable");
					location.removeMapTile(65, 28, "Buildings");
				}
				if (location.hasTileAt(23, 43, "Buildings", null))
				{
					location.removeTileProperty(23, 43, "Back", "Passable");
					location.removeMapTile(23, 43, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.35f && Game1.random.NextDouble() > 0.25)
			{
				if (Game1.dayOfMonth % 2 != 0)
				{
					location.setTileProperty(67, 42, "Back", "Passable", "T");
					location.setMapTile(67, 42, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.12\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front5642"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings5624"));
					if (location.hasTileAt(56, 24, "Buildings", null))
					{
						location.removeTileProperty(56, 24, "Back", "Passable");
						location.removeMapTile(56, 24, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(56, 24, "Back", "Passable", "T");
					location.setMapTile(56, 24, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.12\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front6742"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings6742"));
					if (location.hasTileAt(67, 42, "Buildings", null))
					{
						location.removeTileProperty(67, 42, "Back", "Passable");
						location.removeMapTile(67, 42, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front5642"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings5624"));
				location.map.RemoveLayer(location.map.GetLayer("Front6742"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings6742"));
				if (location.hasTileAt(56, 24, "Buildings", null))
				{
					location.removeTileProperty(56, 24, "Back", "Passable");
					location.removeMapTile(56, 24, "Buildings");
				}
				if (location.hasTileAt(67, 42, "Buildings", null))
				{
					location.removeTileProperty(67, 42, "Back", "Passable");
					location.removeMapTile(67, 42, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.4f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 == 0)
				{
					location.setTileProperty(41, 20, "Back", "Passable", "T");
					location.setMapTile(41, 20, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.10\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front937"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings937"));
					if (location.hasTileAt(9, 37, "Buildings", null))
					{
						location.removeTileProperty(9, 37, "Back", "Passable");
						location.removeMapTile(9, 37, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(9, 37, "Back", "Passable", "T");
					location.setMapTile(9, 37, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.10\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front4120"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings4120"));
					if (location.hasTileAt(41, 20, "Buildings", null))
					{
						location.removeTileProperty(41, 20, "Back", "Passable");
						location.removeMapTile(41, 20, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front937"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings937"));
				location.map.RemoveLayer(location.map.GetLayer("Front4120"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings4120"));
				if (location.hasTileAt(9, 37, "Buildings", null))
				{
					location.removeTileProperty(9, 37, "Back", "Passable");
					location.removeMapTile(9, 37, "Buildings");
				}
				if (location.hasTileAt(41, 20, "Buildings", null))
				{
					location.removeTileProperty(41, 20, "Back", "Passable");
					location.removeMapTile(41, 20, "Buildings");
				}
			}
			if ((float)(donated / total) >= 0.45f && Game1.random.NextDouble() > 0.30000001192092896)
			{
				if (Game1.dayOfMonth % 2 != 0)
				{
					location.setTileProperty(20, 42, "Back", "Passable", "T");
					location.setMapTile(20, 42, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.11\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front6413"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings6413"));
					if (location.hasTileAt(64, 13, "Buildings", null))
					{
						location.removeTileProperty(64, 13, "Back", "Passable");
						location.removeMapTile(64, 13, "Buildings");
					}
				}
				else
				{
					location.setTileProperty(64, 13, "Back", "Passable", "T");
					location.setMapTile(64, 13, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.11\"", false);
					location.map.RemoveLayer(location.map.GetLayer("Front2042"));
					location.map.RemoveLayer(location.map.GetLayer("Buildings2042"));
					if (location.hasTileAt(20, 42, "Buildings", null))
					{
						location.removeTileProperty(20, 42, "Back", "Passable");
						location.removeMapTile(20, 42, "Buildings");
					}
				}
			}
			else
			{
				location.map.RemoveLayer(location.map.GetLayer("Front6413"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings6413"));
				location.map.RemoveLayer(location.map.GetLayer("Front2042"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings2042"));
				if (location.hasTileAt(20, 42, "Buildings", null))
				{
					location.removeTileProperty(20, 42, "Back", "Passable");
					location.removeMapTile(20, 42, "Buildings");
				}
				if (location.hasTileAt(64, 13, "Buildings", null))
				{
					location.removeTileProperty(64, 13, "Back", "Passable");
					location.removeMapTile(64, 13, "Buildings");
				}
			}
			if ((float)(donated / total) < 0.5f || Game1.random.NextDouble() <= 0.30000001192092896)
			{
				location.map.RemoveLayer(location.map.GetLayer("Front4340"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings4340"));
				location.map.RemoveLayer(location.map.GetLayer("Front1834"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings1834"));
				if (location.hasTileAt(18, 34, "Buildings", null))
				{
					location.removeTileProperty(18, 34, "Back", "Passable");
					location.removeMapTile(18, 34, "Buildings");
				}
				if (location.hasTileAt(43, 40, "Buildings", null))
				{
					location.removeTileProperty(43, 40, "Back", "Passable");
					location.removeMapTile(43, 40, "Buildings");
				}
				return;
			}
			if (Game1.dayOfMonth % 2 == 0)
			{
				location.setTileProperty(18, 34, "Back", "Passable", "T");
				location.setMapTile(18, 34, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.0\"", false);
				location.map.RemoveLayer(location.map.GetLayer("Front4340"));
				location.map.RemoveLayer(location.map.GetLayer("Buildings4340"));
				if (location.hasTileAt(43, 40, "Buildings", null))
				{
					location.removeTileProperty(43, 40, "Back", "Passable");
					location.removeMapTile(43, 40, "Buildings");
				}
				return;
			}
			location.setTileProperty(43, 40, "Back", "Passable", "T");
			location.setMapTile(43, 40, 1677, "Buildings", "spring_town", "Message \"Nature.NiTV.Vis.0\"", false);
			location.map.RemoveLayer(location.map.GetLayer("Front1834"));
			location.map.RemoveLayer(location.map.GetLayer("Buildings1834"));
			if (location.hasTileAt(18, 34, "Buildings", null))
			{
				location.removeTileProperty(18, 34, "Back", "Passable");
				location.removeMapTile(18, 34, "Buildings");
			}
		}

		// Token: 0x04000001 RID: 1
		public IModHelper helper;

		// Token: 0x04000002 RID: 2
		public Dictionary<string, List<string>> creatureData;

		// Token: 0x04000003 RID: 3
		private HashSet<string> desertLocationNames;

		// Token: 0x04000004 RID: 4
		private HashSet<string> forestLocationNames;

		// Token: 0x04000005 RID: 5
		private HashSet<string> waterLocationNames;

		// Token: 0x04000006 RID: 6
		private HashSet<string> underLocationNames;

		// Token: 0x04000007 RID: 7
		public static IModHelper staticHelper;

		// Token: 0x04000008 RID: 8
		private static Texture2D netTexture;

		// Token: 0x04000009 RID: 9
		public static Dictionary<string, List<string>> staticCreatureData;

		// Token: 0x0400000A RID: 10
		private int Tiles;

		// Token: 0x0400000B RID: 11
		private List<Vector2> treePos;

		// Token: 0x0400000C RID: 12
		private List<Vector2> waterPos;

		// Token: 0x0400000D RID: 13
		private List<Layer> layersToSave;

		// Token: 0x0400000E RID: 14
		public static List<NatCreature> creatures;

		// Token: 0x0400000F RID: 15
		public NatInValleyConfig config;

		// Token: 0x04000010 RID: 16
		public static PerScreen<int> AnimatedBump;

		// Token: 0x04000011 RID: 17
		private List<Vector2> bushPos;

		// Token: 0x04000012 RID: 18
		private List<Vector2> stumpPos;

		// Token: 0x04000013 RID: 19
		public static NatInValleyConfig staticConfig;

		// Token: 0x04000014 RID: 20
		private float dailyMod;

		// Token: 0x04000015 RID: 21
		private List<float> possibleDailyMods;

		// Token: 0x04000016 RID: 22
		private int locationCap;

		// Token: 0x04000017 RID: 23
		private double spawnChance;

		// Token: 0x04000018 RID: 24
		private int donatedCount;

		// Token: 0x04000019 RID: 25
		private Dictionary<string, List<string>> locationalData;

		// Token: 0x0400001A RID: 26
		public static Dictionary<string, RewardModel> customRewards;

		// Token: 0x0400001B RID: 27
		private static PerScreen<SparklingText> sparklingText = new PerScreen<SparklingText>();

		// Token: 0x0400001C RID: 28
		private int count;

		// Token: 0x0400001D RID: 29
		private int spawnedInLoc;

		// Token: 0x0400001E RID: 30
		private List<string> CreatureSpawnList = new List<string>();

		// Token: 0x0400001F RID: 31
		private Dictionary<string, CreatureModel> ModelData = new Dictionary<string, CreatureModel>();
	}
}
