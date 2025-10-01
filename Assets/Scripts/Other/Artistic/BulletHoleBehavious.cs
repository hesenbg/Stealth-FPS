using UnityEngine;

public class BulletHoleBehaviour : MonoBehaviour
{
    [SerializeField] Material HoleMaterial;       // Base material (used as a template)
    [SerializeField] Texture2D[] HoleTextures;    // List of textures
    [SerializeField] MeshRenderer Hole;           // The bullet hole mesh
    [SerializeField] float BulletHoleLDuration;

    Material[] HoleMaterials; // Store unique materials (1 per texture)

    private void Awake()
    {
        if (HoleTextures == null || HoleTextures.Length == 0 || HoleMaterial == null)
        {
            Debug.LogWarning("Missing material or textures!");
            return;
        }

        // Create a material for each texture
        HoleMaterials = new Material[HoleTextures.Length];
        for (int i = 0; i < HoleTextures.Length; i++)
        {
            HoleMaterials[i] = new Material(HoleMaterial); // clone
            HoleMaterials[i].mainTexture = HoleTextures[i];
        }
    }

    public void ApplyRandomTexture()
    {
        if (HoleMaterials == null || HoleMaterials.Length == 0)
        {
            Debug.LogWarning("HoleMaterials not initialized!");
            return;
        }

        int randomIndex = Random.Range(0, HoleMaterials.Length);

        // Assign one of the prebuilt materials
        Hole.material = HoleMaterials[randomIndex];
    }

    private void Update()
    {
        if (BulletHoleLDuration > 0)
        {
            BulletHoleLDuration -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
