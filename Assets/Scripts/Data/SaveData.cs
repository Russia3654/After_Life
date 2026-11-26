using System;
using System.Collections.Generic;
using UnityEngine;
using AfterLife.Dimensions;

namespace AfterLife.Data
{
    /// <summary>Everything written to the save file.</summary>
    [Serializable]
    public class SaveData
    {
        [SerializeField]
        private string levelId;
        [SerializeField]
        private string checkpointId;
        [SerializeField]
        private Dimension dimension;
        [SerializeField]
        private List<string> collectedIds = new List<string>();
        [SerializeField]
        private List<string> activatedIds = new List<string>();
        [SerializeField]
        private List<string> heldKeyIds = new List<string>();
        [SerializeField]
        private List<BlockState> blocks = new List<BlockState>();

        /// <summary>ID of the level being played.</summary>
        public string LevelId { get => levelId; set => levelId = value; }

        /// <summary>ID of the last checkpoint, or empty for the level start.</summary>
        public string CheckpointId { get => checkpointId; set => checkpointId = value; }

        /// <summary>The dimension to restore.</summary>
        public Dimension Dimension { get => dimension; set => dimension = value; }

        /// <summary>IDs of collected orbs and keys.</summary>
        public List<string> CollectedIds => collectedIds;

        /// <summary>IDs of flipped switches, opened locks and reached checkpoints.</summary>
        public List<string> ActivatedIds => activatedIds;

        /// <summary>IDs of keys collected but not yet used.</summary>
        public List<string> HeldKeyIds => heldKeyIds;

        /// <summary>Saved crate positions.</summary>
        public List<BlockState> Blocks => blocks;
    }
}