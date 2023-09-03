using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace GoblinTower.scripts.extensions
{
    public static class Node2DExtensions
    {
        public static Vector2 FromScreenToWorld(this Node2D node, Vector2 screenPos)
        {
            return node.GetCanvasTransform().AffineInverse().Translated(screenPos).Origin;
        }
    }
}