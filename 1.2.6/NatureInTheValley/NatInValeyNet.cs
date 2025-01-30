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
	// Token: 0x02000007 RID: 7
	[XmlType("Mods_NatInValeyNet")]
	[XmlRoot(ElementName = "natInValeyNet", Namespace = "")]
	[KnownType(typeof(NatInValeyNet))]
	[XmlInclude(typeof(NatInValeyNet))]
	public class NatInValeyNet : Tool
	{
		// Token: 0x0600007D RID: 125 RVA: 0x000024D6 File Offset: 0x000006D6
		protected override Item GetOneNew()
		{
			return new NatInValeyNet();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000024DD File Offset: 0x000006DD
		protected override string loadDisplayName()
		{
			return this.helper.Translation.Get("NetName");
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000024F9 File Offset: 0x000006F9
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000A9F0 File Offset: 0x00008BF0
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000AA60 File Offset: 0x00008C60
		public override void actionWhenStopBeingHeld(Farmer who)
		{
			base.actionWhenStopBeingHeld(who);
			NatureInTheValleyEntry.AnimatedBump.Value = 0;
			who.canMove = true;
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

		// Token: 0x06000082 RID: 130 RVA: 0x0000AAD4 File Offset: 0x00008CD4
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
							Value = -((float)who.Speed + who.buffs.Speed - 1.5f)
						}
					},
					millisecondsDuration = 20000
				});
			}
			return false;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002515 File Offset: 0x00000715
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000AB78 File Offset: 0x00008D78
		public override bool onRelease(GameLocation location, int x, int y, Farmer who)
		{
			who.stopJittering();
			this.isHeld = false;
			who.canMove = false;
			who.jitterStrength = 0f;
			who.UsingTool = false;
			if (who.hasBuff("NatCSpeedN"))
			{
				ICue cue;
				Game1.playSound("NIVNetSwing", out cue);
				cue.Volume = cue.Volume * 2.5f / 3f;
				who.buffs.Remove("NatCSpeedN");
			}
			if (this.chargeSound != null)
			{
				this.chargeSound.Stop(AudioStopOptions.Immediate);
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

		// Token: 0x06000085 RID: 133 RVA: 0x0000AD04 File Offset: 0x00008F04
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

		// Token: 0x06000086 RID: 134 RVA: 0x00002518 File Offset: 0x00000718
		private void EndOfUse(Farmer who)
		{
			who.canMove = true;
			this.isHeld = false;
			who.jitterStrength = 0f;
			NatureInTheValleyEntry.TryCatch(who);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002539 File Offset: 0x00000739
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002539 File Offset: 0x00000739
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000AEC0 File Offset: 0x000090C0
		public NatInValeyNet()
		{
			base.ItemId = "NIVNet";
			this.helper = NatureInTheValleyEntry.staticHelper;
			this.config = this.helper.ReadConfig<NatInValleyConfig>();
			NatInValeyNet.texture = this.helper.ModContent.Load<Texture2D>("PNGs\\NormalNet");
			base.Category = -99;
			this.Name = "NatValleyNet";
			this.Stack = 1;
		}

		// Token: 0x04000048 RID: 72
		public static Texture2D texture;

		// Token: 0x04000049 RID: 73
		private IModHelper helper;

		// Token: 0x0400004A RID: 74
		public bool isHeld;

		// Token: 0x0400004B RID: 75
		private int save;

		// Token: 0x0400004C RID: 76
		public int variant;

		// Token: 0x0400004D RID: 77
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x0400004E RID: 78
		private float castingPower;

		// Token: 0x0400004F RID: 79
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x04000050 RID: 80
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
