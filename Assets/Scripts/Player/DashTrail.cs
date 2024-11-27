using System.Collections;
using UnityEngine;

public class DashTrail : MonoBehaviour
{
    public float activeTime = 2f;

    public PlayerMovementController pmc;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;
    public Material jumpmat;
    public string shaderVarRef;
    private bool isTrailActive;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    void Start()
    {
        pmc = GetComponent<PlayerMovementController>();

        if (pmc == null) {
            Debug.Log("Cannot Find PlayerMovementController Script");
        }
    }

    void LateUpdate()
    {
        //Start Dashing Trail Effect
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isTrailActive && pmc.IsDashing)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime, false));
        }

        //Start Jumping Trail Effect
        if(Input.GetKeyDown(KeyCode.Space) && !isTrailActive && pmc.HasDJump && pmc.JumpCount == 2)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime, true));
        }
    }

    IEnumerator ActivateTrail (float timeActive, bool isJump) 
    {
        while (timeActive > 0) 
        {
            timeActive -= meshRefreshRate;

            if(skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            //Place and destroy a designated number of copies to represent a trail
            for(int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                //Create copy of player model in current position
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;

                //Add respective transparent glowing material
                if (isJump) {
                    mr.material = jumpmat;
                } else {
                    mr.material = mat;
                }   

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);

            isTrailActive = false;
        }
    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal) 
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
