using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void GetTreeTest()
        {
            try

            {
                var list = new List<TreeEntity>
            {
                new TreeEntity{Id=1,Name=1,ParentId=0},
                new TreeEntity{Id=2,Name=1,ParentId=0},
                new TreeEntity{Id=11,Name=1,ParentId=1},
                new TreeEntity{Id=12,Name=1,ParentId=1},
                new TreeEntity{Id=21,Name=1,ParentId=2},
                new TreeEntity{Id=22,Name=1,ParentId=2}
            };
                var result = GetTree<TreeEntity>(list,a=>a.Id,a=>a.ParentId,0);
            }

            catch (Exception ss)
            {

                var a = ss;
            }

        }

        private bool eq<T>(T a,T b) 
        {
            return a.Equals(b);
        }
        private TreeNode<T> GetTree<T>(List<T> list, Func<T, object> IdFunc, Func<T, object> parentIdFunc, object nodeId) where T :class
        {
            var data = list.FirstOrDefault(a=>IdFunc(a).Equals(nodeId));
            var parent = data == null ? null : list.FirstOrDefault(a => IdFunc(a).Equals(parentIdFunc(data)));
            var childs = list.Where(a =>parentIdFunc(a).Equals(nodeId)).Select(a => GetTree(list, IdFunc,parentIdFunc,IdFunc(a))).ToList();
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

    public class TreeEntity
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int ParentId { get; set; }
    }
}
