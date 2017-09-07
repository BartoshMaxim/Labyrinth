using Assets.Scripts.Core;
using System;

/// <summary>
/// Heap item list(created to speed up the work of the list, use binary tree)
/// </summary>
/// <typeparam name="T"> T must implements IHeapItem interface</typeparam>
public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);

    }
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }



    void SortDown(T item)
    {
        while (true)
        {
            int childrenIndexLeft = item.HeapIndex * 2 + 1;
            int childrenIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childrenIndexLeft < currentItemCount)
            {
                swapIndex = childrenIndexLeft;
                if (childrenIndexRight < currentItemCount)
                {
                    if (items[childrenIndexLeft].CompareTo(items[childrenIndexLeft]) < 0)
                    {
                        swapIndex = childrenIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else return;
            }
            else return;
        }
    }


    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
                break;
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}
