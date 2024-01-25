using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIDescriptor : MonoBehaviour
{
    //universal thing for descriptions.

    GameObject holder;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Transform[] containers;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void ControlHolder(bool choice) => holder.SetActive(choice);
    public void UpdateBasic(Sprite portraitSprite, string name, string description)
    {
        UpdatePortrait(portraitSprite);
        UpdateName(name);
        UpdateDescription(description);
    }
    public void UpdateContainer(int index, List<string> contentList)
    {
        if(index > containers.Length)
        {
            Debug.LogError("something wrong");
            return;
        }
        if (containers[index] == null)
        {
            Debug.LogError("didnt find");
            return;
        }

        ClearUI(containers[index]);

        foreach (var item in contentList)
        {
            TextMeshProUGUI newObject = Instantiate(nameText, new Vector3(0, 0, 0), Quaternion.identity);
            newObject.text = item;
            newObject.transform.parent = containers[index];
        }

    }
    void ClearUI(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    public void UpdatePortrait(Sprite portraitSprite)
    {
        if (portrait != null) portrait.sprite = portraitSprite;
    }
    public void UpdateName(string name)
    {
        if (nameText != null) nameText.text = name;
    }
    public void UpdateDescription(string description)
    {
        if (descriptionText != null) descriptionText.text = description;
    }





}
