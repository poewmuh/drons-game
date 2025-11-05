using System;
using System.Collections.Generic;
using UnityEngine;

namespace DronsTeam.Config
{
    [CreateAssetMenu(fileName = "FractionsConfig", menuName = "Data/FractionsConfig")]
    public class FractionConfig : ScriptableObject
    {
        [SerializeField] private List<FractionData> _fractionConfigs;

        public FractionData GetFractionData(int id)
        {
            var fractionConfig = _fractionConfigs.Find(x => x.id == id);
            if (fractionConfig != null) return fractionConfig;
            
            
            Debug.LogError($"[FractionConfig] Could not find fraction config with id {id}");
            return null;
        }
    }

    [Serializable]
    public class FractionData
    {
        public int id;
        public Vector3 fractionPos;
        public Color fractionColor;
    }
}
