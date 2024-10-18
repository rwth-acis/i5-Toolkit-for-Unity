using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using UnityEngine;

public class ObjImportTestSceneHandler : MonoBehaviour
{
    [SerializeField]
    public string obj_url;
    [SerializeField]
    private bool useCache;
    [SerializeField]
    private bool persistentSessions;
    [SerializeField]
    private GameObject spawner;
    [SerializeField]
    private int numberToLoad;

    private Renderer cubeRenderer;


    private void Awake()
    {
        cubeRenderer = spawner.GetComponent<Renderer>();
    }

    private void Start()
    {
        if (useCache)
        {
            FileCacheService objCache = new FileCacheService(sessionPersistence: persistentSessions);
            ServiceManager.RegisterService(objCache);
        }
        ObjImporter importer = new ObjImporter();
        ServiceManager.RegisterService(importer);
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //start signal
            //Get the Renderer component from the new cube
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
