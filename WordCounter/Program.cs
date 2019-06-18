using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(args[0]))
            using (StreamWriter sw = new StreamWriter(args[1]))
            {
                string text = sr.ReadToEnd();
                Regex regex = new Regex("[^a-zA-Z]");
                text = regex.Replace(text.ToLower(), " ");

                string[] words = text.Split(new char[]
                {
                    ' '
                }, StringSplitOptions.RemoveEmptyEntries);

                var query = (from string word in words where word.Length > 1 orderby word select word).Distinct();
                string[] result = query.ToArray();


                LinkedList data = new LinkedList();
                foreach (string word in result)
                {
                    int count = 0;
                    int i = 0;

                    while ((i = text.IndexOf(word, i)) != -1)
                    {
                        i += word.Length;
                        count++;
                    }

                    data.AddLast(word, count);
                    sw.WriteLine("{0} {1}", word, count);
                }

                sw.WriteLine(" ");

                data.head = MergeSort(data.head);
                //Console.WriteLine(data.GetLengthIterative());
                //Console.WriteLine(data.head.GetLengthRecursive());
                //for (int i = 0; i < data.head.GetLengthRecursive(); i++)
                //{
                //    sw.WriteLine($"{data.head.word} , {data.head.count}");
                //}

                Node current = data.head;
                while (current != null)
                {
                    sw.WriteLine($"{current.word} , {current.count}");
                    current = current.Next;
                }
            }
        }

        static Node Merge(Node first, Node second) //merging thte two lists
        {
            Node result = null;
            if (first == null)
            {
                return second;
            }
            if (second == null)
            {
                return first;
            }

            if (first.count >= second.count)
            {
                result = first;
                result.Next = Merge(first.Next, second);
            }
            else
            {
                result = second;
                result.Next = Merge(first, second.Next);
            }

            return result;
        }

        static Node MergeSort(Node node)
        {
            if (node == null || node.Next == null)
            {
                return node;
            }

            Node middle = GetMiddle(node);
            Node nextOfMiddle = middle.Next;

            middle.Next = null;

            Node left = MergeSort(node);
            Node right = MergeSort(nextOfMiddle);

            //merge lists
            Node sortedlist = Merge(left, right);
            return sortedlist;
        }

        static Node GetMiddle(Node node)
        {
            if (node == null) //1  node - already middle
            {
                return node;
            }

            Node fpointer = node.Next;
            Node spoiter = node;

            //moving fpointer by 1 and slowing spointer by 2 will eventually point to the middle
            while (fpointer != null)
            {
                fpointer = fpointer.Next;
                if (fpointer != null)
                {
                    spoiter = spoiter.Next;
                    fpointer = fpointer.Next;
                }
            }
            return spoiter; //aka middle
        }
    }
}

public class Node
{
    public string word { get; set; }
    public int count { get; set; }
    public Node Next { get; set; }
    //public Node(string word, int count)
    //{
    //   this.word = word;
    //   this.count = count;
    //   this.Next = null;
    //}
    public int GetLengthRecursive()
    {
        if (this.Next == null)
        {
            return 1;
        }

        return 1 + this.Next.GetLengthRecursive();
    }

    public override string ToString()
    {
        return $"{word.ToString()} , {count.ToString()}";
    }
}

public class LinkedList
{
    public Node head;

    public void AddLast(string word, int count)
    {
        if (head == null)
        {
            head = new Node();

            head.word = word;
            head.count = count;
            head.Next = null;
        }
        else
        {
            Node toAdd = new Node();
            toAdd.word = word;
            toAdd.count = count;

            Node current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = toAdd;
        }
    }

    public int GetLengthIterative()
    {
        Node current = head;
        int length = 0;
        while (current != null)
        {
            length++;
            current = current.Next;
        }
        return length;
    }
}

