using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class CharAnimsDragBon : MonoBehaviour
{
    private UnityArmatureComponent animComponent;
    // Start is called before the first frame update
    void Start()
    {
        /*
        DragonBonesData dbd = UnityFactory.factory.LoadDragonBonesData("RedGuy/RedGuy_ske"); // DragonBones file path (without suffix)
        UnityTextureAtlasData tad = UnityFactory.factory.LoadTextureAtlasData("RedGuy/RedGuy_tex"); //Texture atlas file path (without suffix) 

        // Create armature.
        uac = UnityFactory.factory.BuildArmatureComponent("RedGuy");
        // Input armature name

        // Play animation.
        uac.animation.Play("Idle");

        // Change armatureposition.
        uac.transform.localPosition = transform.position;//;new Vector3(0.0f, 0.0f, 0.0f);
        */
        animComponent = GetComponent<UnityArmatureComponent>();
        animComponent.AddDBEventListener(EventObject.LOOP_COMPLETE, OnAnimationCompleteEventHandler);
        animComponent.animation.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        // Change armatureposition.
        //uac.transform.localPosition = transform.position;//;new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetButtonDown("Jump"))
        {
            animComponent.animation.FadeIn("Jump", 0.2f);
        }
    }

    void OnAnimationCompleteEventHandler(string type, EventObject eventObject)
    {
        Debug.Log("Anim event: " + type);
        if (eventObject.animationState.name == "Jump")
        {
            animComponent.animation.FadeIn("Idle", 0.2f);
        }
    }
}

//code pieces
/*
 *     public void SwitchSkin()
    {
        this._skinIndex++;
        this._skinIndex %= SKINS.Length;
        var skinName = SKINS[this._skinIndex];
        var skinData = UnityFactory.factory.GetArmatureData(skinName).defaultSkin;
        List<string> exclude = new List<string>();
        exclude.Add("weapon_l");
        exclude.Add("weapon_r");
        UnityFactory.factory.ReplaceSkin(this._armature, skinData, false, exclude);
    }
    
           if (this._moveDir == 0)
        {
            this._speedX = 0;
            this._armature.animation.FadeIn("idle", -1.0f, -1, 0, NORMAL_ANIMATION_GROUP).resetToPose = false;
            this._walkState = null;
        }

        this._armatureComponent = UnityFactory.factory.BuildArmatureComponent("mecha_1502b");
        this._armature = this._armatureComponent.armature;

        this._armatureComponent.transform.localPosition = new Vector3(0.0f, CoreElement.GROUND, 0.0f);

        this._armatureComponent.AddDBEventListener(DragonBones.EventObject.FADE_IN_COMPLETE, this._OnAnimationEventHandler);

		if(Input.GetMouseButtonDown(0))
		{
			this._mechaArmatureComp.animation.FadeIn("skill_03", 0.2f);
		}
 */
