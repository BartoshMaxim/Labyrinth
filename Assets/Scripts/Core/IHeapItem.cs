using System;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// Heap collection contans HeapItems(items whitch implements IHeapItem interface)
    /// </summary>
    /// <typeparam name="T">implements IHeapItem interface</typeparam>
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get; set;
        }
    }
}
