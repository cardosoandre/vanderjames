using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VDJ.BuilderGame.Resources;

public class ResourceIndicator : MonoBehaviour {
    public ResourceDeliveryPoint point;
    public Text text;

    public void OnDataUpdated()
    {
        UpdateToMatch(point.CurrentRequirements);
    }

    private void UpdateToMatch(IEnumerable<ResourceCost.Requirement> currentRequirements)
    {
        var builder = new StringBuilder();

        foreach (var req in currentRequirements)
        {
            builder.AppendLine(string.Format("{0}: {1}", req.type, req.quantity));
        }
        text.text = builder.ToString();
    }
}
