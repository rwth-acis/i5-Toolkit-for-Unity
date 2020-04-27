using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MtlParser : IService
{
    public bool ExtendedLogging { get; set; }

    public void Initialize(ServiceManager owner)
    {
    }

    public void Cleanup()
    {
    }

    public async Task<Material> CreateMaterial(string mtlContent)
    {
        int numberOfErrors = 0;

        string[] lines = mtlContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        Material mat = new Material(Shader.Find("Standard"));
        for (int i=0;i<lines.Length;i++)
        {
            string trimmedLine = lines[i].Trim();

            if (trimmedLine.StartsWith("newmtl"))
            {
                mat.name = trimmedLine.Substring(6).TrimStart();
            }
            else if (trimmedLine.StartsWith("#"))
            {
                if (ExtendedLogging)
                {
                    i5Debug.Log("Comment found: " + trimmedLine.Substring(1).TrimStart(), this);
                }
            }
            else if (trimmedLine.StartsWith("Ka"))
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
                    mat.color = albedo;
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
                if (float.TryParse(strValues[0], out float smoothness))
                {
                    // could successfully parse smoothness
                    mat.SetFloat("_Glossiness", smoothness);
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
                i5Debug.Log(texturePath, this);
            }
        }

        return mat;
    }
}
