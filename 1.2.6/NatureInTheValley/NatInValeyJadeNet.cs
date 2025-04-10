﻿using System;
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
		// Token: 0x060000ED RID: 237 RVA: 0x0000272C File Offset: 0x0000092C
		protected override Item GetOneNew()
		{
			return new NatInValeyJadeNet();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00002733 File Offset: 0x00000933
		protected override string loadDisplayName()
		{
			return "=" + this.helper.Translation.Get("NetName");
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00002759 File Offset: 0x00000959
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000D14C File Offset: 0x0000B34C
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyJadeNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000D1BC File Offset: 0x0000B3BC
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

		// Token: 0x060000F2 RID: 242 RVA: 0x0000D230 File Offset: 0x0000B430
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

		// Token: 0x060000F3 RID: 243 RVA: 0x00002515 File Offset: 0x00000715
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000D2BC File Offset: 0x0000B4BC
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

		// Token: 0x060000F5 RID: 245 RVA: 0x0000D448 File Offset: 0x0000B648
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

		// Token: 0x060000F6 RID: 246 RVA: 0x00002775 File Offset: 0x00000975
		private void EndOfUse(Farmer who)
		{
			who.canMove = true;
			this.isHeld = false;
			who.jitterStrength = 0f;
			NatureInTheValleyEntry.TryCatch(who);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00002539 File Offset: 0x00000739
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00002539 File Offset: 0x00000739
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000D604 File Offset: 0x0000B804
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

		// Token: 0x04000081 RID: 129
		public static Texture2D texture;

		// Token: 0x04000082 RID: 130
		private IModHelper helper;

		// Token: 0x04000083 RID: 131
		public bool isHeld;

		// Token: 0x04000084 RID: 132
		private int save;

		// Token: 0x04000085 RID: 133
		public int variant;

		// Token: 0x04000086 RID: 134
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x04000087 RID: 135
		private float castingPower;

		// Token: 0x04000088 RID: 136
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x04000089 RID: 137
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
