using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class CharAnimsDragBon : MonoBehaviour
{
    private UnityArmatureComponent uac;
// Start is called before the first frame update
    void Start()
    {
        DragonBonesData dbd = UnityFactory.factory.LoadDragonBonesData("RedGuy/RedGuy_ske"); // DragonBones file path (without suffix)
        UnityTextureAtlasData tad = UnityFactory.factory.LoadTextureAtlasData("RedGuy/RedGuy_tex"); //Texture atlas file path (without suffix) 

        // Create armature.
        uac = UnityFactory.factory.BuildArmatureComponent("RedGuy");
        // Input armature name

        // Play animation.
        uac.animation.Play("Idle");

        // Change armatureposition.
        uac.transform.localPosition = transform.position;//;new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Change armatureposition.
        uac.transform.localPosition = transform.position;//;new Vector3(0.0f, 0.0f, 0.0f);
    }
}
