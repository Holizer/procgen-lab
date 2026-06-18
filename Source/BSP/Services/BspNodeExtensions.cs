using System.Collections.Generic;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.BSP.Services;

public static class BspNodeExtensions
{
    public static Room GetLeafRoom(this BspNode node, Direction dir)
    {
        if (node.IsLeaf)
            return node.Room;

        var lookInLeft = node.IsHorizontal
            ? dir == Direction.North
            : dir == Direction.West || dir == Direction.North || dir == Direction.South;

        return lookInLeft ? node.Left.GetLeafRoom(dir) : node.Right.GetLeafRoom(dir);
    }

    public static IEnumerable<BspNode> GetLeaves(this BspNode root)
    {
        var stack = new Stack<BspNode>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (node.IsLeaf)
            {
                yield return node;
                continue;
            }

            if (node.Right != null)
                stack.Push(node.Right);
            if (node.Left != null)
                stack.Push(node.Left);
        }
    }
}