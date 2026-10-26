using UnityEngine;
using System;
using TableSO.Scripts;

namespace Table
{
    public class IconAssetTableSO : TableSO.Scripts.AssetTableSO<TableData.IconAsset>
    {
        public override TableType tableType => TableType.Asset;

        [SerializeField] private string assetFolderPath = "Assets/TableSO/Asset/Icon";
        public override string label { get => "IconAssetTableSO"; }
        public override Type assetType { get => typeof(Sprite); }
    }
}
