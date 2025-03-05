using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace NatureInTheValley
{
	// Token: 0x02000014 RID: 20
	[XmlType("Mods_NatInValeyJadeNet")]
	[XmlRoot(ElementName = "natInValeyJadeNet", Namespace = "")]
	[KnownType(typeof(NatInValeyJadeNet))]
	[XmlInclude(typeof(NatInValeyJadeNet))]
	public class NatInValeyJadeNet : Tool
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0000287D File Offset: 0x00000A7D
		protected override Item GetOneNew()
		{
			return new NatInValeyJadeNet();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00002884 File Offset: 0x00000A84
		protected override string loadDisplayName()
		{
			return "=" + this.helper.Translation.Get("NetName");
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000028AA File Offset: 0x00000AAA
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00012B90 File Offset: 0x00010D90
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyJadeNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00012C00 File Offset: 0x00010E00
		public override void actionWhenStopBeingHeld(Farmer who)
		{
			base.actionWhenStopBeingHeld(who);
			who.canMove = true;
			NatureInTheValleyEntry.AnimatedBump.Value = 0;
			this.isHeld = false;
			if (who.UsingTool)
			{
				who.UsingTool = false;
				if (who.FarmerSprite.PauseForSingleAnimation)
				{
					who.FarmerSprite.PauseForSingleAnimation = false;
				}
			}
			if (who.hasBuff("NatCSpeedN"))
			{
				who.buffs.Remove("NatCSpeedN");
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00012C74 File Offset: 0x00010E74
		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			who.jitterStrength = 1f;
			who.canMove = true;
			this.save = 0;
			this.isHeld = true;
			who.forceCanMove();
			if (!who.hasBuff("NatCSpeedN"))
			{
				who.applyBuff(new Buff("NatCSpeedN", null, null, 20000, null, -1, null, null, null, null)
				{
					effects = 
					{
						Speed = 
						{
							Value = -0.3f
						}
					},
					millisecondsDuration = 20000
				});
			}
			return false;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000264E File Offset: 0x0000084E
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00012D00 File Offset: 0x00010F00
		public override bool onRelease(GameLocation location, int x, int y, Farmer who)
		{
			who.stopJittering();
			this.isHeld = false;
			who.canMove = false;
			who.jitterStrength = 0f;
			who.UsingTool = false;
			if (this.chargeSound != null)
			{
				this.chargeSound.Stop(AudioStopOptions.Immediate);
			}
			if (who.hasBuff("NatCSpeedN"))
			{
				ICue cue;
				Game1.playSound("NIVNetSwing", out cue);
				cue.Volume = cue.Volume * 2.5f / 3f;
			}
			who.canReleaseTool = false;
			switch (who.FacingDirection)
			{
			case 0:
				((FarmerSprite)who.Sprite).animateOnce(295, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(0, 0, who);
				break;
			case 1:
				((FarmerSprite)who.Sprite).animateOnce(296, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(1, 0, who);
				break;
			case 2:
				((FarmerSprite)who.Sprite).animateOnce(297, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(2, 0, who);
				break;
			case 3:
				((FarmerSprite)who.Sprite).animateOnce(298, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(3, 0, who);
				break;
			}
			return false;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00012E7C File Offset: 0x0001107C
		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (!this.isHeld || !who.hasBuff("NatCSpeedN"))
			{
				if (this.chargeSound != null && this.chargeSound.IsPlaying)
				{
					this.chargeSound.Stop(AudioStopOptions.Immediate);
				}
				who.stopJittering();
				return;
			}
			double num = (double)this.castingPower;
			double num2 = (double)this.castingTimerSpeed;
			double num3 = (double)time.ElapsedGameTime.Milliseconds;
			double num4 = num2 * num3;
			this.castingPower = Math.Max(0f, Math.Min(1f, (float)(num + num4)));
			if (this.PlayUseSounds && who.IsLocalPlayer && this.config.includeChargeSound)
			{
				if (this.chargeSound == null || !this.chargeSound.IsPlaying)
				{
					this.castingPower = 0.1f;
					Game1.playSound("SinWave", out this.chargeSound);
					this.chargeSound.Volume = this.chargeSound.Volume * 3f / 4f;
				}
				Game1.sounds.SetPitch(this.chargeSound, 2400f * this.castingPower, true);
			}
			if (this.castingPower == 1f || this.castingPower == 0f)
			{
				this.castingTimerSpeed = -this.castingTimerSpeed;
			}
			who.jitterStrength = 0.5f;
			switch (who.FacingDirection)
			{
			case 0:
				who.FarmerSprite.setCurrentSingleFrame(36, 32000, false, false);
				return;
			case 1:
				who.FarmerSprite.setCurrentSingleFrame(48, 100, false, false);
				return;
			case 2:
				who.FarmerSprite.setCurrentSingleFrame(66, 32000, false, false);
				return;
			case 3:
				who.FarmerSprite.setCurrentSingleFrame(48, 100, false, true);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000028C6 File Offset: 0x00000AC6
		private void EndOfUse(Farmer who)
		{
			who.canMove = true;
			this.isHeld = false;
			who.jitterStrength = 0f;
			NatureInTheValleyEntry.TryCatch(who);
			if (who.hasBuff("NatCSpeedN"))
			{
				who.buffs.Remove("NatCSpeedN");
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000268F File Offset: 0x0000088F
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000268F File Offset: 0x0000088F
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00013038 File Offset: 0x00011238
		public NatInValeyJadeNet()
		{
			base.ItemId = "NIVjadeNet";
			this.helper = NatureInTheValleyEntry.staticHelper;
			this.config = this.helper.ReadConfig<NatInValleyConfig>();
			NatInValeyJadeNet.texture = this.helper.ModContent.Load<Texture2D>("PNGs\\JadeNet");
			base.Category = -99;
			this.Name = "NatValleyJadeNet";
			this.Stack = 1;
		}

		// Token: 0x04000089 RID: 137
		public static Texture2D texture;

		// Token: 0x0400008A RID: 138
		private IModHelper helper;

		// Token: 0x0400008B RID: 139
		public bool isHeld;

		// Token: 0x0400008C RID: 140
		private int save;

		// Token: 0x0400008D RID: 141
		public int variant;

		// Token: 0x0400008E RID: 142
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x0400008F RID: 143
		private float castingPower;

		// Token: 0x04000090 RID: 144
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x04000091 RID: 145
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
