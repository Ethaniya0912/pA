using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    // Process Instant Effects (Take Damage, Heal)

    // Process Timed Effects (Poison, Build Ups)

    // Process Static Effects (Adding/Removing buffs from talisman ect)

    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        // Take In An Effect
        // Process It
        effect.ProcessEffect(character);
    }
}
