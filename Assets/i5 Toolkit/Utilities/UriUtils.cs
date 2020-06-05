using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UriUtils
{
    public static string RewriteUriPath(Uri uri, string relativeFilePath)
    {
        string mtlLibUrl = uri.GetLeftPart(UriPartial.Authority);
        // add all segments except of the last one which is the file name
        for (int i = 0; i < uri.Segments.Length - 1; i++)
        {
            mtlLibUrl += uri.Segments[i];
        }
        mtlLibUrl += relativeFilePath;
        mtlLibUrl += uri.Query;
        return mtlLibUrl;
    }
}
