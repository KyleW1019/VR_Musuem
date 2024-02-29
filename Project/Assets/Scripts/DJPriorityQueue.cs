using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DJPriorityQueue
{
    private List<QueueElement> theData;

    public DJPriorityQueue()
    {
        theData = new List<QueueElement>();
    }

    public bool Add(QueueElement qe)
    {
        theData.Add(qe);
        int child = theData.Count - 1;
        int parent = (child - 1) / 2;
        while (parent >= 0 && theData[child].CompareTo(theData[parent]) < 0)
        {
            Swap(parent, child);
            child = parent;
            parent = (child - 1) / 2;
        }
        return true;
    }

    public bool IsEmpty()
    {
        return theData.Count == 0;
    }

    public QueueElement Pop()
    {
        if (IsEmpty()) return null;
        
        var res = theData[0];
        if (theData.Count == 1)
        {
            theData.Remove(res);
            return res;
        }

        theData[0] = theData[^1];
        theData.RemoveAt(theData.Count - 1);
        int parent = 0;

        while (true)
        {
            int leftChild = parent * 2 + 1;
            if (leftChild >= theData.Count) break;
            int rightChild = leftChild + 1;
            
            int minChild = leftChild;
            if (rightChild < theData.Count && theData[rightChild].CompareTo(theData[leftChild]) < 0)
                minChild = rightChild;

            if (theData[parent].CompareTo(theData[minChild]) > 0)
            {
                Swap(parent, minChild);
                parent = minChild;
            }
            else
            {
                break;
            }
        }

        return res;
    }

    private void Swap(int a, int b)
    {
        (theData[a], theData[b]) = (theData[b], theData[a]);
    }
}
