using System;
using System.Collections;
using System.Collections.Generic;

public class DeckList<T> : List<T>
{
    public void Push(T obj)
    {
        this.Add(obj);
        updateUI();
    }

    public T Draw()
    {
        if (Count <= 0)
            throw new InvalidOperationException();

        T t = this[Count - 1];
        this.RemoveAt(Count - 1);

       updateUI();

        return t;
    }

    public void Push(T t, int howMany)
    {
        for (int i = 0; i < howMany; i++)
            this.Push(t);

        updateUI();
    }

    public T DrawRandom()
    {
        if (Count <= 0)
            throw new InvalidOperationException();

        Random rng = new Random();
        int x = rng.Next(0, Count);
        T t = this[x];
        this.RemoveAt(x);
        updateUI();
        return t;
    }

    //Fisher-Yates Shuffle
    public void Shuffle()
    {
        Random rng = new Random();
        int n = Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = this[k];
            this[k] = this[n];
            this[n] = value;
        }
    }

    private void updateUI(){
        BoardManager bm = BoardManager.GetBoardManager();
        if (bm == null) return;
        if(bm.deck != null && bm.discard != null && bm.texts != null){
            bm.texts[5].text = "" + bm.deck.Size();
            bm.texts[6].text = "" + bm.discard.Size();
        }
    }
}
