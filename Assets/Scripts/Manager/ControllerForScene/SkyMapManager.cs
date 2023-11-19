using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMapManager : BaseController
{
    [SerializeField] private List<Material> listSkyboxMaterial;
    [SerializeField] private Skybox skybox;

    private int _currentIndex = 0;
    
    public void ChangeMap()
    {
        var material = listSkyboxMaterial[(++_currentIndex) % listSkyboxMaterial.Count];
        skybox.material = material;
    }
}