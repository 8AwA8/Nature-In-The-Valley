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
		// Token: 0x06000112 RID: 274 RVA: 0x00002932 File Offset: 0x00000B32
		protected override Item GetOneNew()
		{
			return new NatInValeyJadeNet();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00002939 File Offset: 0x00000B39
		protected override string loadDisplayName()
		{
			return "=" + this.helper.Translation.Get("NetName");
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000295F File Offset: 0x00000B5F
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00014B94 File Offset: 0x00012D94
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyJadeNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00014C04 File Offset: 0x00012E04
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

		// Token: 0x06000117 RID: 279 RVA: 0x00014C78 File Offset: 0x00012E78
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
							Value = -3.5f
						}
					},
					millisecondsDuration = 20000
				});
			}
			return false;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000026FF File Offset: 0x000008FF
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00014D04 File Offset: 0x00012F04
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

		// Token: 0x0600011A RID: 282 RVA: 0x00014E80 File Offset: 0x00013080
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

		// Token: 0x0600011B RID: 283 RVA: 0x0000297B File Offset: 0x00000B7B
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

		// Token: 0x0600011C RID: 284 RVA: 0x00002740 File Offset: 0x00000940
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00002740 File Offset: 0x00000940
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0001503C File Offset: 0x0001323C
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

		// Token: 0x04000090 RID: 144
		public static Texture2D texture;

		// Token: 0x04000091 RID: 145
		private IModHelper helper;

		// Token: 0x04000092 RID: 146
		public bool isHeld;

		// Token: 0x04000093 RID: 147
		private int save;

		// Token: 0x04000094 RID: 148
		public int variant;

		// Token: 0x04000095 RID: 149
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x04000096 RID: 150
		private float castingPower;

		// Token: 0x04000097 RID: 151
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x04000098 RID: 152
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
