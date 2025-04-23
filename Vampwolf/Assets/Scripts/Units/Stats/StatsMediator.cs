using System;
using System.Collections.Generic;

namespace Vampwolf.Units.Stats
{
    public class StatsMediator
    {
        private readonly LinkedList<StatModifier> modifiers;
        public event EventHandler<Query> Queries;

        public int CurrentModifiers => modifiers.Count;

        public StatsMediator()
        {
            modifiers = new LinkedList<StatModifier>();
        }

        /// <summary>
        /// Perform a query using a sender and the query
        /// </summary>
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

        /// <summary>
        /// Add a modifier
        /// </summary>
        public void AddModifier(StatModifier modifier)
        {
            // Add the modifier to the last position of the list
            modifiers.AddLast(modifier);

            // Add the modifiers handle to the query
            Queries += modifier.Handle;

            // Link the modifier's disposal event
            modifier.OnDispose += _ =>
            {
                // Remove the modifier from the list
                modifiers.Remove(modifier);

                // Deregister the modifier's handle from the queries
                Queries -= modifier.Handle;
            };

            UnityEngine.Debug.Log($"Added modifier - {modifiers.Count}");
        }

        public void Update()
        {
            // Get the first node
            LinkedListNode<StatModifier> node = modifiers.First;

            // Iterate while there are nodes left in the list
            while(node != null)
            {
                // Get the modifier from the node
                StatModifier modifier = node.Value;

                // Update the remaining turns
                modifier.UpdateRemainingTurns();

                // Move down the list
                node = node.Next;
            }

            // We now need to check for removal, so get the first node again
            node = modifiers.First;

            // Iterate while there are nodes left in the list
            while(node != null)
            {
                // Get the next node
                LinkedListNode<StatModifier> nextNode = node.Next;

                // Check if the current node is marked for removal
                if(node.Value.MarkedForRemoval)
                {
                    // Dispose of the node
                    node.Value.Dispose();
                }

                // Set the current node to the next node
                node = nextNode;
            }
        }
    }
}
