using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class ItemTableSO : TableSO.Scripts.CsvTableSO<int, TableData.Item>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "Item"; }

        [SerializeField] private List<ItemData> itemDataList = new();
        [ShowInInspector] public Dictionary<ItemType, List<ItemData>> itemDataDict = new Dictionary<ItemType, List<ItemData>>();

        public override async Task UpdateData()
        {
            await base.UpdateData();
            
            ParseToItemList();
            
            ReleaseData();
        }

        private void ParseToItemList()
        {
            itemDataList.Clear();
            itemDataDict.Clear();

            foreach (var data in dataList)
                itemDataList.Add(GetItem(data));
        }

        private ItemData GetItem(Item item)
        {
            ItemData itemData = new ItemData(item.ID.ToString(), item.Type, item.SpritePath,
                item.Name, item.Description, GetItemStatData(item));
            
            return itemData;
        }

        private List<ItemStatData> GetItemStatData(Item item)
        {
            List<ItemStatData> list = new();

            if (item.Status == null || item.Values == null || item.Durations == null)
                return list;

            int count = Mathf.Min(item.Status.Length, item.Values.Length, item.Durations.Length);

            for (int i = 0; i < count; i++)
            {
                ItemStatData data = new ItemStatData(item.Status[i], item.Values[i], item.Durations[i]);
                list.Add(data);
                Debug.Log($"[ItemTableSO] Item {item.ID}: Stat={item.Status[i]}, Value={item.Values[i]}, Duration={item.Durations[i]}");
            }

            return list;
        }
        
        public ItemData GetItem(int id)
        {
            return itemDataList.Find(item => item.ItemID == id.ToString());
        }

        public Dictionary<ItemType, List<ItemData>> GetItemsByDict()
        {
            if (itemDataDict.Count != 0)
                return itemDataDict;
        
            foreach (var itemData in itemDataList)
            {
                if (itemData.Type == ItemType.None)
                {
                    continue;
                }

                foreach (ItemType flag in System.Enum.GetValues(typeof(ItemType)))
                {
                    if (flag == ItemType.None) continue;

                    bool isSingleFlag = ((int)flag & ((int)flag - 1)) == 0;
                    if (!isSingleFlag) continue;

                    if ((itemData.Type & flag) == flag)
                    {
                        if (!itemDataDict.ContainsKey(flag)) itemDataDict[flag] = new List<ItemData>();
                        itemDataDict[flag].Add(itemData);
                    }
                }
            }
        
            return itemDataDict;
        }
    }
}
