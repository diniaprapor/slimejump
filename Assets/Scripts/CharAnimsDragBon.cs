using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

/*
 * todo:
 * + copy green guy atlas to assets
 * - load both atlases and try to swap them
 * + try to unload unused ones
 * - try to play with slot replacement
 * + try changing whole armature
 * */

public class CharAnimsDragBon : MonoBehaviour
{
    private UnityArmatureComponent animComponent;
    private bool isRed = true;
    // Start is called before the first frame update
    void Start()
    {
        
        UnityFactory.factory.LoadDragonBonesData("RedGuy/RedGuy_ske");
        UnityFactory.factory.LoadTextureAtlasData("RedGuy/RedGuy_tex");

        UnityFactory.factory.LoadDragonBonesData("GreenGuy/GreenGuy_ske");
        UnityFactory.factory.LoadTextureAtlasData("GreenGuy/GreenGuy_tex");
        

        animComponent = UnityFactory.factory.BuildArmatureComponent("RedGuy");
        isRed = true;

        animComponent.transform.localPosition = transform.position;
        animComponent.transform.parent = transform;
        //transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
        
        //animComponent = GetComponent<UnityArmatureComponent>();
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

        //test skin change
        if (Input.GetKeyDown(KeyCode.T))
        {
            //animComponent.armature.
            //UnityFactory.factory.
            //animComponent.
            Debug.Log("Anim event skin change attempt");
            string newSkin = isRed ? "GreenGuy" : "RedGuy";
            ArmatureData newArmatureData = UnityFactory.factory.GetArmatureData(newSkin);
            UnityFactory.factory.ReplaceSkin(animComponent.armature, newArmatureData.defaultSkin);
            isRed = !isRed;
        }

        //test data ubnload
        if (Input.GetKeyDown(KeyCode.I))
        {
            DragonBonesDataDebugPrint();

            UnityFactory.factory.RemoveDragonBonesData("GreenGuy");
            UnityFactory.factory.RemoveTextureAtlasData("GreenGuy");

            DragonBonesDataDebugPrint();

        }
    }

    private void DragonBonesDataDebugPrint()
    {
        string result = "Anims info:\n";
        result += "DragonBones data:\n";
        Dictionary<string, DragonBonesData> d = UnityFactory.factory.GetAllDragonBonesData();
        foreach (KeyValuePair<string, DragonBonesData> entry in d)
        {
            result += entry.Key.ToString() + "\n";
        }
        result += "Texture atlases:\n";
        Dictionary<string, List<TextureAtlasData>> t = UnityFactory.factory.GetAllTextureAtlasData();
        foreach (KeyValuePair<string, List<TextureAtlasData>> entry in t)
        {
            result += entry.Key + "\n";
            foreach (TextureAtlasData tad in entry.Value)
            {
                result += tad.name + "\n";
            }
        }
        Debug.Log(result);
    }

    void OnAnimationCompleteEventHandler(string type, EventObject eventObject)
    {
        //Debug.Log("Anim event: " + type);
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
