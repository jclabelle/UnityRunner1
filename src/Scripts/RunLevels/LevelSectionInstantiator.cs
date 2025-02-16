using System.Collections.Generic;
using UnityEngine;

namespace RunLevels
{
    [ExecuteInEditMode]
    public class LevelSectionInstantiator : MonoBehaviour
    {
        [field: SerializeField] public GameObject Section { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            if (Application.isPlaying && GetComponent<MeshRenderer>() is MeshRenderer debugSphereRenderer)
                debugSphereRenderer.enabled = false;
        
            if (Application.isPlaying && Section)
            {
                var transform1 = transform;
                var position = transform1.position;
            
                var finalPosition = new Vector3(position.x, Section.transform.position.y, position.z) ;
                var prefab = Instantiate(Section, finalPosition, transform1.rotation);
                prefab.transform.position = position;
            }
        
            if(!Application.isPlaying)
                Debug.Log("NotPlaying");

        }
    
        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying)
            {
                var myCollider = GetComponent<Collider>();
                if (myCollider is { })
                {
                    var surfaces = FindObjectsOfType<RunSurface>();
                    var surfaceColliders = new List<Collider>();
                    foreach (var surface in surfaces)
                    {
                        var collider = surface.GetComponent<Collider>();
                        if(collider is Collider c)
                            surfaceColliders.Add(c);
                    }
                
                    foreach (var surface in surfaces)
                    {
                        var surfaceCollider = surface.GetComponent<Collider>();

                        if (surfaceCollider is { } && myCollider.bounds.Intersects(surfaceCollider.bounds))
                        {
                            transform.rotation = surface.transform.rotation;

                            if (surface.Type == RunSurface.EType.Y0)
                                transform.position = new Vector3(surface.transform.position.x, surface.transform.position.y,
                                    transform.position.z);
                        
                            if (surface.Type == RunSurface.EType.Y90)
                                transform.position = new Vector3(transform.position.x, surface.transform.position.y,
                                    surface.transform.position.z);
                        
                        }

                    }
                }
            
            }
           
        }

        public static GameObject InstantiateSection(GameObject section, Vector3 position, Vector3 rotation)
        {
            
            var finalPosition = new Vector3(position.x, position.y, position.z) ;
            var prefab = Instantiate(section, finalPosition, Quaternion.Euler(rotation));
            return prefab;
        }

    }

    public interface ILevelSection
    {
        
    }
}
