using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VDJ.BuilderGame.Resources;
using DG.Tweening;

public class ResourceIndicator : MonoBehaviour {
    public ResourceDeliveryPoint point;

    public Transform resourceOne;
    public Transform resourceTwo;

    public Sprite r1;
    public Sprite r2;

    private int count = 0;

    public Color completeColor;

    public void OnDataUpdated()
    {
        Debug.Log("Updating Data");
        UpdateToMatch(point.CurrentRequirements);
    }

    public void SetActive(){
        GetComponent<CanvasGroup>().DOFade(0,0.2f).SetDelay(.7f);
    }

    private void UpdateToMatch(IEnumerable<ResourceCost.Requirement> currentRequirements)
    {
        count = 0;
        //var builder = new StringBuilder();

        //foreach (var req in currentRequirements)
        //{
        //    builder.AppendLine(string.Format("{0}: {1}", req.type, req.quantity));
        //}
        //text.text = builder.ToString();

        resourceTwo.DOComplete();
        resourceOne.DOComplete();
        resourceOne.DOPunchScale(Vector3.one / 3, 0.3f, 10, 1);
        resourceTwo.DOPunchScale(Vector3.one / 3, 0.3f, 10, 1);

        foreach (var req in currentRequirements)
        {
            if(count == 0){
                resourceOne.gameObject.SetActive(true);
                if (req.quantity > 0)
                {
                    resourceOne.GetComponentInChildren<SuperTextMesh>().text = req.quantity.ToString();
                }else{
                    resourceOne.GetComponentInChildren<SuperTextMesh>().text = string.Empty;
                    resourceOne.GetComponent<Image>().color = completeColor;
                    resourceOne.transform.Find("Arrow").GetComponent<Image>().color = completeColor;
                    resourceOne.transform.Find("Super Text").GetChild(0).gameObject.SetActive(true);
                }
                resourceOne.transform.Find("ResourceImg").GetComponent<Image>().sprite = (req.type == ResourceToken.Type.Stone) ? r1 : r2;
            }
            if(count == 1){
                resourceTwo.gameObject.SetActive(true);
                if (req.quantity > 0)
                {
                    resourceTwo.GetComponentInChildren<SuperTextMesh>().text = req.quantity.ToString();
                }
                else
                {
                    resourceTwo.GetComponentInChildren<SuperTextMesh>().text = string.Empty;
                    resourceTwo.GetComponent<Image>().color = completeColor;
                    resourceTwo.transform.Find("Arrow").GetComponent<Image>().color = completeColor;
                    resourceTwo.transform.Find("Super Text").GetChild(0).gameObject.SetActive(true);
                }
                resourceTwo.transform.Find("ResourceImg").GetComponent<Image>().sprite = (req.type == ResourceToken.Type.Stone) ? r1 : r2;
            }
            count++;
        }
    }
}
