using System;
using System.Collections.Generic;
using Creatures;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Shops;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Network;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Triggers;
using xTile.Dimensions;
using xTile.Layers;

namespace NatureInTheValley
{
	// Token: 0x02000004 RID: 4
	internal sealed class NatureInTheValleyEntry : Mod
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002B90 File Offset: 0x00000D90
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
				IMonitor monitor = base.Monitor;
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
			helper.Events.Multiplayer.ModMessageReceived += this.OnModMessagereceived;
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
			harmony.Patch(AccessTools.Method(typeof(GameLocation), "UpdateWhenCurrentLocation", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "Ticking", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(GameLocation), "answerDialogue", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "PostTextFunction", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(BusStop), "checkAction", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "CoverCheckAction", null), null, null, null);
			harmony.Patch(AccessTools.Method(typeof(Tree), "performTreeFall", null, null), new HarmonyMethod(typeof(NatureInTheValleyEntry), "TreeFell", null), null, null, null);
			if (helper.ModRegistry.IsLoaded("malic.cp.jadeNPC"))
			{
				harmony.Patch(AccessTools.Method(typeof(NPC), "checkAction", null, null), null, new HarmonyMethod(typeof(NatureInTheValleyEntry), "NPC_checkAction", null), null, null);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00003204 File Offset: 0x00001404
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

		// Token: 0x06000005 RID: 5 RVA: 0x00003478 File Offset: 0x00001678
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
					this.locbacklayer = eventArgs.NewLocation.map.GetLayer("Back");
				}
				else
				{
					this.locbacklayer = null;
				}
				if (eventArgs.NewLocation.map.GetLayer("Back") != null)
				{
					this.Tiles = eventArgs.NewLocation.map.GetLayer("Back").Tiles.Array.Length;
				}
				else
				{
					this.Tiles = 10;
				}
				this.locationCap = (int)((double)((float)(2 + this.Tiles / 1800)) * (double)Game1.numberOfPlayers() * (double)this.config.maxcreaturelLimitMultiplier * (double)this.dailyMod);
				this.spawnChance = 0.25 * (double)this.config.spawnRateMultiplier * this.func(this.Tiles) * 1.45 * (double)this.dailyMod;
				this.locationalData = this.CreaturesForArea(eventArgs.NewLocation);
				eventArgs.NewLocation.resourceClumps.OnValueRemoved -= new NetCollection<ResourceClump>.ContentsChangeEvent(this.RemovedResource);
				eventArgs.NewLocation.resourceClumps.OnValueRemoved += new NetCollection<ResourceClump>.ContentsChangeEvent(this.RemovedResource);
			}
			if (eventArgs.OldLocation != null)
			{
				eventArgs.OldLocation.resourceClumps.OnValueRemoved -= new NetCollection<ResourceClump>.ContentsChangeEvent(this.RemovedResource);
				this.ClearUnclaimedCreatures(eventArgs.OldLocation);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000369C File Offset: 0x0000189C
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
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002050 File Offset: 0x00000250
		static NatureInTheValleyEntry()
		{
			NatureInTheValleyEntry.customRewards = new Dictionary<string, RewardModel>();
			NatureInTheValleyEntry.AnimatedBump = new PerScreen<int>();
			NatureInTheValleyEntry.staticCreatureData = new Dictionary<string, List<string>>();
			NatureInTheValleyEntry.creatures = new List<NatCreature>();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00004788 File Offset: 0x00002988
		private static void Ticking(GameLocation __instance, GameTime time)
		{
			if (__instance.wasUpdated || !Game1.shouldTimePass(false))
			{
				return;
			}
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (Vector2.Distance(NatureInTheValleyEntry.creatures[i].Position, Game1.player.Position) < 1700f)
				{
					NatureInTheValleyEntry.creatures[i].Update(time);
					NatureInTheValleyEntry.creatures[i].UpdateAnim(time);
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00004804 File Offset: 0x00002A04
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
			if (Game1.currentLocation.map.GetLayer("Back") != null && (this.locbacklayer == null || this.locbacklayer != Game1.currentLocation.map.GetLayer("Back")))
			{
				this.locbacklayer = Game1.currentLocation.map.GetLayer("Back");
				this.Tiles = this.locbacklayer.Tiles.Array.Length;
				this.locationCap = (int)((double)((float)(2 + this.Tiles / 1800)) * (double)Game1.numberOfPlayers() * (double)this.config.maxcreaturelLimitMultiplier * (double)this.dailyMod);
				this.spawnChance = 0.25 * (double)this.config.spawnRateMultiplier * this.func(this.Tiles) * 1.45 * (double)this.dailyMod;
			}
			else
			{
				this.locbacklayer = null;
				this.Tiles = 10;
			}
			this.locationalData = this.CreaturesForArea(Game1.currentLocation);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000496C File Offset: 0x00002B6C
		public void Instantiate(string name, Vector2 tile, GameLocation location)
		{
			List<string> list = this.creatureData[name];
			NatureInTheValleyEntry.creatures.Add(new NatCreature(tile * 64f, location, name, int.Parse(list[0]), list[1] == "true", float.Parse(list[2]), int.Parse(list[3]), float.Parse(list[4]), list[5] == "true", list[6] == "true", int.Parse(list[7]), list[8] == "true", new List<string>(list[9].Split(',', StringSplitOptions.None)), int.Parse(list[10]), new List<string>(list[11].Split(',', StringSplitOptions.None)), int.Parse(list[12]), int.Parse(list[13]), int.Parse(list[14]), list[15], int.Parse(list[16]), int.Parse(list[17]), int.Parse(list[18]), false, (list.Count > 26) ? int.Parse(list[26]) : 32, (list.Count > 27) ? int.Parse(list[27]) : 32, list[list.Count - 1] == "true"));
			TriggerActionManager.Raise("NAT_NIV_Spawned", new object[]
			{
				name
			}, location, null, null, null);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002084 File Offset: 0x00000284
		public void SpawnCreatureInLocation(GameLocation location)
		{
			if (this.locbacklayer != null)
			{
				this.TrySpawnFromArea(location);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00004B18 File Offset: 0x00002D18
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
				if (flag5 && flag4 && new List<string>(value[9].Split(",", StringSplitOptions.None)).Contains(currentSeason) && timeOfDay <= int.Parse(value[13]) && timeOfDay >= int.Parse(value[12]) && ((!flag && !flag2 && !flag3) || value[10] != "1") && (flag || flag2 || flag3 || value[10] != "2") && (flag2 || value[10] != "3") && (flag3 || value[10] != "4"))
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return dictionary;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00004D94 File Offset: 0x00002F94
		public List<Vector2> GetTrees(GameLocation location)
		{
			List<Vector2> list = new List<Vector2>();
			try
			{
				using (NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>.ValuesCollection.Enumerator enumerator = location.terrainFeatures.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Tree && (enumerator.Current as Tree).growthStage.Value >= 5 && !(enumerator.Current as Tree).stump.Value)
						{
							list.Add(enumerator.Current.Tile);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00004E4C File Offset: 0x0000304C
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

		// Token: 0x0600000F RID: 15 RVA: 0x00004EC8 File Offset: 0x000030C8
		public void ClearUnclaimedCreatures(GameLocation location)
		{
			if (location.farmers.Count > 0)
			{
				return;
			}
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				if (NatureInTheValleyEntry.creatures[i].GetLocation().Name == location.Name)
				{
					NatureInTheValleyEntry.creatures.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00004F2C File Offset: 0x0000312C
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

		// Token: 0x06000011 RID: 17 RVA: 0x00004FB4 File Offset: 0x000031B4
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

		// Token: 0x06000012 RID: 18 RVA: 0x00005070 File Offset: 0x00003270
		private bool CheckForBugAtTile(GameLocation location, Vector2 pos, float distance)
		{
			foreach (NatCreature natCreature in NatureInTheValleyEntry.creatures)
			{
				if (Vector2.Distance(pos, natCreature.Position) <= distance)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000050D4 File Offset: 0x000032D4
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

		// Token: 0x06000014 RID: 20 RVA: 0x000051D8 File Offset: 0x000033D8
		private void setUpPositionalWarp(GameLocation a)
		{
			if (!base.Helper.ModRegistry.IsLoaded("hootless.BusLocations") || !base.Helper.ModRegistry.IsLoaded("Nature.NIVBL"))
			{
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

		// Token: 0x06000015 RID: 21 RVA: 0x000052FC File Offset: 0x000034FC
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

		// Token: 0x06000016 RID: 22 RVA: 0x0000535C File Offset: 0x0000355C
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
			if (__instance.lastQuestionKey == "NatDesk")
			{
				if (answer.responseKey == "NIVFResponse")
				{
					Game1.activeClickableMenu = new CreatureDonationMenu();
					return;
				}
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
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00005490 File Offset: 0x00003690
		public void Command(string common, string[] args)
		{
			this.Instantiate(args[0], Game1.player.Tile, Game1.player.currentLocation);
			Game1.player.addItemByMenuIfNecessary(new Terrarium("Tera.NatInValley.Creature." + args[0], new Vector2(30f, 30f)), null, false);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000054E8 File Offset: 0x000036E8
		public void OnLaunch(object sender, GameLaunchedEventArgs eventArgs)
		{
			this.helper.ModRegistry.GetApi<ISpaceCoreAPI>("spacechase0.SpaceCore").RegisterSerializerType(typeof(NatInValeyNet));
			this.helper.ModRegistry.GetApi<ISpaceCoreAPI>("spacechase0.SpaceCore").RegisterSerializerType(typeof(NatInValeyGoldenNet));
			this.helper.ModRegistry.GetApi<ISpaceCoreAPI>("spacechase0.SpaceCore").RegisterSerializerType(typeof(NatInValeyJadeNet));
			this.helper.ModRegistry.GetApi<ISpaceCoreAPI>("spacechase0.SpaceCore").RegisterSerializerType(typeof(Terrarium));
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
					keyValuePair.Value.compelxAnims.ToString().ToLower()
				};
				if (this.creatureData.ContainsKey(keyValuePair.Key))
				{
					this.creatureData.Remove(keyValuePair.Key);
				}
				this.creatureData.Add(keyValuePair.Key, value2);
			}
			NatureInTheValleyEntry.staticCreatureData = this.creatureData;
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
			IGenericModConfigMenuApi api = base.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
			IToolbarIconsApi api2 = base.Helper.ModRegistry.GetApi<IToolbarIconsApi>("furyx639.ToolbarIcons");
			if (api2 != null)
			{
				api2.AddToolbarIcon("Nature.NIV.Icon", "Mods/NatureInTheValley/Icon", new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16)), this.helper.Translation.Get("DecoratorTT"));
				api2.ToolbarIconPressed += delegate(object o, string s)
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
			IInformant api3 = base.Helper.ModRegistry.GetApi<IInformant>("Slothsoft.Informant");
			if (api3 != null)
			{
				api3.AddItemDecorator("NatureInTheValleyDonate", new Func<string>(this.DecoratorName), new Func<string>(this.DecoratorDesc), new Func<Item, Texture2D>(this.Decorate));
			}
			GameStateQuery.Register("NAT_NIV_DonatedTotalPercent", new GameStateQueryDelegate(NatureInTheValleyEntry.DonatedTotalGSQ));
			GameStateQuery.Register("NAT_NIV_DonatedSpecific", new GameStateQueryDelegate(NatureInTheValleyEntry.DonatedSpecificGSQ));
			GameStateQuery.Register("NAT_NIV_CaughtSpecific", new GameStateQueryDelegate(NatureInTheValleyEntry.CaughtSpecificGSQ));
			GameStateQuery.Register("NAT_NIV_SpawnedSpecific", new GameStateQueryDelegate(NatureInTheValleyEntry.SpawnedSpecificGSQ));
			TriggerActionManager.RegisterAction("NAT_NIV_ClearCreatures", new TriggerActionDelegate(NatureInTheValleyEntry.ActionClearCreatures));
			TriggerActionManager.RegisterAction("NAT_NIV_InstantiateSpecificCreature", new TriggerActionDelegate(NatureInTheValleyEntry.ActionSpawnSpecific));
			TriggerActionManager.RegisterTrigger("NAT_NIV_Spawned");
			if (api == null)
			{
				return;
			}
			api.Register(base.ModManifest, delegate
			{
				this.config = new NatInValleyConfig();
			}, delegate
			{
				base.Helper.WriteConfig<NatInValleyConfig>(this.config);
			}, false);
			api.AddNumberOption(base.ModManifest, () => this.config.spawnRateMultiplier, delegate(float value)
			{
				this.config.spawnRateMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingMult"), () => this.helper.Translation.Get("MenuingMultInfo"), new float?(0.1f), new float?(10f), new float?(0.1f), null, null);
			api.AddNumberOption(base.ModManifest, () => this.config.maxcreaturelLimitMultiplier, delegate(float value)
			{
				this.config.maxcreaturelLimitMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingLim"), () => this.helper.Translation.Get("MenuingLimInfo"), new float?(0.1f), new float?(10f), new float?(0.1f), null, null);
			api.AddNumberOption(base.ModManifest, () => this.config.creaturePriceMultiplier, delegate(float value)
			{
				this.config.creaturePriceMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingPrice"), () => this.helper.Translation.Get("MenuingPriceInfo"), new float?(0.1f), new float?(10f), new float?(0.1f), null, null);
			api.AddBoolOption(base.ModManifest, () => this.config.addCreaturesToShippingCollection, delegate(bool value)
			{
				this.config.addCreaturesToShippingCollection = value;
			}, () => this.helper.Translation.Get("MenuingShip"), () => this.helper.Translation.Get("MenuingShipInfo"), null);
			api.AddBoolOption(base.ModManifest, () => this.config.useOnlyContentPacks, delegate(bool value)
			{
				this.config.useOnlyContentPacks = value;
				NatureInTheValleyEntry.staticConfig.useOnlyContentPacks = value;
			}, () => this.helper.Translation.Get("MenuingPack"), () => "", null);
			api.AddBoolOption(base.ModManifest, () => this.config.useTerrariumWallpapers, delegate(bool value)
			{
				this.config.useTerrariumWallpapers = value;
				NatureInTheValleyEntry.staticConfig.useTerrariumWallpapers = value;
			}, () => this.helper.Translation.Get("MenuingWall"), () => "", null);
			api.AddBoolOption(base.ModManifest, () => this.config.includeChargeSound, delegate(bool value)
			{
				this.config.includeChargeSound = value;
			}, () => this.helper.Translation.Get("MenuingSound"), () => "", null);
			api.AddNumberOption(base.ModManifest, () => this.config.catchingDifficultyMultiplier, delegate(float value)
			{
				this.config.catchingDifficultyMultiplier = value;
				NatureInTheValleyEntry.staticConfig.catchingDifficultyMultiplier = value;
			}, () => this.helper.Translation.Get("MenuingNet"), () => this.helper.Translation.Get("MenuingNetInfo"), new float?(0.7f), new float?(3f), new float?(0.1f), null, null);
			api.AddKeybindList(base.ModManifest, () => this.config.KeyForEncy, delegate(KeybindList value)
			{
				this.config.KeyForEncy = value;
			}, () => this.helper.Translation.Get("MenuingKey"), () => this.helper.Translation.Get("MenuingKeyInfo"), null);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00005E78 File Offset: 0x00004078
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
				if (NatureInTheValleyEntry.creatures[i].isStatic || farmer.currentLocation.Name == "NIVInnerInsec")
				{
					return;
				}
				if (NatureInTheValleyEntry.creatures[i].GetLocation().Name == farmer.currentLocation.Name && (double)Vector2.Distance(NatureInTheValleyEntry.creatures[i].GetEffectivePosition() + (NatureInTheValleyEntry.creatures[i].IsGrounded ? Vector2.Zero : new Vector2(0f, -30f)), value) < (double)((NatureInTheValleyEntry.creatures[i].IsGrounded ? 60f : 85f) * NatureInTheValleyEntry.staticConfig.catchingDifficultyMultiplier) * Math.Sqrt((double)NatureInTheValleyEntry.creatures[i].scale))
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
					Game1.player.addItemByMenuIfNecessary(ItemRegistry.Create("NatInValley.Creature." + NatureInTheValleyEntry.creatures[i].name, 1, 0, false), null, false);
					NatureInTheValleyEntry.creatures.RemoveAt(i);
					i--;
				}
				else if (NatureInTheValleyEntry.creatures[i].GetLocation() == farmer.currentLocation && Vector2.Distance(NatureInTheValleyEntry.creatures[i].GetEffectivePosition() + new Vector2(16f, 16f) + (NatureInTheValleyEntry.creatures[i].IsGrounded ? Vector2.Zero : new Vector2(0f, -32f)), value) < 180f && !NatureInTheValleyEntry.creatures[i].isRunning && !NatureInTheValleyEntry.creatures[i].Dangerous)
				{
					NatureInTheValleyEntry.creatures[i].isRunning = true;
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00006248 File Offset: 0x00004448
		private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
		{
			if (e.NameWithoutLocale.IsEquivalentTo("Mods/NatureInTheValley/Icon", false))
			{
				e.LoadFromModFile<Texture2D>("PNGs/Icon.png", AssetLoadPriority.Low);
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Mods/NatureInTheValley/TerrariumItem", false))
			{
				e.LoadFromModFile<Texture2D>("PNGs/TerrariumItem.png", AssetLoadPriority.Low);
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Nature.NITV/Creatures", false))
			{
				e.LoadFrom(() => new Dictionary<string, CreatureModel>(), AssetLoadPriority.Exclusive, null);
			}
			if (e.NameWithoutLocale.IsEquivalentTo("Nature.NITV/Rewards", false))
			{
				e.LoadFrom(() => new Dictionary<string, RewardModel>(), AssetLoadPriority.Exclusive, null);
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
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00006368 File Offset: 0x00004568
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
			forSale.Insert(13, natInValeyNet);
			itemPriceAndStock.Add(natInValeyNet, new ItemStockInformation(250, int.MaxValue, null, null, LimitedStockMode.Global, null, null, null, null));
			(newMenu as ShopMenu).itemPriceAndStock = itemPriceAndStock;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002095 File Offset: 0x00000295
		private bool ReturnHome(GameLocation location, string[] args, Farmer player, Point point)
		{
			Game1.warpFarmer("BusStop", 22, 11, 0);
			return true;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000642C File Offset: 0x0000462C
		public bool OpenDonationWindow(GameLocation location, string[] args, Farmer player, Point point)
		{
			location.createQuestionDialogue(this.helper.Translation.Get("InsectariumDesk"), new Response[]
			{
				new Response("NIVFResponse", this.helper.Translation.Get("InsectariumC1")),
				new Response("NIVSResponse", this.helper.Translation.Get("InsectariumC2")),
				new Response("NIVRResponse", this.helper.Translation.Get("InsectariumC4")),
				new Response("NIVTResponse", this.helper.Translation.Get("InsectariumC5")),
				new Response("NIVNResponse", this.helper.Translation.Get("InsectariumC3"))
			}, "NatDesk");
			return true;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00006528 File Offset: 0x00004728
		public void setUpInsectarium(GameLocation location)
		{
			int num = 0;
			if (this.CreaturesInLocationR(location) < 1)
			{
				using (Dictionary<string, List<string>>.Enumerator enumerator = this.creatureData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, List<string>> keyValuePair = enumerator.Current;
						string text;
						if (location.modData.TryGetValue("NatureInTheValley/Donated/" + keyValuePair.Key, out text))
						{
							num++;
							List<string> value = keyValuePair.Value;
							List<string> list = new List<string>();
							foreach (string item in value[11].Split(",", StringSplitOptions.None))
							{
								list.Add(item);
							}
							NatureInTheValleyEntry.creatures.Add(new NatCreature(new Vector2(float.Parse(value[21]), float.Parse(value[22])) * 64f, location, keyValuePair.Key, int.Parse(value[0]), value[1] == "true", float.Parse(value[2]), int.Parse(value[3]), float.Parse(value[4]), value[5] == "true", value[6] == "true", int.Parse(value[7]), value[8] == "true", new List<string>(value[9].Split(',', StringSplitOptions.None)), int.Parse(value[10]), list, int.Parse(value[12]), int.Parse(value[13]), int.Parse(value[14]), value[15], int.Parse(value[16]), int.Parse(value[17]), int.Parse(value[18]), true, (value.Count > 26) ? int.Parse(value[26]) : 32, (value.Count > 27) ? int.Parse(value[27]) : 32, value[value.Count - 1] == "true"));
						}
					}
					goto IL_25F;
				}
			}
			num = this.CreaturesInLocationR(location);
			IL_25F:
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
					location.map.GetLayer("Front36")
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
							location.map.InsertLayer(layer, 3);
						}
						catch (Exception)
						{
						}
					}
				}
			}
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
			if (num == 84 && !Game1.player.hasOrWillReceiveMail("InsectariumFinalEvent"))
			{
				Game1.player.mailReceived.Add("InsectariumFinalEvent");
				Event evt2 = new Event(string.Concat(new string[]
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
				location.startEvent(evt2);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00007570 File Offset: 0x00005770
		public void CommandTwo(string common, string[] args)
		{
			GameLocation locationFromName = Game1.getLocationFromName("NIVInnerInsec");
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				locationFromName.modData["NatureInTheValley/Donated/" + keyValuePair.Key] = "true";
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000075E8 File Offset: 0x000057E8
		public void CommandThree(string common, string[] args)
		{
			GameLocation locationFromName = Game1.getLocationFromName("NIVInnerInsec");
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				locationFromName.modData.Remove("NatureInTheValley/Donated/" + keyValuePair.Key);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000020A7 File Offset: 0x000002A7
		public static bool HighlightCollectableRewards(Item item)
		{
			return Game1.player.couldInventoryAcceptThisItem(item);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000765C File Offset: 0x0000585C
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
				num2++;
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

		// Token: 0x06000023 RID: 35 RVA: 0x000020B4 File Offset: 0x000002B4
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

		// Token: 0x06000024 RID: 36 RVA: 0x00007A94 File Offset: 0x00005C94
		public void DayStarted(object sender, DayStartedEventArgs eventArgs)
		{
			NatureInTheValleyEntry.creatures.Clear();
			this.dailyMod = this.possibleDailyMods[Game1.random.Next(this.possibleDailyMods.Count - 1)];
			int num = 0;
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.creatureData)
			{
				string text;
				if (Game1.getLocationFromName("NIVInnerInsec").modData.TryGetValue("NatureInTheValley/Donated/" + keyValuePair.Key, out text))
				{
					num++;
				}
			}
			this.donatedCount = num;
			if (this.helper.ModRegistry.IsLoaded("malic.cp.jadeNPC"))
			{
				if (Game1.seasonIndex == 1 && Game1.player.getFriendshipLevelForNPC("Jade") >= 2 && !Game1.player.hasQuest("NIVjadeQuest1") && !Game1.player.hasQuest("NIVjadeQuest4"))
				{
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest1"));
				}
				if (Game1.seasonIndex == 2 && Game1.player.getFriendshipLevelForNPC("Jade") >= 4 && !Game1.player.hasQuest("NIVjadeQuest2") && !Game1.player.hasQuest("NIVjadeQuest1"))
				{
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest2"));
				}
				if (Game1.seasonIndex == 3 && Game1.player.getFriendshipLevelForNPC("Jade") >= 6 && !Game1.player.hasQuest("NIVjadeQuest3") && !Game1.player.hasQuest("NIVjadeQuest2"))
				{
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest3"));
				}
				if (Game1.seasonIndex == 4 && Game1.player.getFriendshipLevelForNPC("Jade") >= 8 && !Game1.player.hasQuest("NIVjadeQuest4") && !Game1.player.hasQuest("NIVjadeQuest3"))
				{
					Game1.player.questLog.Add(Quest.getQuestFromId("NIVjadeQuest4"));
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000020EF File Offset: 0x000002EF
		private double func(int x)
		{
			return Math.Max((double)x / 4000.0, 1.0);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00007CB8 File Offset: 0x00005EB8
		private void OnModMessagereceived(object sender, ModMessageReceivedEventArgs eventArgs)
		{
			if (eventArgs.FromModID == "Nature.NatureInTheValley" && !Context.IsSplitScreen)
			{
				string type = eventArgs.Type;
				if (type == "Clear")
				{
					NatureInTheValleyEntry.creatures.Clear();
					return;
				}
				if (type == "AddSpecific")
				{
					Tuple<string, int, int, string> tuple = eventArgs.ReadAs<Tuple<string, int, int, string>>();
					List<string> list = this.creatureData[tuple.Item1];
					NatureInTheValleyEntry.creatures.Add(new NatCreature(new Vector2((float)tuple.Item2, (float)tuple.Item3) * 64f, Game1.getLocationFromName(tuple.Item4), tuple.Item1, int.Parse(list[0]), list[1] == "true", float.Parse(list[2]), int.Parse(list[3]), float.Parse(list[4]), list[5] == "true", list[6] == "true", int.Parse(list[7]), list[8] == "true", new List<string>(list[9].Split(',', StringSplitOptions.None)), int.Parse(list[10]), new List<string>(list[11].Split(',', StringSplitOptions.None)), int.Parse(list[12]), int.Parse(list[13]), int.Parse(list[14]), list[15], int.Parse(list[16]), int.Parse(list[17]), int.Parse(list[18]), tuple.Item4 == "NIVInnerInsec", (list.Count > 26) ? int.Parse(list[26]) : 32, (list.Count > 27) ? int.Parse(list[27]) : 32, list[list.Count - 1] == "true"));
					TriggerActionManager.Raise("NAT_NIV_Spawned", new object[]
					{
						tuple.Item1
					}, Game1.getLocationFromName(tuple.Item4), null, null, null);
					return;
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00007EFC File Offset: 0x000060FC
		public void CommandFour(string common, string[] args)
		{
			foreach (NatCreature natCreature in NatureInTheValleyEntry.creatures)
			{
				base.Monitor.Log(natCreature.name, LogLevel.Warn);
				base.Monitor.Log(natCreature.Position.ToString(), LogLevel.Warn);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00007F78 File Offset: 0x00006178
		public void OneSecond(object sender, OneSecondUpdateTickedEventArgs e)
		{
			if (Game1.currentLocation == null || Game1.currentLocation.Name == "NIVInnerInsec" || !Game1.shouldTimePass(false))
			{
				return;
			}
			if (NatureInTheValleyEntry.creatures.Count < this.locationCap && Game1.random.NextDouble() < this.spawnChance)
			{
				this.SpawnCreatureInLocation(Game1.currentLocation);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00007FDC File Offset: 0x000061DC
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

		// Token: 0x0600002A RID: 42 RVA: 0x0000210B File Offset: 0x0000030B
		public int CreaturesInLocation()
		{
			return NatureInTheValleyEntry.creatures.Count;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000080EC File Offset: 0x000062EC
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

		// Token: 0x0600002C RID: 44 RVA: 0x00008154 File Offset: 0x00006354
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

		// Token: 0x0600002D RID: 45 RVA: 0x00002117 File Offset: 0x00000317
		private void Pressed(object sender, ButtonPressedEventArgs eventArgs)
		{
			if (this.config.KeyForEncy.IsDown() && Context.IsPlayerFree)
			{
				Game1.activeClickableMenu = new ClickIntoCreatureInfoMenu();
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000081BC File Offset: 0x000063BC
		public static void TreeFell(Tree __instance, Tool t, int explosion, Vector2 tileLocation)
		{
			for (int i = 0; i < NatureInTheValleyEntry.creatures.Count; i++)
			{
				NatCreature natCreature = NatureInTheValleyEntry.creatures[i];
				if (Vector2.Distance(natCreature.Position / 64f, __instance.Tile) < 2f && natCreature.GetLocation().Name == __instance.Location.Name)
				{
					natCreature.isRunning = true;
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00008230 File Offset: 0x00006430
		public static void NPC_checkAction(NPC __instance)
		{
			if (__instance.Name == "Jade" && Game1.player.getFriendshipLevelForNPC("Jade") >= 2250 && !Game1.player.hasOrWillReceiveMail("NIVJadeNet") && Game1.player.couldInventoryAcceptThisItem(new NatInValeyJadeNet()))
			{
				Game1.player.mailReceived.Add("NIVJadeNet");
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("Jade"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("JadeMessageNet")));
				Game1.player.addItemToInventory(new NatInValeyJadeNet());
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000082E8 File Offset: 0x000064E8
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
			if (code == "2")
			{
				return this.waterLocationNames.Contains(location.Name) || ((this.waterPos != null) ? ((float)this.waterPos.Count) : 0f) / (float)Math.Max(this.Tiles, 100) >= 0.4f;
			}
			return location.Name == code;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00008520 File Offset: 0x00006720
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

		// Token: 0x06000032 RID: 50 RVA: 0x000085A0 File Offset: 0x000067A0
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
				num2++;
			}
			return float.Parse(query[1]) <= (float)(num / num2);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00008634 File Offset: 0x00006834
		public static bool DonatedSpecificGSQ(string[] query, GameStateQueryContext context)
		{
			string text;
			return Game1.getLocationFromName("NIVInnerInsec").modData.TryGetValue("NatureInTheValley/Donated/" + query[1], out text);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00008664 File Offset: 0x00006864
		public static bool CaughtSpecificGSQ(string[] query, GameStateQueryContext context)
		{
			string text;
			return Game1.player.modData.TryGetValue("NIVCaught" + query[1], out text);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00008690 File Offset: 0x00006890
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

		// Token: 0x06000036 RID: 54 RVA: 0x000086F0 File Offset: 0x000068F0
		public static bool ActionClearCreatures(string[] args, TriggerActionContext context, out string error)
		{
			string text;
			if (!ArgUtility.TryGet(args, 0, out text, out error, false, null))
			{
				return false;
			}
			NatureInTheValleyEntry.creatures.Clear();
			NatureInTheValleyEntry.staticHelper.Multiplayer.SendMessage<int>(1, "Clear", new string[]
			{
				"Nature.NatureInTheValley"
			}, null);
			return true;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000873C File Offset: 0x0000693C
		public static bool ActionSpawnSpecific(string[] args, TriggerActionContext context, out string error)
		{
			string text;
			if (!ArgUtility.TryGet(args, 0, out text, out error, false, null))
			{
				return false;
			}
			List<string> list = NatureInTheValleyEntry.staticCreatureData[args[1]];
			List<string> list2 = new List<string>();
			foreach (string item in list[11].Split(",", StringSplitOptions.None))
			{
				list2.Add(item);
			}
			NatureInTheValleyEntry.creatures.Add(new NatCreature(new Vector2((float)int.Parse(args[2]) * 64f, (float)int.Parse(args[3]) * 64f), Game1.currentLocation, args[1], int.Parse(list[0]), list[1] == "true", float.Parse(list[2]), int.Parse(list[3]), float.Parse(list[4]), list[5] == "true", list[6] == "true", int.Parse(list[7]), list[8] == "true", new List<string>(list[9].Split(',', StringSplitOptions.None)), int.Parse(list[10]), list2, int.Parse(list[12]), int.Parse(list[13]), int.Parse(list[14]), list[15], int.Parse(list[16]), int.Parse(list[17]), int.Parse(list[18]), false, (list.Count > 26) ? int.Parse(list[26]) : 32, (list.Count > 27) ? int.Parse(list[27]) : 32, list[list.Count - 1] == "true"));
			NatureInTheValleyEntry.staticHelper.Multiplayer.SendMessage<Tuple<string, int, int, string>>(new Tuple<string, int, int, string>(args[1], int.Parse(args[2]), int.Parse(args[3]), Game1.currentLocation.Name), "AddSpecific", new string[]
			{
				"Nature.NatureInTheValley"
			}, null);
			TriggerActionManager.Raise("NAT_NIV_Spawned", new object[]
			{
				args[1]
			}, null, null, null, null);
			return true;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00008984 File Offset: 0x00006B84
		public void CommandFive(string common, string[] args)
		{
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.locationalData)
			{
				base.Monitor.Log(keyValuePair.Key, LogLevel.Warn);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000089E4 File Offset: 0x00006BE4
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

		// Token: 0x0600003A RID: 58 RVA: 0x00008ACC File Offset: 0x00006CCC
		private Vector2 ValidPosition(GameLocation l)
		{
			for (int i = 0; i < 10; i++)
			{
				Vector2 vector = new Vector2((float)Game1.random.Next(this.locbacklayer.TileWidth), (float)Game1.random.Next(this.locbacklayer.TileHeight));
				if (l.CanSpawnCharacterHere(vector) && Vector2.Distance(Game1.player.Tile, vector) > 5f)
				{
					return vector;
				}
			}
			return Vector2.Zero;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00008B40 File Offset: 0x00006D40
		public void TrySpawnFromArea(GameLocation location)
		{
			string text = this.HandleRarityChance(this.locationalData);
			if (text == "")
			{
				return;
			}
			string a = this.locationalData[text][17];
			if (a == "0")
			{
				Vector2 value = this.ValidPosition(location);
				if (value != Vector2.Zero)
				{
					this.Instantiate(text, value + new Vector2(-0.25f, 0.25f), location);
					if (this.locationalData[text].Count > 29 && this.locationalData[text][29] != "true" && this.locationalData[text][29] != "false")
					{
						for (int i = 0; i < int.Parse(this.locationalData[text][29]) - 1; i++)
						{
							Vector2 vector = value + new Vector2(-0.25f, 0.25f) + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
							if (this.isValidPosition(location, vector))
							{
								this.Instantiate(text, vector, location);
							}
						}
					}
				}
				return;
			}
			if (a == "1")
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
				if (this.locationalData[text].Count > 29 && this.locationalData[text][29] != "true" && this.locationalData[text][29] != "false")
				{
					for (int j = 0; j < int.Parse(this.locationalData[text][29]) - 1; j++)
					{
						Vector2 vector3 = vector2 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
						if (this.isValidPosition(location, vector3))
						{
							this.Instantiate(text, vector3, location);
						}
					}
				}
				return;
			}
			else if (a == "2")
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
				if (this.locationalData[text].Count > 29 && this.locationalData[text][29] != "true" && this.locationalData[text][29] != "false")
				{
					for (int k = 0; k < int.Parse(this.locationalData[text][29]) - 1; k++)
					{
						Vector2 vector5 = vector4 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
						if (this.isValidPosition(location, vector5))
						{
							this.Instantiate(text, vector5, location);
						}
					}
				}
				return;
			}
			else
			{
				if (a != "3")
				{
					if (a == "4")
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
						if (this.locationalData[text].Count > 29 && this.locationalData[text][29] != "true" && this.locationalData[text][29] != "false")
						{
							for (int l = 0; l < int.Parse(this.locationalData[text][29]) - 1; l++)
							{
								Vector2 vector7 = vector6 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
								if (this.isValidPosition(location, vector7))
								{
									this.Instantiate(text, vector7, location);
								}
							}
						}
					}
					return;
				}
				List<Vector2> list4 = (this.waterPos != null) ? this.waterPos : this.GetWater(location);
				if (list4.Count < 2)
				{
					return;
				}
				Vector2 vector8 = list4[Game1.random.Next(list4.Count - 1)];
				this.Instantiate(text, vector8, location);
				if (this.locationalData[text].Count > 29 && this.locationalData[text][29] != "true" && this.locationalData[text][29] != "false")
				{
					for (int m = 0; m < int.Parse(this.locationalData[text][29]) - 1; m++)
					{
						Vector2 vector9 = vector8 + new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
						if (this.isValidPosition(location, vector9))
						{
							this.Instantiate(text, vector9, location);
						}
					}
				}
				return;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000213C File Offset: 0x0000033C
		private bool isValidPosition(GameLocation l, Vector2 vector)
		{
			return l.CanSpawnCharacterHere(vector);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002145 File Offset: 0x00000345
		public string DecoratorName()
		{
			return this.helper.Translation.Get("DecoratorTT");
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002161 File Offset: 0x00000361
		public string DecoratorDesc()
		{
			return this.helper.Translation.Get("DecoratorDesc");
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000916C File Offset: 0x0000736C
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

		// Token: 0x06000063 RID: 99 RVA: 0x000091D8 File Offset: 0x000073D8
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
								keyValuePair.Value.compelxAnims.ToString().ToLower()
							};
							if (this.creatureData.ContainsKey(keyValuePair.Key))
							{
								this.creatureData.Remove(keyValuePair.Key);
							}
							this.creatureData.Add(keyValuePair.Key, value);
						}
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
		private Layer locbacklayer;

		// Token: 0x0400001C RID: 28
		private static PerScreen<SparklingText> sparklingText = new PerScreen<SparklingText>();

		// Token: 0x0400001D RID: 29
		private int count;
	}
}
