using Microsoft.Xna.Framework;
using Penumbra;
using System.Collections.Generic;
using System.ComponentModel;

namespace PenumbraPhysics.Editor.Classes.Basic
{
    public class ConnectedObject
    {
        public Vector2 Position;
        public object Object;

        public ConnectedObject(object connectObject, Vector2 position)
        {
            Object = connectObject;
            Position = position;
        }

        public void Update()
        {
            if (Object != null)
            {
                if (Object is Light) ((Light)Object).Position = Position;
            }
        }
    }

    #region BodyFlags
    
    public struct BodyFlags
    {
        [Browsable(false)]
        public List<Hull> HullList { get; set; }

        [Browsable(false)]
        public float ShadowHullScale { get; set; }

        [ReadOnly(true)]
        public float StartRotation { get; set; }

        [ReadOnly(true)]
        public Vector2 StartPosition { get; set; }
    }
    
    public struct PivotBodyFlags
    {
        public ConnectedObject ConnectedObject { get; set; }
        public float StartRotation { get; set; }
        public Vector2 StartPosition { get; set; }
    }

    #endregion
}
