﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snail.Common
{
    public class TreeNodeHelper
    {
        public static TreeNode<T> GetTree<T>(List<T> list, Func<T, object> IdFunc, Func<T, object> parentIdFunc, object nodeId) where T : class
        {
            var data = list.FirstOrDefault(a => IdFunc(a).Equals(nodeId));
            var parent = data == null ? null : list.FirstOrDefault(a => IdFunc(a).Equals(parentIdFunc(data)));
            var childs = list.Where(a => parentIdFunc(a).Equals(nodeId)).Select(a => GetTree(list, IdFunc, parentIdFunc, IdFunc(a))).ToList();
            return new TreeNode<T>
            {
                Data = data,
                Parent = parent,
                Childs = childs
            };
        }
    }

    public class TreeNode<T>
    {
        public T Data { get; set; }
        public T Parent { get; set; }
        public List<TreeNode<T>> Childs { get; set; }
    }
}