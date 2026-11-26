using System;
using UnityEngine;

namespace AfterLife.Data
{
    /// <summary>The saved position of one crate.</summary>
    [Serializable]
    public class BlockState
    {
        [SerializeField]
        private string id;
        [SerializeField]
        private Vector3 position;

        /// <summary>Creates the saved state of a crate.</summary>
        /// <param name="id">The crate's unique ID.</param>
        /// <param name="position">Where the crate is.</param>
        public BlockState(string id, Vector3 position)
        {
            this.id = id;
            this.position = position;
        }

        /// <summary>The crate's unique ID.</summary>
        public string Id => id;

        /// <summary>Where the crate was.</summary>
        public Vector3 Position => position;
    }
}