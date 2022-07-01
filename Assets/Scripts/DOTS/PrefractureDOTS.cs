using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(Rigidbody))]
public class PrefractureDOTS : MonoBehaviour
{

    public FractureOptionsDOTS fractureOptions;
    public CallbackOptionsDOTS callbackOptions;

    /// <summary>
    /// Collector object that stores the produced fragments
    /// </summary>
    private GameObject fragmentRoot;
    private GameObject originalGameObject;
    private List<GameObject> chunks = new List<GameObject>();
    public DestructableObjectController destObjCon;
 

    public bool fractureAgain;

    private void Awake()
    {
        if (Application.isPlaying)
        {
            if(this.gameObject.GetComponent<Rigidbody>() != null)
            {
                Destroy(this.gameObject.GetComponent<Rigidbody>());
            }
        }
    }


    void OnValidate()
    {
        if (this.transform.parent != null)
        {
            // When an object is fractured, the fragments are created as children of that object's parent.
            // Because of this, they inherit the parent transform. If the parent transform is not scaled
            // the same in all axes, the fragments will not be rendered correctly.
            var scale = this.transform.parent.localScale;
            if ((scale.x != scale.y) || (scale.x != scale.z) || (scale.y != scale.z))
            {
                Debug.LogWarning($"Warning: Parent transform of fractured object must be uniformly scaled in all axes or fragments will not render correctly.", this.transform);
            }
        }
    }

    /// <summary>
    /// Compute the fracture and create the fragments
    /// </summary>
    /// <returns></returns>
    [ExecuteInEditMode]
    [ContextMenu("Prefracture DOTS")]
    public void ComputeFracture()
    {
        // This method should only be called from the editor during design time
        if (!Application.isEditor || Application.isPlaying) return;

        if (chunks.Count != 0)
        {
            for(int i = chunks.Count - 1; i > -1; i--)
            {
                DestroyImmediate(chunks[i]);
                chunks.RemoveAt(i);
            }
        }
    
            originalGameObject = this.gameObject;
            var mesh = this.GetComponent<MeshFilter>().sharedMesh;
            var meshRen = this.GetComponent<MeshRenderer>();
            Collider m_Collider;
            m_Collider = GetComponent<Collider>();

            if (mesh != null)
            {
            // Create a game object to contain the fragments
            if (this.gameObject.GetComponent<DestructableObjectController>() == null)
            {
                destObjCon = gameObject.AddComponent<DestructableObjectController>();
            }
                meshRen.enabled = false;
                m_Collider.enabled = false;

                var fragmentTemplate = CreateFragmentTemplate();

                FragmenterDOTS.Fracture(this.gameObject,
                                    this.fractureOptions,
                                    fragmentTemplate,
                                    this.transform);

                // Done with template, destroy it. Since we're in editor, use DestroyImmediate
                DestroyImmediate(fragmentTemplate);


                // Fire the completion callback
                if (callbackOptions.onCompleted != null)
                {
                    callbackOptions.onCompleted.Invoke();
                }
            }

            if (mesh == null)
            {
                throw new Exception("Object has no mesh to Prefracture, please add a mesh.");
            }


            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {

                GameObject child = originalGameObject.transform.GetChild(i).gameObject;
                if (child.name.Contains("Fragment"))
                {
                    chunks.Add(child);
                }
                //Do something with child
            }


        }
    


    /// <summary>
    /// Creates a template object which each fragment will derive from
    /// </summary>
    /// <returns></returns>
    private GameObject CreateFragmentTemplate()
    {
        // If pre-fracturing, make the fragments children of this object so they can easily be unfrozen later.
        // Otherwise, parent to this object's parent
        GameObject obj = new GameObject();
        obj.name = "Fragment";
        obj.tag = this.tag;
  
        // Update mesh to the new sliced mesh
        obj.AddComponent<MeshFilter>();

        // Add renderer. Default material goes in slot 1, cut material in slot 2
        var meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = new Material[2] {
            this.GetComponent<MeshRenderer>().sharedMaterial,
            this.fractureOptions.insideMaterial
        };

        // Copy collider properties to fragment
        var thisCollider = this.GetComponent<Collider>();
        var fragmentCollider = obj.AddComponent<MeshCollider>();
        fragmentCollider.convex = true;
        fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;
        fragmentCollider.isTrigger = thisCollider.isTrigger;

       // Copy rigid body properties to fragment
        var rigidBody = obj.AddComponent<Rigidbody>();
       // When pre-fracturing, freeze the rigid body so the fragments don't all crash to the ground when the scene starts.
        rigidBody.drag = this.GetComponent<Rigidbody>().drag;
        rigidBody.angularDrag = this.GetComponent<Rigidbody>().angularDrag;
        rigidBody.useGravity = this.GetComponent<Rigidbody>().useGravity;

        //Add Fracture component to all fragments
        if(fractureAgain)
        {



            if (destObjCon.triggerOptions.triggerAllowedTags.Count != 0)
            {
                //for(int i = 0; i < destObjCon.triggerOptions.triggerAllowedTags.Count; i++)
                //{
                obj.AddComponent<FractureDOTS>();
               var fracture = obj.AddComponent<FractureDOTS>();
                if (fracture != null)
                {
                    //Debug.Log("Fuck you");
                }

                var copy = new List<String>(destObjCon.triggerOptions.triggerAllowedTags);
                    Debug.Log("Iterate through trigger options!");
                if(obj.GetComponent<FractureDOTS>() == null)
                {
                    Debug.Log("Fuck you");
                }
                    obj.GetComponent<FractureDOTS>().triggerOptions.triggerAllowedTags.AddRange(copy);
             
                
                //fracture.triggerOptions.triggerAllowedTags = this.GetComponent<DestructableObjectController>().triggerOptions.triggerAllowedTags;
  
                //}
            }
        }
        //var unfreeze = obj.AddComponent<UnfreezeFragmentDOTS>();
        //unfreeze.unfreezeAll = prefractureOptions.unfreezeAll;
        //unfreeze.triggerOptions = this.triggerOptions;
        //unfreeze.onFractureCompleted = callbackOptions.onCompleted;

        return obj;
    }
}