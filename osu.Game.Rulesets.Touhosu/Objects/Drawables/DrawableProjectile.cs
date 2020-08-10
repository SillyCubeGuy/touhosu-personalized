﻿using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.UI;
using System;
using osu.Game.Rulesets.Touhosu.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableProjectile : DrawableTouhosuHitObject
    {
        protected virtual float BaseSize { get; } = 25;

        protected virtual bool AffectPlayer { get; } = true;

        protected virtual bool CheckWallCollision { get; } = true;

        protected virtual bool UseGlow { get; } = true;

        protected virtual string ProjectileName { get; } = "Sphere";

        protected readonly ProjectilePiece Piece;
        private double missTime;

        protected DrawableProjectile(Projectile h)
            : base(h)
        {
            Origin = Anchor.Centre;
            Size = new Vector2(BaseSize * MathExtensions.Map(h.CircleSize, 0, 10, 0.2f, 1));
            Position = h.Position;
            Scale = Vector2.Zero;
            AddInternal(Piece = new ProjectilePiece(ProjectileName, UseGlow));
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AccentColour.BindValueChanged(accent => Piece.AccentColour = accent.NewValue, true);
        }

        protected override void UpdateInitialTransforms()
        {
            this.ScaleTo(Vector2.One, HitObject.TimePreempt);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
            {
                if (AffectPlayer)
                {
                    if (CheckHit.Invoke(this))
                    {
                        missTime = timeOffset;
                        ApplyResult(r => r.Type = HitResult.Miss);
                        return;
                    }
                }

                if (CheckWallCollision)
                {
                    if (Position.X > TouhosuPlayfield.PLAYFIELD_SIZE.X + Size.X / 2f
                    || Position.X < -Size.X / 2f
                    || Position.Y > TouhosuPlayfield.PLAYFIELD_SIZE.Y + Size.Y / 2f
                    || Position.Y < -Size.Y / 2f)
                        ApplyResult(r => r.Type = HitResult.Perfect);
                }
            }
        }

        public Func<DrawableProjectile, bool> CheckHit;

        public Func<DrawableProjectile, float> CheckDistance;

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    // Check DrawableHitCircle L#168
                    this.Delay(missTime).FadeOut();
                    break;
            }
        }
    }
}
