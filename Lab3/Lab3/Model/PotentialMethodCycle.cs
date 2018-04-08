using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    class PotentialMethodCycle
    {
        public class CycleItem
        {
            public double Cost;
            public double Count;
            public int i;
            public int j;
            public bool IsPositive;
            public CycleItem From;
            public List<CycleItem> To = new List<CycleItem>();
            public bool IsUsed;

            public override bool Equals(object obj)
            {
                var item = obj as CycleItem;
                if (item == null)
                    return false;
                return i == item.i && j == item.j;
            }
        }

        public CycleItem First;
        public List<CycleItem> Items;
        public List<CycleItem> ItemPool;

        double[,] count;
        double[,] cost;

        int rawCount;
        int needCount;

        bool FoundStart = false;

        public void CreateCycle(CycleItem first, double[,] Count, double[,] Cost)
        {
            Items = new List<CycleItem>();
            ItemPool = new List<CycleItem>();
            ItemPool.Add(first);

            rawCount = Count.GetLength(0);
            needCount = Count.GetLength(1);
            count = Count;
            cost = Cost;
            CreateItemPool();
            //SetPossibleNextItems();
            First = ItemPool.Where(ci => ci.i == first.i && ci.j == first.j).First();

            FoundStart = false;
            Iteration(First);

            //set signs
            for (int i = 0; i < Items.Count(); i++)
                Items[i].IsPositive = i % 2 == 0;
        }

        void Iteration(CycleItem currentItem)
        {
            currentItem.To = new List<CycleItem>();
            for (int j = 0; j < ItemPool.Count(); j++)
                if (CanConnectItems(currentItem, ItemPool[j]) &&
                    /*!ItemPool[j].To.Contains(currentItem)*/
                    !ItemPool[j].Equals(currentItem.From))
                    currentItem.To.Add(ItemPool[j]);

            if (currentItem.Equals(First) && Items.Count > 0)
            {
                FoundStart = true;
                return;
            }
            if (currentItem.To.Count() == 0)
                return;

            currentItem.IsUsed = true;
            Items.Add(currentItem);

            for (int i = 0; i < currentItem.To.Count(); i++)
            {
                if (currentItem.To[i].Equals(First))
                {
                    FoundStart = true;
                    return;
                }

                if (!currentItem.To[i].IsUsed)
                {
                    currentItem.To[i].From = currentItem;
                    Iteration(currentItem.To[i]);

                    if (FoundStart)
                        return;
                    else
                        currentItem.To[i].From = null;
                }
            }

            currentItem.To = new List<CycleItem>();
            currentItem.IsUsed = false;
            Items.RemoveAt(Items.Count() - 1);
        }

        void SetPossibleNextItems()
        {
            for (int i = 0; i < ItemPool.Count(); i++)
            {
                ItemPool[i].To = new List<CycleItem>();
                for (int j = 0; j < ItemPool.Count(); j++)
                    if (CanConnectItems(ItemPool[i], ItemPool[j]) &&
                        !ItemPool[j].To.Contains(ItemPool[i]))
                        ItemPool[i].To.Add(ItemPool[j]);
            }
        }

        public bool CanConnectItems(CycleItem From, CycleItem To)
        {
            if (From.Equals(To))
                return false;

            if(From.j == To.j)
            {
                //search bottom
                if(From.i < To.i)
                {
                    //another item lower
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To) && ci.IsUsed
                            && ci.i > From.i && ci.i < To.i && ci.j == From.j)
                            return false;
                    //another item lower
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To)
                            && ci.i > To.i && ci.j == From.j)
                            return false;
                }
                //search top
                else
                {
                    //another item higher
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To) && ci.IsUsed
                            && ci.i < From.i && ci.i > To.i && ci.j == From.j)
                            return false;
                    //another item higher
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To)
                            && ci.i < To.i && ci.j == From.j)
                            return false;
                }
                return true;
            }
            else if(From.i == To.i)
            {
                //search right
                if (From.j < To.j)
                {
                    //another item righter
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To) && ci.IsUsed
                            && ci.j > From.j && ci.j < To.j && ci.i == From.i)
                            return false;
                    //another item righter
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To)
                            && ci.j > To.j && ci.i == From.i)
                            return false;
                }
                //search left
                else
                {
                    //another item lefter
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To) && ci.IsUsed
                            && ci.j < From.j && ci.j > To.j && ci.i == From.i)
                            return false;

                    //another item lefter
                    foreach (var ci in ItemPool)
                        if (!ci.Equals(From) && !ci.Equals(To)
                            && ci.j < To.j && ci.i == From.i)
                            return false;
                }
                return true;
            }

            return false;
        }

        public void CreateItemPool()
        {
            for (int i = 0; i < rawCount; i++)
                for (int j = 0; j < needCount; j++)
                    if (!count[i, j].Equals(Double.NaN) &&
                        ItemPool.Where(ci => ci.i == i && ci.j == j).Count() == 0)
                        ItemPool.Add(new CycleItem
                        {
                            Cost = cost[i, j],
                            Count = count[i, j],
                            i = i,
                            j = j
                        });
        }
    }
}
