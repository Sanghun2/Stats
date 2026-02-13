using UnityEngine;

namespace BilliotGames
{
    public class Stat : ValueContainer
    {
        public string ID => id;

        [SerializeField] string id;

        public Stat(string id) {
            this.id = id;
        }

        public Stat(string id, float maxValue) {
            this.id = id;
            this.maxValue = maxValue;            
        }
    }
}
