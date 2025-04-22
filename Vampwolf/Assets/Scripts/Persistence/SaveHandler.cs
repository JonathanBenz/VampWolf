using UnityEngine;
using Vampwolf.Persistence;

namespace Vampwolf
{
    /// <summary>
    /// Generic inheritance class for all save handlers to allow Guid editor extensions
    /// to pass to their inspectors
    /// </summary>
    public class SaveHandler : MonoBehaviour 
    {
        [field: SerializeField] public SerializableGuid ID { get; set; } = SerializableGuid.NewGuid();
    }
}
