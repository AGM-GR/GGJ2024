using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private List<EffectsPlayer> _effects;

    private Dictionary<string, EffectsPlayer> _idToEffectPlayer;

    private void Awake()
    {
        _idToEffectPlayer = _effects.ToDictionary(x => x.Id, x => x);
    }

    public void Play(string id)
    {
        if (!_idToEffectPlayer.ContainsKey(id)) return;
        _idToEffectPlayer[id].Play();
    }

    public void Stop(string id)
    {
        if (!_idToEffectPlayer.ContainsKey(id)) return;
        _idToEffectPlayer[id].Stop();
    }
}
