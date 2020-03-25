using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.ModelImporters
{
    public class ObjImporter : IService
    {
        private ServiceManager owner;

        public void Cleanup()
        {
        }

        public void Initialize(ServiceManager owner)
        {
            this.owner = owner;
            // make sure that the ObjParser exists
            if (!ServiceManager.ServiceExists<ObjParser>())
            {
                ObjParser parser = new ObjParser();
                ServiceManager.RegisterService(parser);
            }
        }

        public void Import(ObjImportOperation operation)
        {
            owner.StartCoroutine(Fetch(operation.url, (res) =>
            {
                if (res.isNetworkError || res.isHttpError)
                {
                    operation.status = OperationStatus.ERROR;
                    operation.ReturnCallback();
                    return;
                }

                string[] contentLines = res.downloadHandler.text.Split('\n');
                ParseOperation parseOperation = new ParseOperation(contentLines, ProcessParseResult);
            }));
        }

        private void ProcessParseResult(Operation<GeometryConstructor> finishedParseOperation)
        {
            if (finishedParseOperation.status == OperationStatus.ERROR)
            {
                return;
            }


        }

        private IEnumerator Fetch(string url, Action<UnityWebRequest> callback)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            callback(www);
        }


    }

    public class ObjImportOperation : Operation<GameObject>
    {
        public string url;

        public ObjImportOperation(string url, Action<Operation<GameObject>> callback) : base(callback)
        {
            this.url = url;
        }
    }
}