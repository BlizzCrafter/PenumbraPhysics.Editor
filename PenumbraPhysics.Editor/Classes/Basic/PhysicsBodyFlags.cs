using Microsoft.Xna.Framework;
using Penumbra;
using System.Collections.Generic;

namespace PenumbraPhysics.Editor.Classes.Basic
{
    public struct PhysicsBodyFlags
    {
        public List<Hull> HullList { get; set; }
        public float StartRotation { get; set; }
        public Vector2 StartPosition { get; set; }
    }
}
