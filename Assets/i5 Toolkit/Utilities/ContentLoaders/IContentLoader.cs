using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Utilities.ContentLoaders
{
    public interface IContentLoader
    {
        Task<Response> LoadAsync(string uri);
    }
}