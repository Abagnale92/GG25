using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


[RequireComponent(typeof(Slider))]
public class VolumeSlider : UIElement
{
#if UNITY_EDITOR

    [MenuItem("GameObject/UI/UIAddons/Volume Slider")]
    public static void AddVolumeSlider()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UIAddonsPrefabs/Volume Slider"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

#endif

    public AudioCategory category;
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = AudioManager.instance.GetAudioVolume(category);
    }


    public void UpdateAudioVolume(System.Single volume)
    {
        AudioManager.instance.SetVolume(category, volume);
    }
}
