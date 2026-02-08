using Godot;
using System.Collections.Generic;

namespace Generics
{
    public static class NodeExtensions
    {
        public static T FindChild<T>(this Node parent) where T : Node
        {
            foreach (Node child in parent.GetChildren())
            {
                if (child is T found)
                    return found;

                T result = child.FindChild<T>();
                if (result != null)
                    return result;
            }
            return null;
        }
        
        public static List<T> FindAllChildren<T>(this Node parent) where T : Node
        {
            List<T> results = new();
            CollectChildren(parent, results);
            return results;
        }

        private static void CollectChildren<T>(Node node, List<T> list) where T : Node
        {
            foreach (Node child in node.GetChildren())
            {
                if (child is T match)
                    list.Add(match);
                CollectChildren(child, list);
            }
        }
    }
}