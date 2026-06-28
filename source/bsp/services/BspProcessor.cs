using System.Collections.Generic;
using Godot;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Interfaces;

namespace ProcGenLab.BSP.Services;

public class BspProcessor(RandomNumberGenerator rng)
{
    public BspNode GeneratePartitionTree(IBspConfig config)
    {
        Queue<BspNode> queue = new();
        BspNode root = new(new Rect2I(0, 0, config.MapWidth, config.MapHeight));
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (TrySplitNode(currentNode, config))
            {
                queue.Enqueue(currentNode.Left!);
                queue.Enqueue(currentNode.Right!);
            }
        }

        return root;
    }

    private bool TrySplitNode(BspNode node, IBspConfig config)
    {
        if (node.Depth >= config.MaxDepth)
            return false;

        if (!TryGetSplitOrientation(node.Area.Size, config, out var splitVertically))
            return false;

        var minSize = config.MinSplitSize;
        var areaSize = node.Area.Size;
        var maxLineLength = splitVertically ? areaSize.X : areaSize.Y;
        var splitOffset = rng.RandiRange(minSize, maxLineLength - minSize);
        var childDepth = node.Depth + 1;

        if (splitVertically)
        {
            node.Left = new BspNode(
                new Rect2I(node.Area.Position, new Vector2I(splitOffset, areaSize.Y)),
                childDepth
            );

            node.Right = new BspNode(
                new Rect2I(
                    new Vector2I(node.Area.Position.X + splitOffset, node.Area.Position.Y),
                    new Vector2I(areaSize.X - splitOffset, areaSize.Y)
                ),
                childDepth
            );
        }
        else
        {
            node.Left = new BspNode(
                new Rect2I(node.Area.Position, new Vector2I(areaSize.X, splitOffset)),
                childDepth
            );

            node.Right = new BspNode(
                new Rect2I(
                    new Vector2I(node.Area.Position.X, node.Area.Position.Y + splitOffset),
                    new Vector2I(areaSize.X, areaSize.Y - splitOffset)
                ),
                childDepth
            );
        }

        return true;
    }

    private bool TryGetSplitOrientation(Vector2I size, IBspConfig config, out bool splitVertically)
    {
        var canV = size.X > config.MinSplitSize * 2;
        var canH = size.Y > config.MinSplitSize * 2;

        splitVertically = false;

        if (!canV && !canH)
            return false;

        splitVertically = (canV, canH) switch
        {
            (true, false) => true,
            (false, true) => false,
            _ when (float)size.X / size.Y > config.AspectRatioThreshold => true,
            _ when (float)size.Y / size.X > config.AspectRatioThreshold => false,
            _ => rng.Randf() > 0.5f
        };

        return true;
    }
}