using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var moduleA = new Item("消息服务");
            var moduleG = new Item("短信服务");
            var moduleF = new Item("认证服务", moduleA, moduleG);
            var moduleD = new Item("行情服务", moduleF);
            var moduleC = new Item("交易服务", moduleA, moduleG, moduleD, moduleF);
            var moduleB = new Item("管理前置", moduleA, moduleG, moduleC);
            var moduleE = new Item("交易前置", moduleA, moduleG, moduleC);
            moduleC.Add(moduleB);
            moduleC.Add(moduleE);

            var unsorted = new[] { moduleG, moduleD, moduleC, moduleB, moduleF, moduleE, moduleA };

            var sorted = Sort(unsorted, x => x.Dependencies);

            foreach (var item in sorted)
            {
                Console.WriteLine(item.Name);
            }

            Console.ReadLine();
        }

        public static IList<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        {
            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();


            foreach (var item in source)
            {
                int level = 1;
                Visit(item, getDependencies, sorted, visited, ref level);
            }

            return sorted;
        }

        public static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited, ref int level)
        {


            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            // 如果已经访问该顶点，则直接返回
            if (alreadyVisited)
            {
                // 如果处理的为当前节点，则说明存在循环引用
                if (inProcess)
                {
                    //throw new ArgumentException("Cyclic dependency found.");
                    Console.WriteLine("Cyclic dependency found.");
                }
            }
            else
            {
                // 正在处理当前顶点
                visited[item] = true;

                // 获得所有依赖项
                var dependencies = getDependencies(item);
                // 如果依赖项集合不为空，遍历访问其依赖节点
                if (dependencies != null)
                {
                    if (dependencies.Any())
                    {
                        level++;
                    }

                    Console.WriteLine(item + ":" + level);

                    foreach (var dependency in dependencies)
                    {
                        Console.WriteLine(item + "->" + dependency + "->:" + level);
                        // 递归遍历访问
                        Visit(dependency, getDependencies, sorted, visited, ref level);

                    }

                }

                // 处理完成置为 false
                visited[item] = false;
                sorted.Add(item);

            }
        }
    }
    // Item 定义
    public class Item
    {
        // 条目名称
        public string Name { get; private set; }
        // 依赖项
        public List<Item> Dependencies { get; set; }

        public Item(string name, params Item[] dependencies)
        {
            Name = name;
            Dependencies = dependencies.ToList();
        }
        public void Add(Item item)
        {
            Dependencies.Add(item);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
