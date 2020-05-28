using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class MtlParser : IService
{
    public bool ExtendedLogging { get; set; }

    public void Initialize(ServiceManager owner)
    {
    }

    public void Cleanup()
    {
    }

    public List<MtlParseResult> ParseMaterials(string mtlContent, Shader shader)
    {
        int numberOfErrors = 0;

        List<MtlParseResult> parsedMaterials = new List<MtlParseResult>();

        string[] lines = mtlContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        MtlParseResult currentMaterial = null;
        for (int i=0;i<lines.Length;i++)
        {
            string trimmedLine = lines[i].Trim();

            if (trimmedLine.StartsWith("newmtl"))
            {
                if (currentMaterial != null)
                {
                    parsedMaterials.Add(currentMaterial);
                }
                currentMaterial = new MtlParseResult(new Material(shader));
                currentMaterial.material.name = trimmedLine.Substring(6).TrimStart();
            }
            else if (trimmedLine.StartsWith("#"))
            {
                if (ExtendedLogging)
                {
                    i5Debug.Log("Comment found: " + trimmedLine.Substring(1).TrimStart(), this);
                }
            }
            else
            {
                if (currentMaterial != null)
                {
                    if (trimmedLine.StartsWith("Ka"))
                    {
                        string[] strValues = trimmedLine.Substring(2).TrimStart().Split(' ');
                        if (strValues.Length != 3)
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Expected three color values but found " + strValues.Length, this);
                            }
                            continue;
                        }
                        if (ParserUtils.TryParseStringArrayToVector3(strValues, out Vector3 colorVector))
                        {
                            // could successfully parse color vector
                            Color albedo = colorVector.ToColor();
                            currentMaterial.material.color = albedo;
                        }
                        else
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Could not parse color data", this);
                            }
                        }
                    }
                    else if (trimmedLine.StartsWith("Ks"))
                    {
                        string[] strValues = trimmedLine.Substring(2).TrimStart().Split(' ');
                        if (strValues.Length != 3 && strValues.Length != 1)
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Expected one or three smoothness values but found " + strValues.Length, this);
                            }
                            continue;
                        }
                        // we assume that all values are equal
                        if (float.TryParse(strValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float smoothness))
                        {
                            // could successfully parse smoothness
                            currentMaterial.material.SetFloat("_Glossiness", smoothness);
                        }
                        else
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Could not parse color data", this);
                            }
                        }
                    }
                    else if (trimmedLine.StartsWith("map_Kd"))
                    {
                        string texturePath = trimmedLine.Substring(6).TrimStart();
                        currentMaterial.textureMaps.Add("albedo", texturePath);
                    }
                }
            }
        }

        if (currentMaterial != null)
        {
            parsedMaterials.Add(currentMaterial);
        }

        return parsedMaterials;
    }
}
