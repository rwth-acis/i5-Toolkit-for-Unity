using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjImportTestSceneHandler : MonoBehaviour
{

    public string obj_url;
    public bool useCache;
    public bool persistentSessions;
    public GameObject spawner;
    public int numberToLoad;

    // Start is called before the first frame update
    void Start()
    {
        if (useCache)
        {
            if (persistentSessions)
            {
                FileCache objCache = new FileCache(sessionPersistence: true);
                ServiceManager.RegisterService(objCache);
            }
            else
            {
                FileCache objCache = new FileCache();
                ServiceManager.RegisterService(objCache);
            }
        }
        ObjImporter importer = new ObjImporter();
        ServiceManager.RegisterService(importer);
    }

    // Update is called once per frame
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //start signal
            //Get the Renderer component from the new cube
            var cubeRenderer = spawner.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.red);

            for(int i=0; i<numberToLoad; i++)
            {
                Vector3 currentPosition = new Vector3(spawner.transform.position.x, spawner.transform.position.y + ((i+1) * 2), spawner.transform.position.z);
                GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync(obj_url);
                obj.transform.position = currentPosition;
                obj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            }

            //end signal
            cubeRenderer.material.SetColor("_Color", Color.green);
        }
    }
}
