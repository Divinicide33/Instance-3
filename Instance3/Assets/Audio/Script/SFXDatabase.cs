using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SFX Database", menuName = "AudioManager/SFX Database")]
public class SFXDatabase : ScriptableObject
{
    public List<SFXAsset> sfxClips;

    private Dictionary<string, SFXAsset> lookup;

    public SFXAsset GetSFX(string name) 
    {
        if (lookup == null) 
        {
            lookup = new Dictionary<string, SFXAsset>();
            foreach (var sfx in sfxClips) 
            {
                lookup[sfx.sfxName] = sfx;
            }
        }

        return lookup.TryGetValue(name, out var result) ? result : null;
    }
}
