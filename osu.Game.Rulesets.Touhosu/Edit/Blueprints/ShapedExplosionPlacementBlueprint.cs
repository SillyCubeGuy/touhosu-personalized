﻿using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Explosions.Components;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints
{
    public class ShapedExplosionPlacementBlueprint : TouhosuPlacementBlueprint<ShapedExplosion>
    {
        private readonly ExplosionPiece piece;

        public ShapedExplosionPlacementBlueprint()
            : base(new ShapedExplosion())
        {
            InternalChild = piece = new ExplosionPiece();
        }

        protected override void Update()
        {
            base.Update();
            piece.UpdateFrom(HitObject);
        }
    }
}
