using System;

namespace Calculator.Stack
{
    public class MyStack<T>
    {
        private Node<T> _head;
        private int _count;

        public bool IsEmpty => _count == 0;

        public int Count => _count;

        public void Push(T item)
        {
            Node<T> node = new Node<T>(item);
            node.Next = _head;

            _head = node;
            _count++;
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");

            Node<T> temp = _head;

            _head = _head.Next;
            _count--;

            return temp.Data;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");

            return _head.Data;
        }

        public void Clear()
        {
            while (!IsEmpty)
                Pop();
        }
    }
}